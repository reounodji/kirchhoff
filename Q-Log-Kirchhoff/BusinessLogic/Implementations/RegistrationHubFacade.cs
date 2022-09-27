using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class RegistrationHubFacade : IRegistrationHubFacade
    {
        private readonly ILogger<RegistrationHubFacade> _logger;
        private readonly IServiceProvider _serviceProvider;


        public RegistrationHubFacade(ILogger<RegistrationHubFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public List<ForwardingAgency> GetForwardingAgencies(string input)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();

                var all = _forwardingAgenciesRepository.GetAll().Where(x => x.Name.ToUpper().Contains(input)).ToList();

                return all;
            }
        }

        public List<string> GetSuppliers(string input)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();

                var all = _supplierRepository.GetAll().Where(x => x.Name.ToUpper().Contains(input)).ToList();

                List<string> supplierNamesWithNumbers = new List<string>();

                foreach (Supplier supplier in all)
                {
                    foreach(SupplierNumber supplierNumber in _supplierRepository.GetAllSupplierNumbersFromSupplier(supplier.Name))
                    {
                        supplierNamesWithNumbers.Add(string.Format("{0} \u2192 {1}", supplier.Name, supplierNumber.Number));
                    }
                }

                return supplierNamesWithNumbers;
            }
        }

        public List<ParcelService> GetParcelServices(string input)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _ParcelServiceRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();

                var all = _ParcelServiceRepository.GetAll().Where(x => x.Name.ToUpper().Contains(input)).ToList();

                return all;
            }
        }

        public List<Fitter> GetFitters(string input)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _FitterRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();

                var all = _FitterRepository.GetAll().Where(x => x.Name.ToUpper().Contains(input)).ToList();

                return all;
            }
        }
    }
}
