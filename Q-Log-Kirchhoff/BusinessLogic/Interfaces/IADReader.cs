using System.Collections.Generic;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IADReader
    {
        /// <summary>
        /// Creates a list of all groupnames that the
        /// user with the given name is part of.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<string> GetADGroups(string name);

        bool Validate(string username, string password);
    }
}
