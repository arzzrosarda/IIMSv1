using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.Shared;
using IIMSv1.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IIMSv1.Controllers
{
    public class PrintRecordsController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public PrintRecordsController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Print Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index(string relnum = "", bool toReceive = false, bool received = false, bool pullout = false)
        {
            List<Released> releases = _context.Released
                .Include(x => x.item)
                .Include(x => x.Department)
                .Where(x => x.ReleaseNumber.Equals(relnum) && x.Department.IsActive.Equals(true))
                .ToList();

            if(releases.Count() == 0)
            {
                return NotFound();
            }

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

                List<ReleaseHistory> history = _context.ReleaseHistory
                    .Where(x => x.ReleaseId == release.Id)
                    .ToList();
                string date = "";
                foreach (var h in history)
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
            List<_RecordInputModel> ToReceiveRecords = new List<_RecordInputModel>();
            List<_RecordInputModel> ReceivedRecords = new List<_RecordInputModel>();
            List<_RecordInputModel> PullOutRecords = new List<_RecordInputModel>();
            foreach (var rec in record)
            {
                bool IsToReceive = rec.IsReceived.Equals(false) && rec.IsPullout.Equals(false);

                if (IsToReceive)
                {
                    ToReceiveRecords.Add(rec);
                }
            }
            foreach (var rec in record)
            {
                bool IsPullout = rec.IsReceived.Equals(false) && rec.IsPullout.Equals(true);

                if (IsPullout)
                {
                    PullOutRecords.Add(rec);
                }
            }
            foreach (var rec in record)
            {
                bool IsReceived = rec.IsReceived.Equals(true) && rec.IsPullout.Equals(false);

                if (IsReceived)
                {
                    ReceivedRecords.Add(rec);
                }
            }
            if (toReceive == true)
            {
               
            }
            else if (pullout == true)
            {
                
            }
            else if (received == true)
            {
               
            }
            

            PrintRecordsViewModel model = new PrintRecordsViewModel();
            model.record = record;
            model.relnumtext = relnum.ToString();
            if (toReceive == true)
            {
                model.ToReceiveRecords = ToReceiveRecords.OrderBy(x => x.Item).ToList();
            }
            else if (pullout == true)
            {
                model.PullOutRecords = PullOutRecords.OrderBy(x => x.Item).ToList();
            }
            else if (received == true)
            {
                model.ReceivedRecords = ReceivedRecords.OrderBy(x => x.Item).ToList();
            }
            else
            {
                model.ReceivedRecords = ReceivedRecords.OrderBy(x => x.Item).ToList();
                model.PullOutRecords = PullOutRecords.OrderBy(x => x.Item).ToList();
                model.ToReceiveRecords = ToReceiveRecords.OrderBy(x => x.Item).ToList();
            }
            model.dateReleased = dateOnly;
            model.depttxt = department;
            model.totalQuantity = TotalQuantity;

            return View(model);
        }
    }
}
