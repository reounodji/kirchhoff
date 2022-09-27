using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface ISMSSettingsRepository
    {
        SMSSettings Get();

        Task Set(SMSSettings settings);
    }
}
