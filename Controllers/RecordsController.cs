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
        //_CancelRelease
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _CancelRelease(string RelId)
        {
            AccountUser? user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
            }
            AccountUser? currUser = await _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .SingleOrDefaultAsync(x => x.Id.Equals(user.Id));

            Released? released = await _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .SingleOrDefaultAsync(x => x.Id.Equals(RelId));

            if (released == null)
            {
                return NotFound();
            }
            else
            {
                var date = DateOnly.FromDateTime(DateTime.Now);
                var time = TimeOnly.FromDateTime(DateTime.Now);

                ReleaseHistory newRelHistory = new ReleaseHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = currUser.Id,
                    Date = date,
                    Time = time,
                    ReleaseId = released.Id,
                    OldQuantity = released.ReleaseQuantity,
                    NewQuantity = 0,
                    Action = "PULLOUT"
                };
                var ReleaseQuantity = released.ReleaseQuantity;
                var PULLOUTQuantity = released.item.itemQuantity + released.ReleaseQuantity;
                ItemQuantityHistory newQuanHistory = new ItemQuantityHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    QuantityFrom = (int)released.item.itemQuantity,
                    QuantityTo = (int)PULLOUTQuantity,
                    Activity = "PULLOUT: return stock",
                    Date = date,
                    Time = time,
                    ItemId = released.item.Id,
                    UserId = currUser.Id,
                };
                released.ReleaseQuantity = 0;
                released.item.itemQuantity = PULLOUTQuantity;
                released.Pullout = true;
                await _context.SaveChangesAsync(currUser.Id, "PULLOUT Release: return stocks");
                _context.AddAsync(newRelHistory);
                _context.AddAsync(newQuanHistory);
                _context.SaveChanges();
                TempData["alert"] = Global.GenerateToast("PULLOUT", "<b>ITEM:</b>"
                    + released.item.ItemName 
                    + " <br /> <b>FOR:</b> "
                    + released.Department.NormalizedName 
                    + " <br /> <b>PULLOUT QUANTITY:</b> "
                    + ReleaseQuantity.ToString(), "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
               
                return Json("");
            }
        }
        //Search Realase Number 
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _SearchRelNum(int relnum, string year, int page, int entries, string toreceive, string pullout, string received)
        {
            return RedirectToAction("Index", "Records", new { relnum = relnum, year = year, page = page, entries = entries, toreceive = toreceive, pullout = pullout, received = received });
        }
        //Index
        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index(int relnum = 0, string year = "", int page = 1, int entries = 10, string toreceive = "", string pullout = "", string received = "" /*, string dept = ""*/)
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
                .OrderBy(x => x.Received && x.Pullout)
                .ThenBy(x => x.item.ItemName)
                .Where(x => x.ReleaseNumber.Equals(relnumTxt) && x.Department.IsActive.Equals(true))
                .ToList();

            string department = "";
            string dateOnly = "";
            int TotalQuantity = 0;
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
                TotalQuantity += release.ReleaseQuantity;
                record.Add(InRecord);
            }

            List<_RecordInputModel> FilteredeRecords = new List<_RecordInputModel>();
            if (toreceive == "true")
            {
                foreach (var rec in record)
                {
                    bool IsToReceive = rec.IsReceived.Equals(false) && rec.IsPullout.Equals(false);

                    if (IsToReceive)
                    {
                        FilteredeRecords.Add(rec);
                    }
                }
            }
            else if (pullout == "true")
            {
                foreach (var rec in record)
                {
                    bool IsPullout = rec.IsReceived.Equals(false) && rec.IsPullout.Equals(true);

                    if (IsPullout)
                    {
                        FilteredeRecords.Add(rec);
                    }
                }
            }
            else if (received == "true")
            {
                foreach (var rec in record)
                {
                    bool IsReceived = rec.IsReceived.Equals(true) && rec.IsPullout.Equals(false);

                    if (IsReceived)
                    {
                        FilteredeRecords.Add(rec);
                    }
                }
            }
            else
            {
                foreach (var rec in record)
                {
                    FilteredeRecords.Add(rec);
                }
            }

            List<Released> released = _context.Released
               .Include(x => x.Department)
               .Include(x => x.item)
               .OrderBy(x => x.item.ItemName)
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
            int recordsperpage = entries;
            int recordtotal = FilteredeRecords.Count();
            int pagetotal = Global.GetPaginationTotalPages(recordtotal, recordsperpage);

            if (page < 1 || (page > pagetotal && recordtotal > 0))
            {
                page = 1;
            }
            List<_RecordInputModel> recordfilteredReleases = FilteredeRecords
                .Skip(recordsperpage * (page - 1))
                .Take(recordsperpage)
                .ToList();

            RecordsViewModel model = new RecordsViewModel();
            model.PaginationModel = new PaginationModel("Index", "Records", "", recordtotal, recordsperpage, page);
            model.Received = received;
            model.ToReceive = toreceive;
            model.record = record;
            model.Pullout = pullout;
            model.year = listYear;
            model.yeartxt = year;
            model.perPage = entries.ToString();
            model.relnumtext = relnumTxt;
            model.relnum = relnum.ToString("000000");
            model.releases = recordfilteredReleases;
            model.dateReleased = dateOnly;
            model.depttxt = department;
            model.totalQuantity = TotalQuantity;

            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
