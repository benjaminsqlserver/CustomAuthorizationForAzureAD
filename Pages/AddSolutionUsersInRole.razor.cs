using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace RadzenBlazorServerADDemo.Pages
{
    public partial class AddSolutionUsersInRole
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public ConDataService ConDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            solutionUsersInRole = new RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole();

            solutionRolesForRoleID = await ConDataService.GetSolutionRoles();

            solutionUsersForUserID = await ConDataService.GetSolutionUsers();
        }
        protected bool errorVisible;
        protected RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole solutionUsersInRole;

        protected IEnumerable<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> solutionRolesForRoleID;

        protected IEnumerable<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> solutionUsersForUserID;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.CreateSolutionUsersInRole(solutionUsersInRole);
                DialogService.Close(solutionUsersInRole);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;
    }
}