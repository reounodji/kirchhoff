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
    public class EFAuthorizationGroupsRepository : IAuthorizationGroupsRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<EFAuthorizationGroupsRepository> _logger;

        public EFAuthorizationGroupsRepository(ApplicationDBContext context, ILogger<EFAuthorizationGroupsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(AuthorizationGroup group)
        {
            if (group == null)
            {
                _logger.LogError("Error while trying to add authorization group. Param group == null");
                throw new ArgumentNullException("Gruppe konnte nicht hinzugefügt werden. Die angegebene gruppe existiert nicht.");
            }
            try
            {
                await _context.AuthorizationGroups.AddAsync(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to add Authorization group to db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim hinzufügen der Berechtigungsgruppe. Stellen Sie sicher, dass der Name noch nicht vergeben ist.");
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var entry = (from g in _context.AuthorizationGroups
                             where g.ID == id
                             select g).FirstOrDefault();
                if (entry == null)
                {
                    _logger.LogWarning("Did not remove AuthorizationGroup with id: " + id + " this group could not be found.");
                }
                else
                {
                    _context.AuthorizationGroups.Remove(entry);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while deleting AuthorizationGrozup with id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Löschen der Gruppe.");
            }
        }

        public async Task Edit(AuthorizationGroup group)
        {
            if (group == null)
            {
                _logger.LogError("Error while trying to edit authorization group. Param group == null");
                throw new ArgumentNullException("Gruppe konnte nicht bearbeitet werden. Die angegebene gruppe existiert nicht.");
            }

            try
            {

                var grp = (from g in _context.AuthorizationGroups
                           where g.ID == @group.ID
                           select g).FirstOrDefault();

                grp.Name = group.Name;
                grp.ADGroupName = group.ADGroupName;

                grp.CanUseProcessingList = group.CanUseProcessingList;
                grp.CanModifyProcessingList = group.CanModifyProcessingList;
                grp.CanSetLoadingStation = group.CanSetLoadingStation;
                grp.CanSetGate = group.CanSetGate;
                grp.CanSetRelease = group.CanSetRelease;
                grp.CanSetCall = group.CanSetCall;
                grp.CanSetEntrance = group.CanSetEntrance;
                grp.CanSetProcessStart = group.CanSetProcessStart;
                grp.CanSetProcessEnd = group.CanSetProcessEnd;
                grp.CanSetExit = group.CanSetExit;

                grp.CanUseHistory = group.CanUseHistory;
                grp.CanExportHistory = group.CanExportHistory;

                grp.CanUseConfig = group.CanUseConfig;
                grp.CanModifyAllSettings = group.CanModifyAllSettings;
                grp.CanModifyApproachTyps = group.CanModifyApproachTyps;
                grp.CanInspectApproachTyps = group.CanInspectApproachTyps;
                grp.CanModifyUnknownApproachTyps = group.CanModifyUnknownApproachTyps;
                grp.CanInspectUnknownApproachTyps = group.CanInspectUnknownApproachTyps;

                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to edit authorizationgroup. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Speichern der Gruppe. Stellen Sie sicher, dass der Name noch nicht vergeben ist.");
            }
        }

        public List<AuthorizationGroup> GetAll()
        {
            try
            {
                var all = (from a in _context.AuthorizationGroups
                           select a).ToList();
                return all;
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get all AuthorizationGroups. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                return new List<AuthorizationGroup>();
            }
        }

        public AuthorizationGroup Get(int id)
        {
            try
            {
                var group = (from g in _context.AuthorizationGroups
                             where g.ID == id
                             select g).FirstOrDefault();
                return group;
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get authorization group with id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim laden der Berechtigungsgruppe mit der id: " + id);
            }
        }

        public AuthorizationGroup Get(string name)
        {
            try
            {
                var group = (from g in _context.AuthorizationGroups
                             where g.Name == name
                             select g).FirstOrDefault();
                return group;
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get authorization group with name: " + name + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim laden der Berechtigungsgruppe mit der name: " + name);
            }
        }
    }
}
