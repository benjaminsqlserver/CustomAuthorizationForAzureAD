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
    public partial class EditSolutionRole
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

        [Parameter]
        public int RoleID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            solutionRole = await ConDataService.GetSolutionRoleByRoleId(RoleID);
        }
        protected bool errorVisible;
        protected RadzenBlazorServerADDemo.Models.ConData.SolutionRole solutionRole;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.UpdateSolutionRole(RoleID, solutionRole);
                DialogService.Close(solutionRole);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           ConDataService.Reset();
            hasChanges = false;
            canEdit = true;

            solutionRole = await ConDataService.GetSolutionRoleByRoleId(RoleID);
        }
    }
}