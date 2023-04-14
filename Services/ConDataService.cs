using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using RadzenBlazorServerADDemo.Data;

namespace RadzenBlazorServerADDemo
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportSolutionRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/solutionroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/solutionroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSolutionRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/solutionroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/solutionroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSolutionRolesRead(ref IQueryable<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> items);

        public async Task<IQueryable<RadzenBlazorServerADDemo.Models.ConData.SolutionRole>> GetSolutionRoles(Query query = null)
        {
            var items = Context.SolutionRoles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSolutionRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSolutionRoleGet(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> GetSolutionRoleByRoleId(int roleid)
        {
            var items = Context.SolutionRoles
                              .AsNoTracking()
                              .Where(i => i.RoleID == roleid);

  
            var itemToReturn = items.FirstOrDefault();

            OnSolutionRoleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSolutionRoleCreated(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item);
        partial void OnAfterSolutionRoleCreated(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> CreateSolutionRole(RadzenBlazorServerADDemo.Models.ConData.SolutionRole solutionrole)
        {
            OnSolutionRoleCreated(solutionrole);

            var existingItem = Context.SolutionRoles
                              .Where(i => i.RoleID == solutionrole.RoleID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SolutionRoles.Add(solutionrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(solutionrole).State = EntityState.Detached;
                throw;
            }

            OnAfterSolutionRoleCreated(solutionrole);

            return solutionrole;
        }

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> CancelSolutionRoleChanges(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSolutionRoleUpdated(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item);
        partial void OnAfterSolutionRoleUpdated(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> UpdateSolutionRole(int roleid, RadzenBlazorServerADDemo.Models.ConData.SolutionRole solutionrole)
        {
            OnSolutionRoleUpdated(solutionrole);

            var itemToUpdate = Context.SolutionRoles
                              .Where(i => i.RoleID == solutionrole.RoleID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(solutionrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSolutionRoleUpdated(solutionrole);

            return solutionrole;
        }

        partial void OnSolutionRoleDeleted(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item);
        partial void OnAfterSolutionRoleDeleted(RadzenBlazorServerADDemo.Models.ConData.SolutionRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> DeleteSolutionRole(int roleid)
        {
            var itemToDelete = Context.SolutionRoles
                              .Where(i => i.RoleID == roleid)
                              .Include(i => i.SolutionUsersInRoles)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSolutionRoleDeleted(itemToDelete);


            Context.SolutionRoles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSolutionRoleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSolutionUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/solutionusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/solutionusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSolutionUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/solutionusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/solutionusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSolutionUsersRead(ref IQueryable<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> items);

        public async Task<IQueryable<RadzenBlazorServerADDemo.Models.ConData.SolutionUser>> GetSolutionUsers(Query query = null)
        {
            var items = Context.SolutionUsers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSolutionUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSolutionUserGet(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> GetSolutionUserByUserId(long userid)
        {
            var items = Context.SolutionUsers
                              .AsNoTracking()
                              .Where(i => i.UserID == userid);

  
            var itemToReturn = items.FirstOrDefault();

            OnSolutionUserGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSolutionUserCreated(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item);
        partial void OnAfterSolutionUserCreated(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> CreateSolutionUser(RadzenBlazorServerADDemo.Models.ConData.SolutionUser solutionuser)
        {
            OnSolutionUserCreated(solutionuser);

            var existingItem = Context.SolutionUsers
                              .Where(i => i.UserID == solutionuser.UserID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SolutionUsers.Add(solutionuser);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(solutionuser).State = EntityState.Detached;
                throw;
            }

            OnAfterSolutionUserCreated(solutionuser);

            return solutionuser;
        }

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> CancelSolutionUserChanges(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSolutionUserUpdated(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item);
        partial void OnAfterSolutionUserUpdated(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> UpdateSolutionUser(long userid, RadzenBlazorServerADDemo.Models.ConData.SolutionUser solutionuser)
        {
            OnSolutionUserUpdated(solutionuser);

            var itemToUpdate = Context.SolutionUsers
                              .Where(i => i.UserID == solutionuser.UserID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(solutionuser);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSolutionUserUpdated(solutionuser);

            return solutionuser;
        }

        partial void OnSolutionUserDeleted(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item);
        partial void OnAfterSolutionUserDeleted(RadzenBlazorServerADDemo.Models.ConData.SolutionUser item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> DeleteSolutionUser(long userid)
        {
            var itemToDelete = Context.SolutionUsers
                              .Where(i => i.UserID == userid)
                              .Include(i => i.SolutionUsersInRoles)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSolutionUserDeleted(itemToDelete);


            Context.SolutionUsers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSolutionUserDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSolutionUsersInRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/solutionusersinroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/solutionusersinroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSolutionUsersInRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/solutionusersinroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/solutionusersinroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSolutionUsersInRolesRead(ref IQueryable<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> items);

        public async Task<IQueryable<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole>> GetSolutionUsersInRoles(Query query = null)
        {
            var items = Context.SolutionUsersInRoles.AsQueryable();

            items = items.Include(i => i.SolutionRole);
            items = items.Include(i => i.SolutionUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSolutionUsersInRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSolutionUsersInRoleGet(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> GetSolutionUsersInRoleById(long id)
        {
            var items = Context.SolutionUsersInRoles
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.SolutionRole);
            items = items.Include(i => i.SolutionUser);
  
            var itemToReturn = items.FirstOrDefault();

            OnSolutionUsersInRoleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSolutionUsersInRoleCreated(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item);
        partial void OnAfterSolutionUsersInRoleCreated(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> CreateSolutionUsersInRole(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole solutionusersinrole)
        {
            OnSolutionUsersInRoleCreated(solutionusersinrole);

            var existingItem = Context.SolutionUsersInRoles
                              .Where(i => i.ID == solutionusersinrole.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SolutionUsersInRoles.Add(solutionusersinrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(solutionusersinrole).State = EntityState.Detached;
                throw;
            }

            OnAfterSolutionUsersInRoleCreated(solutionusersinrole);

            return solutionusersinrole;
        }

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> CancelSolutionUsersInRoleChanges(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSolutionUsersInRoleUpdated(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item);
        partial void OnAfterSolutionUsersInRoleUpdated(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> UpdateSolutionUsersInRole(long id, RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole solutionusersinrole)
        {
            OnSolutionUsersInRoleUpdated(solutionusersinrole);

            var itemToUpdate = Context.SolutionUsersInRoles
                              .Where(i => i.ID == solutionusersinrole.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(solutionusersinrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSolutionUsersInRoleUpdated(solutionusersinrole);

            return solutionusersinrole;
        }

        partial void OnSolutionUsersInRoleDeleted(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item);
        partial void OnAfterSolutionUsersInRoleDeleted(RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole item);

        public async Task<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> DeleteSolutionUsersInRole(long id)
        {
            var itemToDelete = Context.SolutionUsersInRoles
                              .Where(i => i.ID == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSolutionUsersInRoleDeleted(itemToDelete);


            Context.SolutionUsersInRoles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSolutionUsersInRoleDeleted(itemToDelete);

            return itemToDelete;
        }
    
      public async Task ExportFetchRolesForAdUsersToExcel(string Username, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/fetchrolesforadusers/excel(Username='{Username}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/fetchrolesforadusers/excel(Username='{Username}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task ExportFetchRolesForAdUsersToCSV(string Username, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/fetchrolesforadusers/csv(Username='{Username}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/fetchrolesforadusers/csv(Username='{Username}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task<IQueryable<RadzenBlazorServerADDemo.Models.ConData.FetchRolesForAdUser>> GetFetchRolesForAdUsers(string Username, Query query = null)
      {
          OnFetchRolesForAdUsersDefaultParams(ref Username);

          var items = Context.FetchRolesForAdUsers.FromSqlInterpolated($"EXEC [dbo].[FetchRolesForADUser] {Username}").ToList().AsQueryable();

          if (query != null)
          {
              if (!string.IsNullOrEmpty(query.Expand))
              {
                  var propertiesToExpand = query.Expand.Split(',');
                  foreach(var p in propertiesToExpand)
                  {
                      items = items.Include(p.Trim());
                  }
              }

              if (!string.IsNullOrEmpty(query.Filter))
              {
                  if (query.FilterParameters != null)
                  {
                      items = items.Where(query.Filter, query.FilterParameters);
                  }
                  else
                  {
                      items = items.Where(query.Filter);
                  }
              }

              if (!string.IsNullOrEmpty(query.OrderBy))
              {
                  items = items.OrderBy(query.OrderBy);
              }

              if (query.Skip.HasValue)
              {
                  items = items.Skip(query.Skip.Value);
              }

              if (query.Top.HasValue)
              {
                  items = items.Take(query.Top.Value);
              }
          }
          
          OnFetchRolesForAdUsersInvoke(ref items);

          return await Task.FromResult(items);
      }

      partial void OnFetchRolesForAdUsersDefaultParams(ref string Username);

      partial void OnFetchRolesForAdUsersInvoke(ref IQueryable<RadzenBlazorServerADDemo.Models.ConData.FetchRolesForAdUser> items);  
    }
}