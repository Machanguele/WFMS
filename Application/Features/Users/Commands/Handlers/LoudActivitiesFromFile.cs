using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

using Application.Features.Activities;
using Application.Interfaces;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace Application.Features.Users.Commands.Handlers
{
    public class LoudActivitiesFromFile
    {
        public class ActivitiesAdapter
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int ActivityTypeId { get; set; }
        }
        public class LoudActivitiesFromFileCommand : IRequest<string>
        {
            public LoudActivitiesFromFileCommand(IFormFile file, int componentId)
            {
                File = file;
                ComponentId = componentId;
            }
            public int ComponentId { get; set; }
            public IFormFile File { get; set; }
        }

        public class LoudActivitiesFromFileCommandValidator : AbstractValidator<LoudActivitiesFromFileCommand>
        {
            public LoudActivitiesFromFileCommandValidator()
            {
                RuleFor(x => x.File).NotEmpty();
                RuleFor(x => x.ComponentId).NotEmpty();
            }
        }

        public class LoudActivitiesFromFileCommandHandler : IRequestHandler<LoudActivitiesFromFileCommand, string>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;

            public LoudActivitiesFromFileCommandHandler(DataContext context, IUserAccessor userAccessor, 
                UserManager<AppUser> userManager)
            {
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
            }
            public async Task<string> Handle(LoudActivitiesFromFileCommand request, CancellationToken cancellationToken)
            {
                var status = await _context.ActivityStatuses.FirstOrDefaultAsync(x => x.Name == "Por fazer");
                if(status == null)
                    throw new WebException("Status not found", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                var component = await _context.Components.FirstOrDefaultAsync(x => x.Id == request.ComponentId);
                if(component == null)
                    throw new WebException("Status not found", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);

                var createdBy = await _userManager.Users.FirstOrDefaultAsync(x =>
                    x.Email == _userAccessor.GetCurrentUserEmail(), cancellationToken: cancellationToken);
                
                var fileStream = ConvertFile(request.File);
                var data = ReadXlsx(fileStream);

                foreach (var item in data)
                {
                    var activityExists = await _context.Activities.FirstOrDefaultAsync(x => x.Name.Equals(item.Name)) == null;
                    if (!activityExists)
                    {
                        var activity = new Activity()
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Status = status,
                            Component = component,
                            CreatedBy = createdBy,
                        };
                        await _context.AddAsync(activity);
                }
            }
                
                if (await _context.SaveChangesAsync(cancellationToken) > 0) return "Criado com Sucesso";

                throw new WebException("Falha ao carregar", 
                    (WebExceptionStatus) HttpStatusCode.InternalServerError);
            }
        }

        public static MemoryStream ConvertFile(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                file?.CopyTo(stream);
                var byteArray = stream.ToArray();
                
                
                return new MemoryStream(byteArray);
            }
        }

        public static List<ActivitiesAdapter> ReadXlsx(Stream stream)
        {
            var result = new List<ActivitiesAdapter>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;
                
                for (int row = 2; row<= rowCount; row++)
                {
                    var activity = new ActivitiesAdapter()
                    {
                        Name = worksheet.Cells[row, 1].Value.ToString(),
                        Description = worksheet.Cells[row, 2].Value.ToString()
                    };
                    
                    result.Add(activity);
                }
            }

            return result;
        }
    }
}