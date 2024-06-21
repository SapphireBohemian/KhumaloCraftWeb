using KhumaloCraftWeb.Data;
using KhumaloCraftWeb.Models;
using KhumaloCraftWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhumaloCraftWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly NotificationService _notificationService;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
    //       _notificationService = notificationService;
        }

        // Search action
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("SearchResults", new List<Products>());
            }

            var results = await _context.Product
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query) || p.Category.Contains(query))
                .ToListAsync();

            return View("SearchResults", results);
        }
    }
}
