using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Microsoft.Extensions.Logging;
using MVC.Data.DBContext;
using MVC.Data.Entities;
using MVC.Models.ConfigurationViewModels;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MVC.Repositories.Implementations
{
    public class EFSupplierRepository : ISupplierRepository
    {
        private readonly ILogger<EFForwardingAgenciesRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFSupplierRepository(ILogger<EFForwardingAgenciesRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(SupplierViewModel supplierViewModel)
        {
            if (supplierViewModel == null)
            {
                throw new ArgumentNullException("ViewModel ist null");
            }
            _logger.LogInformation("Adding Supplier.");
            try
            {
                _context.Suppliers.Add(supplierViewModel.Supplier);
                foreach (string number in supplierViewModel.Numbers.Split(","))
                {
                    if (!string.IsNullOrWhiteSpace(number))
                        _context.SupplierNumbers.Add(new SupplierNumber() { Number = number.Trim(), SupplierName = supplierViewModel.Supplier.Name });
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add supplier to DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Lieferant konnte nicht hinzugefügt werden. Stellen Sie sicher, dass der Name nicht bereits vergeben ist.");
            }
        }

        public Supplier Get(string name)
        {
            try
            {
                var agency = (from a in _context.Suppliers
                              where a.Name == name
                              select a).FirstOrDefault();
                return agency;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get Supplier. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }


        public Supplier Get(int id)
        {
            try
            {
                var agency = (from a in _context.Suppliers
                              where a.ID == id
                              select a).FirstOrDefault();
                return agency;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get Supplier. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public List<SupplierNumber> GetAllSupplierNumbersFromSupplier(string name)
        {
            try
            {
                var suppliernumbers = (from a in _context.SupplierNumbers
                                       where a.SupplierName.ToUpper() == name.ToUpper()
                                       select a).ToList();

                return suppliernumbers;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get Suppliernumbers. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public List<Supplier> GetAll()
        {
            try
            {
                return _context.Suppliers.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all Suppliers. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }


        public List<SupplierNumber> GetAllSupplierNumber()
        {
            try
            {
                return _context.SupplierNumbers.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all SupplierNumbers. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task EditSupplier(SupplierViewModel supplierViewModel)
        {
            if (supplierViewModel == null)
            {
                _logger.LogError("Could not edit supplier. param was null.");
                throw new ArgumentNullException("supplier is null");
            }
            try
            {
                var curSupplier = (from a in _context.Suppliers
                                   where a.ID == supplierViewModel.Supplier.ID
                                   select a).FirstOrDefault();
                if (curSupplier == null)
                {
                    _logger.LogWarning("Could not edit supplier. Supplier wasnt found in db. id: " + supplierViewModel.Supplier.ID);
                    return;
                }
                curSupplier.Name = supplierViewModel.Supplier.Name;
                curSupplier.ColorCode = supplierViewModel.Supplier.ColorCode;

                if (string.IsNullOrWhiteSpace(supplierViewModel.Numbers))
                {
                    return;
                }

                var curSupplierNumbers = _context.SupplierNumbers.Where(x => x.SupplierName == supplierViewModel.OldName).ToList();

                if (curSupplierNumbers == null)
                {
                    curSupplierNumbers = new List<SupplierNumber>();
                }

                List<SupplierNumber> newSupplierNubmers = new List<SupplierNumber>();

                foreach (string number in supplierViewModel.Numbers.Split(","))
                {
                    if (string.IsNullOrWhiteSpace(number))
                        continue;

                    newSupplierNubmers.Add(new SupplierNumber() { Number = number.Trim(), SupplierName = supplierViewModel.OldName });
                }

                foreach (SupplierNumber number in newSupplierNubmers)
                {
                    if (!curSupplierNumbers.Contains(curSupplierNumbers.Where(x => x.Number == number.Number).FirstOrDefault()))
                    {
                        curSupplierNumbers.Add(number);
                        _context.SupplierNumbers.Add(number);
                    }
                }

                for (int i = curSupplierNumbers.Count - 1; i >= 0; i--)
                {
                    if (!newSupplierNubmers.Contains(newSupplierNubmers.Where(x => x.Number == curSupplierNumbers[i].Number).FirstOrDefault()))
                        _context.SupplierNumbers.Remove(_context.SupplierNumbers.Where(x => x.ID == curSupplierNumbers[i].ID).FirstOrDefault());
                }

                if (supplierViewModel.OldName != supplierViewModel.Supplier.Name)
                {
                    foreach (SupplierNumber number in curSupplierNumbers)
                    {
                        number.SupplierName = supplierViewModel.Supplier.Name;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not edit supplier. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Lieferant konnte nicht bearbeitet werden. Stellen Sie sicher, dass der Name nicht bereits verwendet wird.");
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var sup = Get(id);
                if (sup == null)
                {
                    _logger.LogWarning("Did not remove supplier with id: " + id + " because it wasnt found in the db.");
                    return;
                }
                _context.SupplierNumbers.RemoveRange(_context.SupplierNumbers.Where(x => x.SupplierName == sup.Name));
                _context.Suppliers.Remove(sup);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete Supplier. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Import(List<Supplier> suppliers, List<SupplierNumber> supplierNumbers)
        {
            var oldSupplier = new List<Supplier>();
            var oldSupplierNumbers = new List<SupplierNumber>();

            var uniqueNames = CheckForUniqueNames(suppliers);
            if (!uniqueNames)
            {
                throw new Exception("Die Namen der Lieferanten müssen einzigartig sein!");
            }
            try
            {
                oldSupplier = (from f in _context.Suppliers
                               select f).ToList();
                oldSupplierNumbers = _context.SupplierNumbers.ToList();

                _context.Suppliers.RemoveRange(oldSupplier);
                _context.SupplierNumbers.RemoveRange(oldSupplierNumbers);

                await _context.Suppliers.AddRangeAsync(suppliers);
                await _context.SupplierNumbers.AddRangeAsync(supplierNumbers);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _context.Suppliers.AddRangeAsync(oldSupplier);
                    await _context.SupplierNumbers.AddRangeAsync(oldSupplierNumbers);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Could not add the old data back to db. Message: " + ex.Message + " inner: " + e.InnerException?.Message);
                }
                _logger.LogError("Error while trying to import suppliers into db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Speichern der Daten in der Datenbank.");
            }
        }

        private bool CheckForUniqueNames(List<Supplier> suppliers)
        {
            for (int i = 0; i < suppliers.Count; i++)
            {
                for (int j = i + 1; j < suppliers.Count; j++)
                {
                    if (suppliers[i].Name == suppliers[j].Name)
                        return false;
                }
            }

            return true;
        }

        public string GetColorCode(string Name)
        {
            try
            {
                var supplier = (from a in _context.Suppliers
                                where a.Name == Name
                                select a).FirstOrDefault();
                return supplier?.ColorCode ?? "";
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get supplier ColorCode with name: " + Name + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Lieferant konnte nicht geladen werden.");
            }
        }


        public async Task<bool> fullUpdateSupplier(List<SupplierNumber> apiSupNumbers)
        {
            List<string> apiSupplierNames = new List<string>();

            try
            {
                foreach (SupplierNumber sp in apiSupNumbers)
                {
                    if (!apiSupplierNames.Contains(sp.SupplierName))
                        apiSupplierNames.Add(sp.SupplierName);
                }

                var oldSupplier = GetAll();
                foreach (Supplier sup in oldSupplier)
                {
                    if (apiSupplierNames.FirstOrDefault(x => x == sup.Name) == null)
                    {
                        _context.SupplierNumbers.RemoveRange(_context.SupplierNumbers.Where(x => x.SupplierName == sup.Name));
                        _context.Suppliers.Remove(sup);
                    }
                }



                foreach (string apiName in apiSupplierNames)
                {
                    if (!oldSupplier.Contains(oldSupplier.Where(x => x.Name == apiName).FirstOrDefault()))
                    {
                        var newSup = new Supplier()
                        {
                            Name = apiName,
                            ColorCode = "#ffffff"
                        };

                        _context.Suppliers.Add(newSup);
                    }
                }


                var oldSupplierNumbers = GetAllSupplierNumber();
                foreach(SupplierNumber oldSup in oldSupplierNumbers)
                {
                    if (!apiSupNumbers.Contains(apiSupNumbers.Where(x => x.Number == oldSup.Number).FirstOrDefault()))
                    {
                        _context.SupplierNumbers.Remove(oldSup);
                    }
                }

                foreach (SupplierNumber apiNumber in apiSupNumbers)
                {
                    var number = oldSupplierNumbers.Where(x => x.Number == apiNumber.Number).FirstOrDefault();

                    if (number != null)
                    {
                        if (number.SupplierName != apiNumber.SupplierName)
                        {
                            _context.SupplierNumbers.Remove(number);
                            _context.SupplierNumbers.Add(apiNumber);
                        }
                    }
                    else
                    {
                        _context.SupplierNumbers.Add(apiNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception occured while get 'lieferanten'. Exception {0}. InnerException: {1}", ex.Message, ex.InnerException));
                ResetContextState();
                throw;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private void ResetContextState() => _context.ChangeTracker.Entries()
    .Where(e => e.Entity != null).ToList()
    .ForEach(e => e.State = EntityState.Detached);
    }
}
