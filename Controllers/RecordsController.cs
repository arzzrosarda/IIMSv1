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
    public class RecordsController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public RecordsController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Search Realase Number 
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _SearchRelNum(int relnum, string year)
        {
            return RedirectToAction("Index", "Records", new { relnum = relnum, year = year});
        }
        //Index
        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index(int relnum = 0, string year = "")
        {
            AccountUser? user = await _userManager.GetUserAsync(User);

            AccountUser? currUser = await _context.AccountCredentials
                .Include(x => x.Employee)
               .ThenInclude(x => x.department)
                .SingleOrDefaultAsync(x => x.Id.Equals(user.Id));

            string relnumTxt = "RLSD" + "-" + year + "-" + relnum.ToString("000000");

            List<Released> releases = _context.Released
                .Include(x => x.Department)
                .Include(x => x.item)
                .ThenInclude(x => x.itemType)
                .OrderBy(x => x.item.itemType.itemType)
                .Where(x => x.ReleaseNumber.Equals(relnumTxt) && x.Department.IsActive.Equals(true))
                .ToList();

            string department = "";
            string dateOnly = "";
            int index = 1;

            List<_RecordInputModel> record = new List<_RecordInputModel>();
            foreach (var release in releases)
            {
                bool isDepartment = department.Any(x => x.Equals(release.Department.NormalizedName));
                if (!isDepartment)
                {
                    department = release.Department.NormalizedName;
                }

                bool isDate = dateOnly.Any(x => x.Equals(release.DateReleased));
                if (!isDate)
                {
                    dateOnly = release.DateReleased.ToShortDateString();
                }

                List<ReleaseHistory> history =  _context.ReleaseHistory
                    .Where(x => x.ReleaseId == release.Id)
                    .ToList();
                string date = "";
                foreach(var h in history)
                {
                        bool isToReceive = h.Action.Equals("TO RECEIVE") && release.Received == false && release.Pullout == false;
                        if (isToReceive)
                        {
                            date = h.Date.ToShortDateString();
                        }
                        bool isReceived = h.Action.Equals("RECEIVED") && release.Received == true && release.Pullout == false;
                        if (isReceived)
                        {
                            date = h.Date.ToShortDateString();
                        }
                        bool isPullOut = h.Action.Equals("PULLOUT") && release.Received == false && release.Pullout == true;
                        if (isPullOut)
                        {
                            date = h.Date.ToShortDateString();
                        }
                }
                _RecordInputModel InRecord = new _RecordInputModel()
                {
                    num = index++,
                    Id = release.Id,
                    Item = release.item.ItemName,
                    Quantity = release.ReleaseQuantity,
                    Date = date,
                    IsReceived = release.Received,
                    IsPullout = release.Pullout,

                };
                record.Add(InRecord);
            }

            List<Released> released = _context.Released
               .Include(x => x.Department)
               .Include(x => x.item)
               .ThenInclude(x => x.itemType)
               .OrderBy(x => x.item.itemType.itemType)
               .ToList();

            List<string> listYear = new List<string>();
            foreach (var release in released)
            {
                string[] splitYear = release.ReleaseNumber.Split('-');
                bool isYear = listYear.Any(x => x.Equals(splitYear[1]));
                if (!isYear)
                {
                    listYear.Add(splitYear[1]);
                }
            }
            RecordsViewModel model = new RecordsViewModel();
            model.record = record;
            model.year = listYear;
            model.yeartxt = year;
            model.relnumtext = relnumTxt;
            model.relnum = relnum.ToString("000000");
            model.dateReleased = dateOnly;
            model.depttxt = department;

            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
