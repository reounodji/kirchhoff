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
    public class LicensePlateRecognitionAPIController : ControllerBase
    {
        private readonly ILogger<LicensePlateRecognitionAPIController> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public LicensePlateRecognitionAPIController(ILogger<LicensePlateRecognitionAPIController> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpPost]
        [Route("entry-barrier-test")]
        public IActionResult OpenEntryGate()
        {
            _logger.LogInformation("OPEN THE GATE! The one at the front i mean....");
            return Ok();
        }

        [HttpPost]
        [Route("license-recognition/detected-entry")]
        public async Task<IActionResult> DetectedEntryLicensePlate( LicensePlateRecognitionDto dto)
        {
            _logger.LogInformation("LicensePlateRecognitionAPIController, DetectedEntryLicensePlate: License has been detected at the front gate.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<ILicensePlateRecognitionFacade>();
                    await facade.DetectedEntry(dto);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception e)
            {
                 _logger.LogError("LicensePlateRecognitionAPIController, DetectedEntryLicesnePlate: Error! Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("license-recognition/detected-exit")]
        public async Task<IActionResult> DetectedExitLicensePlate(LicensePlateRecognitionDto dto)
        {
            _logger.LogInformation("LicensePlateRecognitionAPIController, DetectedExitLicensePlate: License has been detected at the exit gate.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<ILicensePlateRecognitionFacade>();
                    await facade.DetectedExit(dto);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("LicensePlateRecognitionAPIController, DetectedExitLicensePlate: Error! Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}