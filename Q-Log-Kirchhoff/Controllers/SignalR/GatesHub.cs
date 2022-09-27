using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers.SignalR
{
    /// <summary>
    /// Is used to update the overview page of the gates.
    /// </summary>
    public class GatesHub: Hub
    {
        private readonly ILogger<GatesHub> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public GatesHub(ILogger<GatesHub> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
    }
}
