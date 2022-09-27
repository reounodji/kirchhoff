using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MVC.BusinessLogic;
using MVC.BusinessLogic.Implementations;
using MVC.BusinessLogic.Interfaces;
using MVC.Controllers.SignalR;
using MVC.Data.DBContext;
using MVC.Data.Entities;
using MVC.HostedServices;
using MVC.Repositories.Implementations;
using MVC.Repositories.Interfaces;
using MVC.Validation;
using System;
using System.Globalization;
using System.Net.Http;

namespace LKW3000
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            // SignalR
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });


            services.AddDbContext<ApplicationDBContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("ApplicationDBContext")), ServiceLifetime.Transient);

            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = false;
                opts.User.AllowedUserNameCharacters += "/\\";
                opts.User.AllowedUserNameCharacters += "öäüÖÄÜ";
                opts.Password.RequiredLength = int.Parse( Configuration["Password_RequiredLength"] );
                opts.Password.RequireNonAlphanumeric = bool.Parse(Configuration["Password_RequireNonAlphanumeric"]);
                opts.Password.RequireLowercase = bool.Parse(Configuration["Password_RequireLowercase"]);
                opts.Password.RequireUppercase = bool.Parse(Configuration["Password_RequireUppercase"]);
                opts.Password.RequireDigit = bool.Parse(Configuration["Password_RequireDigit"]);

            }).AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
              //  .AddErrorDescriber<GermanIdentityErrorDescriber>();



            //Facades
            services.AddTransient<IRegistrationHubFacade, RegistrationHubFacade>();
            services.AddTransient<IRegistrationFacade, RegistrationFacade>();
            services.AddTransient<IConfigurationFacade, ConfigurationFacade>();
            services.AddTransient<IProcessingFacade, ProcessingFacade>();
            services.AddTransient<IDisplayFacade, DisplayFacade>();
            services.AddTransient<IProcessingHubFacade, ProcessingHubFacade>();
            services.AddTransient<IHistoryFacade, HistoryFacade>();
            services.AddTransient<IHistoryHubFacade, HistoryHubFacade>();
            services.AddTransient<IGatesFacade, GatesFacade>();
            services.AddTransient<ILicensePlateRecognitionFacade, LicensePlateRecognitionFacade>();
            services.AddTransient<ILocalizationFacade, LocalizationFacade>();
            services.AddTransient<IERPSender, ERPSender>();
            services.AddTransient<ICompanyFacade, CompanyFacade>();

            services.AddTransient<ICsvReader, CsvReader>();
            services.AddTransient<IExportFacade, ExportFacade>();
            services.AddTransient<IADReader, ADReader>();
            services.AddTransient<IAccountFacade, AccountFacade>();

            // Repositorys
            services.AddTransient<IGeneralSettingsRepository, EFGeneralSettingsRepository>();
            services.AddTransient<IGatesRepository, EFGatesRepository>();
            services.AddTransient<IForwardingAgenciesRepository, EFForwardingAgenciesRepository>();
            services.AddTransient<IOpenRegistrationsRepository, EFOpenRegistrationsRepository>();
            services.AddTransient<IClosedRegistrationsRepository, EFClosedRegistrationsRepository>();
            services.AddTransient<IDisplayConfigurationRepository, EFDisplayConfigurationRepository>();
            services.AddTransient<IAuthorizationGroupsRepository, EFAuthorizationGroupsRepository>();
            services.AddTransient<IADSettingsRepository, EFADSettingsRepository>();
            services.AddTransient<ISupplierRepository, EFSupplierRepository>();
            services.AddTransient<ITerminalSettingsRepository, EFTerminalSettingsRepository>();
            services.AddTransient<IUnknownForwardingAgenciesRepository, EFUnknownForwardingAgenciesRepository>();
            services.AddTransient<IUnknownSupplierRepository, EFUnknownSupplierRepository>();
            services.AddTransient<ISMSSettingsRepository, EFSMSSettingsRepository>();
            services.AddTransient<IBarrierControlSettingsRepository, EFBarrierControlSettingsRepository>();
            services.AddTransient<IFittersRepository, EFFittersRepository>();
            services.AddTransient<IParcelServicesRepository, EFParcelServicesRepository>();
            services.AddTransient<IUnknownFitterRepository, EFUnknownFitter>();
            services.AddTransient<IUnknownParcelServiceRepository, EFUnknownParcelServiceRepository>();
            services.AddTransient<ILoadingStationsRepository, EFLoadingStationsRepository>();

            services.AddScoped<ILocalizationRepository, LocalizationRepository>();

            services.AddHostedService<DisplayService>();
            services.AddHostedService<SupplierService>();

            services.AddTransient<HttpClientWrapper>();


            Utility.CompressChar = char.Parse(Configuration["CompressChar"]);
            var expireMins = int.Parse(Configuration["LoginExpireTimeSpan"]);
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "LKW3000Cookie";
                options.Cookie.HttpOnly = true;

                options.ExpireTimeSpan = TimeSpan.FromMinutes(expireMins);
                options.LoginPath = "/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddLocalization(o => o.ResourcesPath = "Resources");

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("de"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("fr"),
                    new CultureInfo("es"),
                    new CultureInfo("it"),
                    new CultureInfo("pt"),
                    new CultureInfo("pl"),
                    new CultureInfo("hr"),
                    new CultureInfo("tr"),
                    new CultureInfo("cs-CZ"),
                    new CultureInfo("sk"),
                    new CultureInfo("hu"),
                    new CultureInfo("ro"),
                    new CultureInfo("ru"),
                    new CultureInfo("lt"),
                    new CultureInfo("uk-UA")
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "de", uiCulture: "de");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Registration/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.UseMvcWithDefaultRoute();
            // HAS TO BE BEFORE USE MVC!!!!
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<RegistrationHub>("/RegistrationHub");
                routes.MapHub<ProcessingHub>("/ProcessingHub");
                routes.MapHub<GatesHub>("/GatesHub");
                routes.MapHub<HistoryHub>("/HistoryHub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Processing}/{action=Index}/{id?}");
            });
            SeedData.CreateRolesAndAdmin(app.ApplicationServices).Wait();
        }
    }
}
