using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IIMSv1.Data;
using IIMSv1.Models;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Models.ViewModel;
using System;
using System.Collections.Generic;

namespace IIMSv1.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public InventoryController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Get Released
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _GetReleased(string dept = "")
        {
            IQueryable<Released> rels = _context.Released
                .Include(x => x.item)
                .ThenInclude(x => x.itemType)
                .Where(x => x.Department.IsActive.Equals(true) && x.DepartmentId.Contains(dept));

            List<Released> released = new List<Released>();
            foreach (var rel in rels)
            {
                bool isDuplicate = released.Any(x => x.ReleaseNumber == rel.ReleaseNumber);
                if (!isDuplicate)
                {
                    released.Add(rel);
                }
            }
            List<Released> releases = released
                .OrderByDescending(x => x.ReleaseNumber)
                .Take(6)
                .ToList();

            return Json(releases);
        }

        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index()
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);

            AccountUser? thisUser = await _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .ThenInclude(x => x.DepartmentCluster)
                .SingleOrDefaultAsync(x => x.Id.Equals(currUser.Id));

            //Items
            List<Items> items = _context.Items
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .Include(x => x.itemUnit)
                .Where(x => x.IsEnabled.Equals(true))
                .ToList();

            //InStockItems
            List<Items> InStockItems = _context.Items
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .Include(x => x.itemUnit)
                .Where(x => x.IsEnabled.Equals(true) && x.itemQuantity > 0)
                .ToList();

            //OutofStockItems
            List<Items> OutofStockItems = _context.Items
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .Include(x => x.itemUnit)
                .Where(x => x.IsEnabled.Equals(true) && x.itemQuantity.Equals(0))
                .ToList();


            //Department 
            List<Department> departments = _context.Departments
                .Include(x => x.DepartmentCluster)
                .Where(x => x.IsActive.Equals(true))
                .ToList();


            List<Released> Releases = _context.Released
                .Include(x => x.item)
                .ThenInclude(x => x.itemType)
                .Include(x => x.Department)
                .Where(x => x.Department.IsActive.Equals(true))
                .ToList();

            List<Released> CountReleases = new List<Released>();
            int CountReleasesToReceive = 0;
            int CountReleasesReceive = 0;
            int CountReleasesPullout = 0;
            List<int> years = new List<int>();
            foreach (var rel in Releases)
            {
                bool isExists = CountReleases.Any(x => x.ReleaseNumber.Equals(rel.ReleaseNumber));

                if (!isExists)
                {
                    CountReleases.Add(rel);
                }
                bool isYearExists = years.Any(x => x.Equals(rel.DateReleased.Year));
                if (!isYearExists)
                {
                    years.Add(rel.DateReleased.Year);
                }
                if (rel.Received == false && rel.Pullout == false)
                {
                    CountReleasesToReceive += 1;
                }
                else if (rel.Received == true && rel.Pullout == false)
                {
                    CountReleasesReceive += 1;
                }
                else if (rel.Received == false && rel.Pullout == true)
                {
                    CountReleasesPullout += 1;
                }
            }
            InventoryViewModel model = new InventoryViewModel();
            if(years.Count() > 0)
            {
                model.year = years.Min();
            }
            else
            {
                model.year = DateTime.Now.Year;
            }
            model.departments = departments;
            model.items = items.Count();
            model.InStock = InStockItems.Count();
            model.OutofStock = OutofStockItems.Count();
            model.Releases = CountReleases.Count();
            model.ToReceive = CountReleasesToReceive;
            model.Received = CountReleasesReceive;
            model.Pullout = CountReleasesPullout;

            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
