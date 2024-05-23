using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;


namespace KhumaloCraftWeb.Services
{
    public interface IRoleInitializer
    {
        Task InitializeAsync();
    }

    public class RoleInitializer : IRoleInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleInitializer(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            // Create roles if they don't exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }
    }

}
