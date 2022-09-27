using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.DBContext
{
    /// <summary>
    /// Used to seed the DB with some necessary objects.
    /// </summary>
    public static class SeedData
    {
        public static void Initialize(ApplicationDBContext context)
        {
            if(!context.SMSSettings.Any())
            {
                context.SMSSettings.Add(new SMSSettings
                {
                    UseSMSService = false
                });
            }

            if (!context.BarrierControlSettings.Any())
            {
                context.BarrierControlSettings.Add(new BarrierControlSettings
                {
                    UseBarrierControl = false,
                    EntryBarrierAPIUrl = "",
                    ExitBarrierAPIUrl = ""
                });
            }

            if (!context.TerminalSettings.Any())
            {
                context.TerminalSettings.Add(new TerminalSettings {
                    TimePerLanguage = 30,
                    TimeTillReset = 90
                });
            }

            if (!context.GeneralSettings.Any())
            {
                context.GeneralSettings.Add(new Entities.GeneralSettings
                {
                    RegistrationTimeThreshold = 60,
                    DefaultHistoryTimespan = 14,
                    DisplayUpdateInterval = 10,
                    ExceededWaitTimeColorCode = "#ff0000",
                    RecentChangeColorCode = "#fcff5e",
                    ExitColorCode = "#26ff00",
                    HoverColorCode = "#affffb",
                    NewEntryColorCode = "#5e8eff",
                });
            }

            if (!context.ADSettings.Any())
            {
                context.ADSettings.Add(new Entities.ADSettings
                {
                    DomainNames = null,
                    DomainUserName = null,
                    DomainUserPassword = null,
                    ServerIP = null,
                    UseAD = false,
                    GenerateAccountsForNewADUsers = false
                });
            }

            if (!context.AuthorizationGroups.Any())
            {
                context.AuthorizationGroups.Add(new Entities.AuthorizationGroup
                {
                    Name = "Administatoren",
                    ADGroupName = "",

                    CanUseProcessingList = true,
                    CanModifyProcessingList = true,
                    CanSetLoadingStation = true,
                    CanSetGate = true,
                    CanSetRelease = true,
                    CanSetCall = true,
                    CanSetEntrance = true,
                    CanSetProcessStart = true,
                    CanSetProcessEnd = true,
                    CanSetExit = true,

                    CanUseHistory = true,
                    CanExportHistory = true,

                    CanUseConfig = true,
                    CanModifyAllSettings = true,
                    CanInspectApproachTyps = true,
                    CanModifyApproachTyps = true,
                    CanInspectUnknownApproachTyps = true,
                    CanModifyUnknownApproachTyps = true
                });
            }

            if (!context.LoadingStations.Any())
            {
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "Alle",
                    Description = "Listet alle Tore auf",
                    ShowAll = true
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "501",
                    Description = "Zubehör",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "Entsorgung",
                    Description = "Entsorgung",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "502",
                    Description = "Versand-LKW",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "503",
                    Description = "Versand-Abholer",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "504",
                    Description = "Fremdbezug",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "505",
                    Description = "Luftfracht",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "506",
                    Description = "Lehrwerkstatt",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "APL",
                    Description = "APL",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "Kesselhaus",
                    Description = "Kesselhaus",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "510",
                    Description = "APL/SP",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "511",
                    Description = "Korpus",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "512",
                    Description = "Front Einbauteil",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "513",
                    Description = "Container",
                    ShowAll = false
                });
                context.LoadingStations.Add(new LoadingStation
                {
                    Name = "514",
                    Description = "Pappe Instandhaltung",
                    ShowAll = false
                });
            }

            if (!context.Gates.Any())
            {
                context.Gates.Add(new Gate
                {
                    Name = "99",
                    Description = "Tor",
                    LoadingStation = "501",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "51",
                    Description = "Tor",
                    LoadingStation = "501",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "52",
                    Description = "Tor",
                    LoadingStation = "501",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "53",
                    Description = "Tor",
                    LoadingStation = "501",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "54",
                    Description = "Tor",
                    LoadingStation = "501",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "55",
                    Description = "Tor",
                    LoadingStation = "Entsorgung",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "56",
                    Description = "Tor",
                    LoadingStation = "Entsorgung",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "57",
                    Description = "Tor",
                    LoadingStation = "Entsorgung",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "58",
                    Description = "Tor",
                    LoadingStation = "Entsorgung",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "59",
                    Description = "Tor",
                    LoadingStation = "Entsorgung",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "60",
                    Description = "Tor",
                    LoadingStation = "Entsorgung",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "61",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "62",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "63",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "64",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "65",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "66",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "67",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "68",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "69",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "70",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "71",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "72",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "73",
                    Description = "Tor",
                    LoadingStation = "502",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "74",
                    Description = "Tor",
                    LoadingStation = "503",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "75",
                    Description = "Tor",
                    LoadingStation = "503",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "76",
                    Description = "Tor",
                    LoadingStation = "503",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "78",
                    Description = "Tor",
                    LoadingStation = "504",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "79",
                    Description = "Tor",
                    LoadingStation = "504",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "80",
                    Description = "Tor",
                    LoadingStation = "504",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "81",
                    Description = "Tor",
                    LoadingStation = "504",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "82",
                    Description = "Tor",
                    LoadingStation = "504",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "83",
                    Description = "Tor",
                    LoadingStation = "504",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "85",
                    Description = "Tor",
                    LoadingStation = "505",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "86",
                    Description = "Tor",
                    LoadingStation = "506",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "88",
                    Description = "Tor",
                    LoadingStation = "APL",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "89",
                    Description = "Tor",
                    LoadingStation = "Kesselhaus",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "90",
                    Description = "Tor",
                    LoadingStation = "510",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "91",
                    Description = "Tor",
                    LoadingStation = "511",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "92",
                    Description = "Tor",
                    LoadingStation = "512",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "93",
                    Description = "Tor",
                    LoadingStation = "513",
                    IsOccupied = false
                });
                context.Gates.Add(new Gate
                {
                    Name = "94",
                    Description = "Tor",
                    LoadingStation = "513",
                    IsOccupied = false
                });


                context.Gates.Add(new Gate
                {
                    Name = "96",
                    Description = "Tor",
                    LoadingStation = "514",
                    IsOccupied = false
                });
            }

            context.SaveChanges();
        }

        public static async Task CreateRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roles = new List<string> { "CanUseProcessingList", "CanModifyProcessingList", "CanSetLoadingStation", "CanSetGate", "CanSetRelease",
                "CanSetCall", "CanSetEntrance", "CanSetProcessStart", "CanSetProcessEnd", "CanSetExit", "CanUseHistory", "CanExportHistory", "CanUseConfig",
                "CanModifyAllSettings", "CanInspectApproachTyps", "CanModifyApproachTyps", "CanInspectUnknownApproachTyps", "CanModifyUnknownApproachTyps" };
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var admin = new AppUser
            {
                UserName = "admin",
                Email = "admin@schauf.eu",
                AuthorizationGroup = "Administatoren"
            };
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            if (await userManager.FindByNameAsync("admin") == null)
            {
                var result = await userManager.CreateAsync(admin, "Schauf!1");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, roles);
                }
            }
        }
    }
}
