using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IAuthorizationGroupsRepository
    {
        /// <summary>
        /// Returns all AuthorizationGroups from the DB.
        /// </summary>
        /// <returns></returns>
        List<AuthorizationGroup> GetAll();

        /// <summary>
        /// Adds the AuthorizationGroup to the DB.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Add(AuthorizationGroup group);

        /// <summary>
        /// Removes the AuthorizationGroup with the given id
        /// from the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Applies the values to the corresponding DB entry.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Edit(AuthorizationGroup group);

        /// <summary>
        /// Returns the Authorizationgroup with the given id.
        /// Will return null if none is found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AuthorizationGroup Get(int id);

        /// <summary>
        /// Returns the AuthorizationGroup with the given name.
        /// Will return null if none is found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AuthorizationGroup Get(string name);
    }
}
