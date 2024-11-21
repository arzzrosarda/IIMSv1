using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IIMSv1.Controllers
{
    public class PrintReleasedController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public PrintReleasedController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index(string department = "", string relnum = "", string year1 = "", string year2 = "", string searchtxt = "")
        {   
            string[] arrMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
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
                .Include(x => x.Department)
                .Include(x => x.item)
               .Where(x => x.DepartmentId.Contains(department) && x.Department.IsActive.Equals(true));

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
            IQueryable<Items> QueryItem = _context.Items
                .Where(x => x.ItemName.Contains(searchtxt));

            int CountTotal = 0;
            int CountToReceive = 0;
            int CountReceived = 0;
            int CountPullout = 0;
            List<int> years = new List<int>();
            List<Department> depts = new List<Department>();
            foreach (var rel in rels)
            {
                bool isToReceive = rel.Received.Equals(false) && rel.Pullout.Equals(false) && QueryItem.Any(x => x.Id.Equals(rel.ItemId));
                if (isToReceive)
                {
                    CountToReceive += rel.ReleaseQuantity;
                }
                bool isReceived = rel.Received.Equals(true) && rel.Pullout.Equals(false) && QueryItem.Any(x => x.Id.Equals(rel.ItemId));
                if (isReceived)
                {
                    CountReceived += rel.ReleaseQuantity;
                }
                bool isPullout = rel.Received.Equals(false) && rel.Pullout.Equals(true) && QueryItem.Any(x => x.Id.Equals(rel.ItemId));
                if (isPullout)
                {
                    CountPullout += rel.ReleaseQuantity;
                }

                bool isYearExists = years.Any(x => x.Equals(rel.DateReleased.Year) && QueryItem.Any(x => x.Id.Equals(rel.ItemId)));
                if (!isYearExists)
                {
                    years.Add(rel.DateReleased.Year);
                }
                bool isTotal = QueryItem.Any(x => x.Id.Equals(rel.ItemId));
                if (isTotal)
                {
                    CountTotal += rel.ReleaseQuantity;
                }
                bool isDepartmentExists = depts.Any(x => x.Id.Equals(rel.DepartmentId));
                if (!isDepartmentExists)
                {
                    depts.Add(rel.Department);
                }
            }
            List<Items> items = QueryItem
                .OrderBy(x => x.ItemName)
                .Where(x => x.IsEnabled.Equals(true) && x.itemQuantity != null)
                .ToList();

            List<Department> departments = depts
                .OrderBy(x => x.NormalizedName)
                .Where(x => x.IsActive.Equals(true))
                .ToList();

            PrintReleasedViewModel model = new PrintReleasedViewModel();
            if(year1 != "" && year2 != "")
            {
                model.year1Text = year1;
                model.year2Text = year2;
            }
            else
            {
                model.year1Text = years.Min().ToString();
                model.year2Text = years.Max().ToString();

            }
            model.departments = departments;
            model.Released = rels;
            model.relnumText = relnum;
            model.Months = arrMonth.ToList();
            model.items = items;
            model.toReceiveCount = CountToReceive.ToString();
            model.receivedCount = CountReceived.ToString();
            model.pullOutCount = CountPullout.ToString();
            model.TotalCount = CountTotal.ToString();

            return View(model);
        }
    }
}
