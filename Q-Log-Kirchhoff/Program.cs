using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.Data.DBContext;
using MVC.Repositories.Interfaces;
using NLog.Web;
using System;

namespace LKW3000
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                IWebHost host = null;
                try
                {
                    host = CreateWebHostBuilder(args).Build();
                }
                catch (Exception e)
                {
                    logger.Error("host = CreateWebHostBuilder error " + e.Message);
                    throw e;
                }
                if (host == null)
                {
                    logger.Error("Host is null. Stopping program.");
                    NLog.LogManager.Shutdown();
                    return;
                }
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    if (services == null)
                    {
                        logger.Error("Services is null. Stopping program.");
                        throw new NullReferenceException("Services null");
                    }
                    try
                    {
                        // Get the context
                        var context = services.GetRequiredService<ApplicationDBContext>();
                        if (context == null)
                        {
                            logger.Error("Context null");
                        }
                        try
                        {
                            context.Database.Migrate();
                        }
                        catch (Exception e)
                        {
                            logger.Error("Could not contact Database. Please make sure that the DB address is setup correctly in the appsettings.json. Stopping program. " + e.Message);
                            return;
                        }


                        logger.Info("Successfully called context.Database.Migrate");

                        SeedData.Initialize(context);

                    }
                    catch (Exception ex)
                    {
                        logger.Error("An error occurred seeding the DB. " + ex.InnerException.Message);
                        throw ex;
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception: " + ex.Message);
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()
                 .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                    
                 })
                .UseNLog();
    }
}
