using KhumaloCraftWeb.Data;
using KhumaloCraftWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KhumaloCraftWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory; // Inject HttpClientFactory

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
        }

        // Action to display order form
        [Authorize]
        public async Task<IActionResult> PlaceOrder()
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the user's shopping cart
            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.ShoppingCartItems)
                .ThenInclude(sci => sci.Product)
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id);

            return View(shoppingCart);
        }

        // Action to process order
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(int shoppingCartId)
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the user's shopping cart
            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.ShoppingCartItems)
                .ThenInclude(sci => sci.Product)
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id && sc.ShoppingCartId == shoppingCartId);

            if (shoppingCart == null)
            {
                return NotFound(); // Return 404 Not Found if shopping cart is not found
            }

            // Check if all products in the shopping cart exist
            var invalidProducts = shoppingCart.ShoppingCartItems.Where(item =>
                item.Product == null || item.Product.Availability == false).ToList();

            if (invalidProducts.Any())
            {
                // Remove invalid products from the shopping cart
                _context.ShoppingCartItem.RemoveRange(invalidProducts);
                await _context.SaveChangesAsync();

                // Return an error message indicating invalid products
                ModelState.AddModelError(string.Empty, "Some products in your cart are no longer available. Please review your order.");

                // Reload the shopping cart
                shoppingCart = await _context.ShoppingCart
                    .Include(sc => sc.ShoppingCartItems)
                    .ThenInclude(sci => sci.Product)
                    .FirstOrDefaultAsync(sc => sc.UserId == user.Id && sc.ShoppingCartId == shoppingCartId);

                return View("PlaceOrder", shoppingCart);
            }

            // Calculate total amount
            decimal totalAmount = shoppingCart.ShoppingCartItems.Sum(item => item.Quantity * item.Product.Price);

            // Create order
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderItems = shoppingCart.ShoppingCartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                }).ToList()
            };

            // Add order to the database
            _context.Order.Add(order);

            // Remove the items from the shopping cart
            _context.ShoppingCartItem.RemoveRange(shoppingCart.ShoppingCartItems);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Trigger Durable Function upon successful order placement
            await TriggerDurableFunction(order.OrderId);

            return RedirectToAction("ViewOrders");
        }

        // Method to trigger Durable Function
        private async Task TriggerDurableFunction(int orderId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://orderprocessingfunction.azurewebsites.net")
                {
                    Content = new StringContent($"{{ 'orderId': {orderId} }}")
                };

                var client = _httpClientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle failure scenario
                    // Log or throw an exception
                    throw new Exception($"Failed to trigger Durable Function. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error triggering Durable Function: {ex.Message}");
                throw;
            }
        }

        // Action to view details of a specific order
        [Authorize]
        public async Task<IActionResult> OrderDetails(int id)
        {
            // Get the order with the specified id
            var order = await _context.Order
                .Include(o => o.OrderItems) // Include related order items
                .ThenInclude(oi => oi.Product) // Include related products
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound(); // Return 404 Not Found if order is not found
            }

            return View(order);
        }

        // Action to view previous orders
        [Authorize]
        public async Task<IActionResult> ViewOrders()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve orders for the current user
            var orders = await _context.Order
                .Include(o => o.OrderItems) // Include related order items
                .ThenInclude(oi => oi.Product) // Include related products
                .Where(o => o.UserId == user.Id)
                .ToListAsync();

            return View(orders);
        }
    }
}
