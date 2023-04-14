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
    public partial class EditSolutionUser
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
        public long UserID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            solutionUser = await ConDataService.GetSolutionUserByUserId(UserID);
        }
        protected bool errorVisible;
        protected RadzenBlazorServerADDemo.Models.ConData.SolutionUser solutionUser;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.UpdateSolutionUser(UserID, solutionUser);
                DialogService.Close(solutionUser);
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

            solutionUser = await ConDataService.GetSolutionUserByUserId(UserID);
        }
    }
}