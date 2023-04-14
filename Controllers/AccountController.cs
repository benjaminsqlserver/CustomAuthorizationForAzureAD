using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using RadzenBlazorServerADDemo.Models;
using RadzenBlazorServerADDemo.Models.ConData;

namespace RadzenBlazorServerADDemo.Controllers
{
    [Route("Account/[action]")]
    public partial class AccountController : Controller
    {
        private readonly ConDataService _conDataService;//creation of private variable for ConData Service

        public AccountController(ConDataService conDataService)
        {
            _conDataService = conDataService;//injected ConDataService into AccountController
        }
        public IActionResult Login()
        {
            var redirectUrl = Url.Content("~/");

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Logout()
        {
            var redirectUrl = Url.Content("~/");

            return SignOut(new AuthenticationProperties { RedirectUri = redirectUrl }, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpPost]
        public async Task<ApplicationAuthenticationState> CurrentUser()
        {
            //call GetUserRolesAsync() method
            await GetUserRolesAsync();
            return new ApplicationAuthenticationState
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Claims = User.Claims.Select(c => new ApplicationClaim { Type = c.Type, Value = c.Value })
            };
        }

        private async Task GetUserRolesAsync()
        {
            //executing stored procedure to fetch user roles inside ConDataService
            //we are passing in the currently logged on user as input parameter to the stored procedure
            //User.Identity.Name is the actual value we are passing in to Username parameter
            IQueryable<Models.ConData.FetchRolesForAdUser> userRoles = await _conDataService.GetFetchRolesForAdUsers(User.Identity.Name);

            if (userRoles.Any())//check if at least one role is returned
            {

                foreach (FetchRolesForAdUser userRole in userRoles)
                {
                    //Create a new Identity for the user attached to the current HttpContext
                    HttpContext.User.AddIdentity(new ClaimsIdentity
                        (new[]
                     {
                        new Claim(ClaimTypes.Role,userRole.RoleName)
                    }, "RadzenBlazorServerADDemo"));


                }


                






            }
        }
    }
}