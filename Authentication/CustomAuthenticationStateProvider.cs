using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Assigment_1.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Models;

namespace Assigment_1.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly IJSRuntime jsRuntime;
        private readonly IUser user;

        private Adult cachedUser;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUser user)
        {
            this.jsRuntime = jsRuntime;
            this.user = user;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (cachedUser == null)
            {
                string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    cachedUser = JsonSerializer.Deserialize<Adult>(userAsJson);

                    identity = SetupClaimsForUser(cachedUser);
                }
                else
                {
                    identity = SetupClaimsForUser(cachedUser);
                }
            }
            ClaimsPrincipal cathedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cathedClaimsPrincipal));
        }

        public void ValidateLogin(string username, string password)
        {
            Console.WriteLine("Validating log in");

            if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");
            
            ClaimsIdentity identity = new ClaimsIdentity();
            try
            {
                Adult adult = user.ValidateUser(username, password);
                identity = SetupClaimsForUser(adult);
                string serialisedData = JsonSerializer.Serialize(user);
                jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
                cachedUser = adult;
            }
            catch (Exception e)
            {
                throw e;
            }
            NotifyAuthenticationStateChanged(
                Task.FromResult<AuthenticationState>(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void Logout()
        {
            cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult<AuthenticationState>(new AuthenticationState(user)));
        }

        private ClaimsIdentity SetupClaimsForUser(Adult adult)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,adult.FirstName + adult.LastName));
            claims.Add(new Claim("Id", adult.Id.ToString()));
            claims.Add(new Claim("HairColor", adult.HairColor));
            claims.Add(new Claim("EyeColor", adult.EyeColor));
            claims.Add(new Claim("Age", adult.Age.ToString()));
            claims.Add(new Claim("Weight", adult.Weight.ToString()));
            claims.Add(new Claim("Height", adult.Height.ToString()));
            claims.Add(new Claim("Sex", adult.Sex));
            
            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }
    }
}