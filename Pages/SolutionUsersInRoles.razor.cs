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
    public partial class SolutionUsersInRoles
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

        protected IEnumerable<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> solutionUsersInRoles;

        protected RadzenDataGrid<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            solutionUsersInRoles = await ConDataService.GetSolutionUsersInRoles(new Query { Expand = "SolutionRole,SolutionUser" });
        }
        protected override async Task OnInitializedAsync()
        {
            solutionUsersInRoles = await ConDataService.GetSolutionUsersInRoles(new Query { Expand = "SolutionRole,SolutionUser" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddSolutionUsersInRole>("Add SolutionUsersInRole", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> args)
        {
            await DialogService.OpenAsync<EditSolutionUsersInRole>("Edit SolutionUsersInRole", new Dictionary<string, object> { {"ID", args.Data.ID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole solutionUsersInRole)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteSolutionUsersInRole(solutionUsersInRole.ID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete SolutionUsersInRole" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConDataService.ExportSolutionUsersInRolesToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "SolutionRole,SolutionUser", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "SolutionUsersInRoles");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ConDataService.ExportSolutionUsersInRolesToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "SolutionRole,SolutionUser", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "SolutionUsersInRoles");
            }
        }
    }
}