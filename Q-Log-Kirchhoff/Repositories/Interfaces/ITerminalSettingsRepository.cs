using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface ITerminalSettingsRepository
    {
        TerminalSettings Get();

        Task Set(TerminalSettings settings);
    }
}
