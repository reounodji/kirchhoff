using MVC.Models.APIDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface ILicensePlateRecognitionFacade
    {
        Task DetectedEntry(LicensePlateRecognitionDto dto);

        Task DetectedExit(LicensePlateRecognitionDto dto);
    }
}
