using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Persistence;

namespace Application.Features.Activities
{
    public class LoudActivitiesFromFile
    {
        public class ActivitiesAdapter
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime ExpectedStarAt { get; set; }
            public DateTime ExpectedEndAt { get; set; }
            
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
               

                var createdBy = await _userManager.Users.FirstOrDefaultAsync(x =>
                    x.Email == _userAccessor.GetCurrentUserEmail(), cancellationToken: cancellationToken);
                
                var fileStream = ConvertFile(request.File);
                var data = ReadXlsx(fileStream);

                foreach (var item in data)
                {
                    var activityExists = await _context.Activities
                        .Include(x=>x.Component)
                        .FirstOrDefaultAsync(x => x.Name.Equals(item.Name) && x.Component.Id == request.ComponentId,
                            cancellationToken: cancellationToken);
                    if (activityExists == null)
                    {
                        if(component == null)
                            throw new WebException("Status not found", 
                                (WebExceptionStatus) HttpStatusCode.NotFound);
                
                        if (item.ExpectedEndAt < item.ExpectedStarAt)
                        {
                            throw new WebException("A data esperada de termino deve ser maior ou igual que a data esperada de inicio", 
                                (WebExceptionStatus) HttpStatusCode.NotFound);
                        }
                        if (item.ExpectedStarAt < component.ExpectedStartDate || item.ExpectedStarAt > component.ExpectedEndDate)
                        {
                            throw new WebException("A data esperada nao deve estar fora do escopo do componente", 
                                (WebExceptionStatus) HttpStatusCode.NotFound);
                        }
                        if (item.ExpectedEndAt  < component.ExpectedStartDate || item.ExpectedEndAt >component.ExpectedEndDate)
                        {
                            throw new WebException("A data esperada nao deve estar fora do escopo do componente", 
                                (WebExceptionStatus) HttpStatusCode.NotFound);
                        }
                        var activity = new Activity()
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Status = status,
                            Component = component,
                            CreatedBy = createdBy,
                            ExpectedEndAt = item.ExpectedEndAt,
                            ExpectedStarAt = item.ExpectedStarAt,
                            CreatedAt = DateTime.Now
                        };
                        await _context.AddAsync(activity, cancellationToken);
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
                    /*if(worksheet.Cells.Any(c => string.IsNullOrEmpty(c.Text))) {
                        break;
                    }*/
                    var activity = new ActivitiesAdapter()
                    {
                        Name = worksheet.Cells[row, 1].Value?.ToString(),
                        Description = worksheet.Cells[row, 2].Value?.ToString(),
                        ExpectedStarAt = (DateTime) worksheet.Cells[row, 3]?.Value!,
                        ExpectedEndAt = (DateTime) worksheet.Cells[row, 4]?.Value!
                    };
                    
                    result.Add(activity);
                }
            }

            return result;
        }
    }
}