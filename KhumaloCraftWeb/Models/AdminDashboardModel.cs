using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KhumaloCraftWeb.Models
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel 
    {
        public void OnGet()
        {
            // Add any necessary logic here
        }
    }
}
