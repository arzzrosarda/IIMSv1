using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IIMSv1.Data;
using IIMSv1.Models;

namespace IIMSv1.Controllers
{
    public class AdminController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public AdminController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> Index()
        {  
            return View();
        }
    }
}
