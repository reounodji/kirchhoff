using Microsoft.Extensions.Logging;
using MVC.Data.DBContext;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories.Implementations
{
    public class EFBarrierControlSettingsRepository : IBarrierControlSettingsRepository
    {
        private readonly ILogger<EFBarrierControlSettingsRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFBarrierControlSettingsRepository(ILogger<EFBarrierControlSettingsRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public BarrierControlSettings Get()
        {
            return _context.Set<BarrierControlSettings>().FirstOrDefault();
        }

        public void Update(BarrierControlSettings settings)
        {
            _context.Set<BarrierControlSettings>().Update(settings);
            _context.SaveChanges();
        }
    }
}
