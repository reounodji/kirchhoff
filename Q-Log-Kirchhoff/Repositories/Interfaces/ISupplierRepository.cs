using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using MVC.Data.Entities;
using MVC.Migrations;
using MVC.Models.ConfigurationViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        /// <summary>
        /// Returns the supplier with the given name.
        /// Will return null if no such supplier is found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Supplier Get(string name);

        /// <summary>
        /// Returns the supplier with the given id.
        /// Will return null if no such supplier is found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Supplier Get(int id);

        /// <summary>
        /// Returns all Suppliernumbers from Supplier with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<SupplierNumber> GetAllSupplierNumbersFromSupplier(string name);

        /// <summary>
        /// Returns a list of all suppliers stored in the DB.
        /// </summary>
        /// <returns></returns>
        List<Supplier> GetAll();

        /// <summary>
        /// Returns all SupplierNumbers from the DB
        /// </summary>
        /// <returns></returns>
        List<SupplierNumber> GetAllSupplierNumber();

        /// <summary>
        /// Adds the supplier to the DB.
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>id</returns>
        Task Add(SupplierViewModel supplierViewModel);

        /// <summary>
        /// Applies the values of the passed along supplier to
        /// the corresponding DB entry.
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        Task EditSupplier(SupplierViewModel supplierViewModel);

        /// <summary>
        /// Removes the supplier with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Checks if all suppliers in the list have a unique name.
        /// Afterwards removes all current suppliers from 
        /// the DB and adds those from the list.
        /// </summary>
        /// <param name="agencies"></param>
        /// <returns></returns>
        Task Import(List<Supplier> suppliers, List<SupplierNumber> supplierNumbers);

        /// <summary>
        /// returns the ColorCode of the Supplier with the given Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        string GetColorCode(string Name);

        /// <summary>
        /// Updates the Suppliernumber with the given From API.
        /// Deletes not given 
        /// ignores known
        /// add new
        /// </summary>
        /// <param name="newSupNumbers"></param>
        /// <returns></returns>
        Task<bool> fullUpdateSupplier(List<SupplierNumber> newSupNumbers);
    }
}
