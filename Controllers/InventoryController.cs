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

        //Get Count Released
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _GetCountReleased(string relnum)
        {
            List<Released> released = _context.Released
                .Where(x => x.ReleaseNumber.Equals(relnum))
                .ToList();

            List<int> counts = new List<int>();
            int toReceive = 0;
            int Received= 0;
            int Pullout = 0;
            foreach (var rel in released)
            {
                if(rel.Received.Equals(false) && rel.Pullout.Equals(false))
                {
                    toReceive += 1;
                }
                else if (rel.Received.Equals(true) && rel.Pullout.Equals(false))
                {
                    Received += 1;
                }
                else if (rel.Received.Equals(false) && rel.Pullout.Equals(true))
                {
                    Pullout += 1;
                }
            }
            counts.Add(toReceive);
            counts.Add(Received);
            counts.Add(Pullout);


            return Json(counts);
        }
        //Get Released
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _GetReleased(string dept = "")
        {
            IQueryable<Released> rels = _context.Released
                .Include(x => x.item)
                .Include(x => x.item)
                .OrderBy(x => x.ReleaseNumber)
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

            return Json(released);
        }

        //Get To Receive
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _GetReceivedQuantity(string Months, string year1 = "", string year2 = "", string dept = "")
        {
            var MonthsTxt = Months.Trim().ToString();
            string[] arrMonth = MonthsTxt.Split(',');
            DateOnly? YearStart = null;
            DateOnly? YearEnd = null;
            if (year1 != "" || year2 != "")
            {
                var Year1 = year1 + "-01-01";

                var needYear = year2 + "-12";
                DateTime year = Convert.ToDateTime(needYear);
                var ConvertYear = DateTime.DaysInMonth(year.Year, year.Month);
                var Year2 = needYear + "-" + ConvertYear;

                DateTime YearFrom = Convert.ToDateTime(Year1);
                DateOnly start = DateOnly.FromDateTime(YearFrom);
                YearStart = start;

                DateTime YearTo = Convert.ToDateTime(Year2);
                DateOnly end = DateOnly.FromDateTime(YearTo);
                YearEnd = end;
            }

            IQueryable<Released> released = _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .Where(x => x.DepartmentId.Contains(dept) && x.Department.IsActive.Equals(true));

            List<Released> rels = new List<Released>();
            if (YearStart != null && YearEnd != null)
            {
                List<Released> release = released
                    .Where(x => x.DateReleased >= YearStart && x.DateReleased <= YearEnd)
                    .ToList();

                foreach (var rel in release)
                {
                    rels.Add(rel);
                }
            }
            else
            {
                List<Released> release = released
                    .ToList();

                foreach (var rel in release)
                {
                    rels.Add(rel);
                }
            }
            List<string> TempMonthData = new List<string>();
            foreach (var m in arrMonth)
            {
                int Sum = 0;
                foreach (var rel in rels)
                {
                    bool isCountExists = rel.DateReleased.ToString("MMMM").ToUpper().Equals(m.ToUpper()) && rel.Received.Equals(true) && rel.Pullout.Equals(false);
                    if (isCountExists)
                    {
                        Sum += rel.ReleaseQuantity;
                    }
                    else
                    {
                        Sum += 0;

                    }
                }
                TempMonthData.Add(Sum.ToString());
            }

            return Json(TempMonthData);
        }
        //Get To Receive
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _GetPulloutQuantity(string Months, string year1 = "", string year2 = "", string dept = "")
        {
            var MonthsTxt = Months.Trim().ToString();
            string[] arrMonth = MonthsTxt.Split(',');

            DateOnly? YearStart = null;
            DateOnly? YearEnd = null;
            if (year1 != "" || year2 != "")
            {
                var Year1 = year1 + "-01-01";

                var needYear = year2 + "-12";
                DateTime year = Convert.ToDateTime(needYear);
                var ConvertYear = DateTime.DaysInMonth(year.Year, year.Month);
                var Year2 = needYear + "-" + ConvertYear;

                DateTime YearFrom = Convert.ToDateTime(Year1);
                DateOnly start = DateOnly.FromDateTime(YearFrom);
                YearStart = start;

                DateTime YearTo = Convert.ToDateTime(Year2);
                DateOnly end = DateOnly.FromDateTime(YearTo);
                YearEnd = end;
            }

            IQueryable<Released> released = _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .Where(x => x.DepartmentId.Contains(dept) && x.Department.IsActive.Equals(true));

            List<Released> rels = new List<Released>();
            if (YearStart != null && YearEnd != null)
            {
                List<Released> release = released
                    .Where(x => x.DateReleased >= YearStart && x.DateReleased <= YearEnd)
                    .ToList();

                foreach (var rel in release)
                {
                    rels.Add(rel);
                }
            }
            else
            {
                List<Released> release = released
                    .ToList();

                foreach (var rel in release)
                {
                    rels.Add(rel);
                }

            }
            List<string> TempMonthData = new List<string>();
            foreach (var m in arrMonth)
            {
                int Sum = 0;
                foreach (var rel in rels)
                {
                    bool isCountExists = rel.DateReleased.ToString("MMMM").ToUpper().Equals(m.ToUpper()) && rel.Received.Equals(false) && rel.Pullout.Equals(true);
                    if (isCountExists)
                    {
                        Sum += rel.ReleaseQuantity;
                    }
                    else
                    {
                        Sum += 0;

                    }
                }
                TempMonthData.Add(Sum.ToString());
            }

            return Json(TempMonthData);
        }
        //Get To Receive
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _GetToReceiveQuantity(string Months, string year1 = "", string year2 = "", string dept = "")
        {
            var MonthsTxt = Months.Trim().ToString();
            string[] arrMonth = MonthsTxt.Split(',');

            DateOnly? YearStart = null;
            DateOnly? YearEnd = null;
            if (year1 != "" || year2 != "")
            {
                var Year1 = year1 + "-01-01";

                var needYear = year2 + "-12";
                DateTime year = Convert.ToDateTime(needYear);
                var ConvertYear = DateTime.DaysInMonth(year.Year, year.Month);
                var Year2 = needYear + "-" + ConvertYear;

                DateTime YearFrom = Convert.ToDateTime(Year1);
                DateOnly start = DateOnly.FromDateTime(YearFrom);
                YearStart = start;

                DateTime YearTo = Convert.ToDateTime(Year2);
                DateOnly end = DateOnly.FromDateTime(YearTo);
                YearEnd = end;
            }
            IQueryable<Released> released = _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .Where(x => x.DepartmentId.Contains(dept) && x.Department.IsActive.Equals(true));

            List<Released> rels = new List<Released>();
            if (YearStart != null && YearEnd != null)
            {
                List<Released> release = released
                     .Where(x => x.DateReleased >= YearStart && x.DateReleased <= YearEnd)
                     .ToList();
                foreach (var rel in release)
                {
                    rels.Add(rel);
                }
            }
            else
            {
                List<Released> release = released
                       .ToList();
                foreach (var rel in release)
                {
                    rels.Add(rel);
                }
            }
            List<string> TempMonthData = new List<string>();
            foreach (var m in arrMonth)
            {

                int Sum = 0;
                foreach (var rel in rels)
                {
                    bool isCountExists = rel.DateReleased.ToString("MMMM").ToUpper().Equals(m.ToUpper()) && rel.Received.Equals(false) && rel.Pullout.Equals(false);
                    if (isCountExists)
                    {
                        Sum += rel.ReleaseQuantity;
                    }
                    else
                    {
                        Sum += 0;

                    }
                }
                TempMonthData.Add(Sum.ToString());

            }
            return Json(TempMonthData);
        }

        //Get Month Data
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _GetMonthData(string Months, string year1 = "", string year2 = "", string dept = "")
        {
            var MonthsTxt = Months.Trim().ToString();
            string[] arrMonth = MonthsTxt.Split(',');
            DateOnly? YearStart = null;
            DateOnly? YearEnd = null;
            if (year1 != "" || year2 != "")
            {
                var Year1 = year1 + "-01-01";

                var needYear = year2 + "-12";
                DateTime year = Convert.ToDateTime(needYear);
                var ConvertYear = DateTime.DaysInMonth(year.Year, year.Month);
                var Year2 = needYear + "-" + ConvertYear;

                DateTime YearFrom = Convert.ToDateTime(Year1);
                DateOnly start = DateOnly.FromDateTime(YearFrom);
                YearStart = start;

                DateTime YearTo = Convert.ToDateTime(Year2);
                DateOnly end = DateOnly.FromDateTime(YearTo);
                YearEnd = end;
            }
            IQueryable<Released> released = _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .Where(x => x.DepartmentId.Contains(dept) && x.Department.IsActive.Equals(true));

            List<Released> rels = new List<Released>();
            if (YearStart != null && YearEnd != null)
            {
                List<Released> release = released
                    .Where(x => x.DateReleased >= YearStart && x.DateReleased <= YearEnd)
                    .ToList();

                foreach (var rel in release)
                {
                    rels.Add(rel);
                }
            }
            else
            {
                List<Released> release = released
                      .ToList();

                foreach (var rel in release)
                {
                    rels.Add(rel);
                }
            }

            List<string> TempMonthData = new List<string>();
            foreach (var m in arrMonth)
            {

                int Sum = 0;
                foreach (var rel in rels)
                {
                    bool isCountExists = rel.DateReleased.ToString("MMMM").ToUpper().Equals(m.ToUpper());
                    if (isCountExists)
                    {
                        Sum += rel.ReleaseQuantity;
                    }
                    else
                    {
                        Sum += 0;

                    }
                }
                TempMonthData.Add(Sum.ToString());

            }
            return Json(TempMonthData);
        }

        //Get Records
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> GetRecords(string DeptId)
        {
            List<Released> CountReleases = new List<Released>();
            int CountReleasesToReceive = 0;
            int CountReleasesReceive = 0;
            int CountReleasesPullout = 0;
            if (DeptId == "All")
            {
                List<Released> Releases = _context.Released
                    .Include(x => x.item)
                    .Include(x => x.Department)
                    .Where(x => x.Department.IsActive == true)
                    .ToList();

                foreach (var rel in Releases)
                {
                    bool isExists = CountReleases.Any(x => x.ReleaseNumber.Equals(rel.ReleaseNumber));

                    if (!isExists)
                    {
                        CountReleases.Add(rel);
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
            }
            else
            {
                List<Released> Releases = _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .Where(x => x.DepartmentId.Contains(DeptId) && x.Department.IsActive == true)
                .ToList();


                foreach (var rel in Releases)
                {
                    bool isExists = CountReleases.Any(x => x.ReleaseNumber.Equals(rel.ReleaseNumber));

                    if (!isExists)
                    {
                        CountReleases.Add(rel);
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
            }

            string count = CountReleases.Count() + "," + CountReleasesToReceive.ToString() + "," + CountReleasesReceive.ToString() + "," + CountReleasesPullout;
            return Json(count);
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
