using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Controllers.SignalR;
using MVC.Models;
using MVC.Models.APIDtos;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class GatesFacade : IGatesFacade
    {
        private readonly ILogger<GatesFacade> _logger;

        private readonly IServiceProvider _serviceProvider;


        public GatesFacade(ILogger<GatesFacade> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void SetState(GateStateDto dto)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var gatesRepo = scope.ServiceProvider.GetRequiredService<IGatesRepository>();

                var gate = gatesRepo.Get(dto.Name);
                if(gate == null)
                {
                    _logger.LogWarning("Could not setState for gate: " + dto.Name + ". No such gate could be found.");
                    throw new NullReferenceException();
                }

                gate.IsOccupied = dto.IsOccupied;
                gatesRepo.Update(gate);

                var gatesHub = scope.ServiceProvider.GetRequiredService<IHubContext<GatesHub>>();
                gatesHub.Clients.All.SendAsync("SetState", new { gate.ID, gate.IsOccupied});

                var processingHub = scope.ServiceProvider.GetRequiredService<IHubContext<ProcessingHub>>();
                processingHub.Clients.All.SendAsync("SetGateState", gate);
            }
        }
    }
}
