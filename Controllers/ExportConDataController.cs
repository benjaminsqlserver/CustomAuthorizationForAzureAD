using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using RadzenBlazorServerADDemo.Data;

namespace RadzenBlazorServerADDemo.Controllers
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;

        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/solutionroles/csv")]
        [HttpGet("/export/ConData/solutionroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSolutionRolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSolutionRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/solutionroles/excel")]
        [HttpGet("/export/ConData/solutionroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSolutionRolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSolutionRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/solutionusers/csv")]
        [HttpGet("/export/ConData/solutionusers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSolutionUsersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSolutionUsers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/solutionusers/excel")]
        [HttpGet("/export/ConData/solutionusers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSolutionUsersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSolutionUsers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/solutionusersinroles/csv")]
        [HttpGet("/export/ConData/solutionusersinroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSolutionUsersInRolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSolutionUsersInRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/solutionusersinroles/excel")]
        [HttpGet("/export/ConData/solutionusersinroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSolutionUsersInRolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSolutionUsersInRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/fetchrolesforadusers/csv(Username='{Username}')")]
        [HttpGet("/export/ConData/fetchrolesforadusers/csv(Username='{Username}', fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFetchRolesForAdUsersToCSV(string Username, string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFetchRolesForAdUsers(Username), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/fetchrolesforadusers/excel(Username='{Username}')")]
        [HttpGet("/export/ConData/fetchrolesforadusers/excel(Username='{Username}', fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFetchRolesForAdUsersToExcel(string Username, string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFetchRolesForAdUsers(Username), Request.Query), fileName);
        }
    }
}
