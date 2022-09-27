using MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IBarrierControlSettingsRepository
    {
        BarrierControlSettings Get();

        void Update(BarrierControlSettings settings);
    }
}
