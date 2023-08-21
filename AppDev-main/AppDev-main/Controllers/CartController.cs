using AppDev.Data;
using AppDev.Models;
using AppDev.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDev.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        private string? _customerId;

        private string CustomerId
        {
            get
            {
                _customerId ??= userManager.GetUserId(User);
                return _customerId;
            }
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var items = await context.CartItems
                .Include(ci => ci.Book)
                .Where(ci => ci.CustomerId == CustomerId)
                .ToListAsync();
            return View(items);
        }
