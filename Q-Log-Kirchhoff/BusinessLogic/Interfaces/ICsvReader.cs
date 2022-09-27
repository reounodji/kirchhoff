using Microsoft.AspNetCore.Http;

namespace MVC.BusinessLogic.Interfaces
{
    public interface ICsvReader
    {
        bool IsValid { get; }

        string[] Keys { get; }

        string[][] Values { get; }

        /// <summary>
        /// Reads the file and sets the Keys, Values and isValid flags
        /// </summary>
        /// <param name="file"></param>
        void Read(IFormFile file);

        /// <summary>
        /// Checks if the given keys are contained in the list of Keys found in the file.
        /// Must be called after 'Read(file)' has been performed.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        bool ContainsKeys(string[] keys);
    }
}
