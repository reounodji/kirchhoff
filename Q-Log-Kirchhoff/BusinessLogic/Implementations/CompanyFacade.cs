using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Enums;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class CompanyFacade : ICompanyFacade
    {
        private readonly ILogger<ProcessingFacade> _logger;
        private readonly IServiceProvider _serviceProvider;


        public CompanyFacade(ILogger<ProcessingFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> ChangeCompany(int ID, string CompanyName)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                var regist = _openRegistrationsRepository.Get(ID);

                string ColorCode = string.Empty;

                switch (regist.ApproachTyp)
                {
                    case EApproachTyp.ForwardingAgency:
                        var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                        ColorCode = _forwardingAgenciesRepository.GetColorCode(CompanyName);
                        break;
                    case EApproachTyp.Supplier:
                        var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
                        ColorCode = _supplierRepository.GetColorCode(CompanyName);
                        break;
                    case EApproachTyp.ParcelService:
                        var _parcelServicesRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                        ColorCode = _parcelServicesRepository.GetColorCode(CompanyName);
                        break;
                    case EApproachTyp.Fitter:
                        var _fittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                        ColorCode = _fittersRepository.GetColorCode(CompanyName);
                        break;
                }

                await _openRegistrationsRepository.SetCompanyName(ID, CompanyName, ColorCode);

                return ColorCode;
            }
        }
    }
}
