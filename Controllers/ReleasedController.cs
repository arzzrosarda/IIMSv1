using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.Shared;
using IIMSv1.Models.ViewModel;

namespace IIMSv1.Controllers
{
    public class ReleasedController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public ReleasedController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Filter 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Filter(string dept,string search, int page, string yearFrom, string yearTo, int entries)
        {
            return RedirectToAction("Index", "Released", new { page = page,search = search, dept = dept, yearFrom = yearFrom, yearTo = yearTo, entries = entries });
        }

        //Index
        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index(string dept = "", string search = "", int page = 1, string yearFrom = "", string yearTo = "", int entries = 10)
        {

            string deptId = dept.Trim().ToString();
            string searchTxt = search.Trim().ToString();
            string yearFromTxt = yearFrom.Trim().ToString();
            string yearToTxt = yearTo.Trim().ToString();

            DateOnly? YearStart = null;
            DateOnly? YearEnd = null;
            if (yearFrom != "" || yearTo != "")
            {
                var Year1 = yearFrom + "-01-01";

                var needYear = yearTo + "-12";
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
            

            IQueryable<Released> deptReleased = _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .Where(x => x.DepartmentId.Contains(deptId) && x.Department.IsActive.Equals(true));

            List<Released> released = new List<Released>();
            if (YearStart != null && YearEnd != null)
            {
                List<Released> releases = deptReleased
                    .Where(x => x.DateReleased >= YearStart
                    && x.DateReleased <= YearEnd)
                    .ToList();
                foreach (var rel in releases)
                {
                    released.Add(rel);
                }
            }
            else
            {
                List<Released> releases = deptReleased
                .ToList();
                foreach(var rel in releases)
                {
                    released.Add(rel);
                }

            }
            IQueryable<Items> item = _context.Items
                .OrderBy(x => x.ItemName)
                .Where(x => x.IsEnabled.Equals(true) && x.itemQuantity != null
                 && x.ItemName.Contains(searchTxt));

            List<_ItemReleasedModel> items1 = new List<_ItemReleasedModel>();
            int index = 0;
            foreach(var i in item)
            {
                index += 1;
                _ItemReleasedModel newitem = new _ItemReleasedModel()
                {
                    num = index,
                    Id = i.Id,
                    Name = i.ItemName,
                };
                items1.Add(newitem);
            }
            int CountTotal = 0;
            int CountToReceive = 0;
            int CountReceived = 0;
            int CountPullout = 0;
            List<int> years = new List<int>();
            foreach (var rel in released)
            {
                bool isToReceive = rel.Received.Equals(false) && rel.Pullout.Equals(false) && items1.Any(x => x.Id.Equals(rel.ItemId));
                if (isToReceive)
                {
                    CountToReceive += rel.ReleaseQuantity;
                }
                bool isReceived = rel.Received.Equals(true) && rel.Pullout.Equals(false) && items1.Any(x => x.Id.Equals(rel.ItemId));
                if (isReceived)
                {
                    CountReceived += rel.ReleaseQuantity;
                }
                bool isPullout = rel.Received.Equals(false) && rel.Pullout.Equals(true) && items1.Any(x => x.Id.Equals(rel.ItemId));
                if (isPullout)
                {
                    CountPullout += rel.ReleaseQuantity;
                }
                bool isTotal =  items1.Any(x => x.Id.Equals(rel.ItemId));
                if (isTotal)
                {
                    CountTotal += rel.ReleaseQuantity;
                }

                bool isYearExists = years.Any(x => x.Equals(rel.DateReleased.Year) && items1.Any(x => x.Id.Equals(rel.ItemId)));
                if (!isYearExists)
                {
                    years.Add(rel.DateReleased.Year);
                }
            }

            int recordsperpage = entries;
            int recordtotal = items1.Count();
            int pagetotal = Global.GetPaginationTotalPages(recordtotal, recordsperpage);

            if (page < 1 || (page > pagetotal && recordtotal > 0))
            {
                page = 1;
            }

            List<_ItemReleasedModel> items = items1
                .Skip(recordsperpage * (page - 1))
                .Take(recordsperpage)
                .ToList();

            List<Department> department = _context.Departments
                .Include(x => x.DepartmentCluster)
                .Where(x => x.IsActive.Equals(true))
                .ToList();

            Department? deptSel = await _context.Departments
                .SingleOrDefaultAsync(x => x.Id.Equals(deptId));

            string[] Month = ["JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"];

            ReleasedViewModel model = new ReleasedViewModel();
            model.PaginationModel = new PaginationModel("Index", "Inventory", "", recordtotal, recordsperpage, page);
            model.Months = Month.ToList();
            model.releasedItems = released;
            model.items = items;
            model.departments = department;
            model.DeptId = deptId;
            model.searchtxt = searchTxt;
            model.YearFrom = yearFrom;
            model.YearTo = yearTo;
            if(years.Count() > 0)
            {
                model.year = years.Min();   
                model.year1Text = years.Min().ToString();
                model.year2Text = years.Max().ToString();
            }
            model.perPage = entries.ToString();
            model.toReceiveCount = CountToReceive.ToString();
            model.receivedCount = CountReceived.ToString();
            model.pullOutCount = CountPullout.ToString();
            model.TotalCount = CountTotal.ToString();
            if (deptSel != null)
            {
                model.DeptTxt = deptSel.Name;
                model.deptText = deptSel.NormalizedName;
            }
            else
            {
                model.deptText = "";
            }
            return View(model);
        }
    }
}
