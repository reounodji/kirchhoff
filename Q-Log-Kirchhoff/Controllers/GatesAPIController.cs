using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Models.APIDtos;

namespace MVC.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GatesAPIController : ControllerBase
    {
        private readonly ILogger<GatesAPIController> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public GatesAPIController(ILogger<GatesAPIController> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpPost]
        [Route("gates/set-state")]
        public IActionResult SetState(GateStateDto dto)
        {
            _logger.LogInformation("GatesAPIController, SetState: setting state of gate: " + dto?.Name + "to isOccupied=" + dto?.IsOccupied);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var gatesFacade = scope.ServiceProvider.GetRequiredService<IGatesFacade>();
                    gatesFacade.SetState(dto);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch(NullReferenceException e)
            {
                _logger.LogError("Error while trying to set the state of the gate: " + dto.Name + ". The Gate couldnt be found. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set the state of the gate: " + dto.Name + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}