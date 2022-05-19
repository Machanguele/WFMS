using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace API.Workers
{
    public class UpdateSystemUsersWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public UpdateSystemUsersWorker(IServiceScopeFactory serviceScopeFactory
            , IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }
        protected override  async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();
                var clientAdapter = services.GetRequiredService<IHttpClientHelper>();
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    var userEcoSystems = await context.UserSystems
                        .Include(x => x.AppUser)
                        .Include(x => x.EcoSystem)
                        .Where(x=>!x.LastUpdateCommittedToTargetSystem)
                        .ToListAsync();

                    var timer = Convert.ToDouble(_configuration["updateSystemTimer"]);
                    Console.WriteLine("Ready to send requests");
                    /*Console.WriteLine("Updating Systems");
                    var response =  await clientAdapter.DoUpdateSystems(new EcoSystem
                        {
                            Description = "Ola",
                            Url = "Hello"
                        },
                        "http://localhost:3000/comments"
                        , 
                        "POST");
                    string resp = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(resp); */  
                    
                    foreach (var ecosystem in userEcoSystems)
                    {
                        try
                        {
                            Console.WriteLine("Updating Systems");
                            Console.WriteLine(ecosystem.EcoSystem.Url);
                            
                            
                            var response =  await clientAdapter.DoUpdateSystems(ecosystem.AppUser,
                                ecosystem.EcoSystem.Url, "POST");
                            
                            string resp = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(resp);

                            //O erro deve ser enviado para o sistema de suporte?? sugestao
                            ecosystem.LastUpdateCommittedToTargetSystem = true;
                            await context.SaveChangesAsync(cancellationToken: stoppingToken);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    
                    await Task.Delay((int) timer, stoppingToken);
                }

            }
        }
    }
}