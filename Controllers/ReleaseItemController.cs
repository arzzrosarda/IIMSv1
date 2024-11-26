using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.ViewModel;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IIMSv1.Controllers
{
    public class ReleaseItemController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public ReleaseItemController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //_ReleaseItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _ReleaseItem(string DeptId, string[] ItemQuantity)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "ReleaseItem");
            }

            DateTime dateTime = DateTime.Now;
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            Department? department = await _context.Departments
                .SingleOrDefaultAsync(x => x.Id.Equals(DeptId));

            if (department != null)
            {
                var GetItem = "";

                string releaseNum = "RLSD-" + date.Year;
                List<Released> releaseds = _context.Released
                    .Where(x => x.ReleaseNumber.Contains(releaseNum))
                    .ToList();

                List<int> filteredRel = new List<int>();
                foreach (var rel in releaseds)
                {
                    string[] split = rel.ReleaseNumber.Split('-');
                    int num = int.Parse(split[2]);
                    bool isFiltered = filteredRel.Any(x => x.Equals(num));

                    if (!isFiltered)
                    {
                        filteredRel.Add(num);
                    }
                }

                int Count = 0;

                if (filteredRel.Count > 0) 
                { 
                    Count = filteredRel.Max() + 1;
                }
                else
                {
                    Count = 1;
                }
                string CountReleased = Count.ToString("000000");

                
                foreach (var q in ItemQuantity)
                {
                    string[] valSplit = q.Split(',');

                    Items? item = await _context.Items
                   .SingleOrDefaultAsync(x => x.Id.Equals(valSplit[0]));

                    
                    int Quantity = Convert.ToInt32(valSplit[1]);
                    var MinusQuantity = item.itemQuantity - Quantity;
                    ItemQuantityHistory newQuantityHistory = new ItemQuantityHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = currUser.Id.ToString(),
                        ItemId = item.Id.ToString(),
                        QuantityFrom = (int)item.itemQuantity,
                        QuantityTo = (int)MinusQuantity,
                        Activity = "Released: " + department.NormalizedName,
                        Date = date,
                        Time = time
                    };
                        
                        Released newRelease = new Released()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ItemId = item.Id.ToString(),
                            ReleaseNumber = releaseNum + "-" + CountReleased.ToString(),
                            DepartmentId = department.Id.ToString(),
                            ReleaseQuantity = Quantity,
                            DateReleased = date,
                            TimeReleased = time,
                            Received = false,
                            Pullout = false
                            
                        };
                        ReleaseHistory transferHistory = new ReleaseHistory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = currUser.Id.ToString(),
                            Date = date,
                            Time = time,
                            ReleaseId = newRelease.Id.ToString(),
                            OldQuantity = 0,
                            NewQuantity = Quantity,
                            Action = "TO RECEIVE",
                        };
                        _context.AddAsync(newRelease);
                        await _context.SaveChangesAsync(currUser.Id, "Released an item to receive");
                        _context.AddAsync(transferHistory);
                        _context.SaveChanges();

                        Released? rel = await _context.Released
                            .Include(x => x.item)
                            .SingleOrDefaultAsync(x => x.Id.Equals(newRelease.Id));

                        GetItem += rel.item.ItemName + "|||" + Quantity + ",";
                        _context.AddAsync(newQuantityHistory);
                        item.itemQuantity = MinusQuantity;
                        _context.SaveChanges();

                }
                TempData["alert"] = Global.GenerateToast("RELEASE", "Successfully released <strong>" + ItemQuantity.Count() + " Items</strong>" , "topRight", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                TempData["releaseId"] = GetItem;
                TempData["releaseInfo"] = releaseNum + "-" + CountReleased + "," + department.NormalizedName + "," + date.ToShortDateString();
                return RedirectToAction("Index", "ReleaseItem");
            }
            else
            { 
                TempData["alert"] = Global.GenerateToast("ERROR", "Department field is required!", "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.Danger);
                return RedirectToAction("Index", "ReleaseItem");
            }
           
            
        }


        //GetPerItems
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> GetPerItems(string ItemId)
        {
            Items? items = await _context.Items
                //.Include(x => x.itemUnit)
                .SingleOrDefaultAsync(x => x.Id == ItemId);

            return Json(items);
        }
        
        //GetItems
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> GetItems(string ItemId)
        {
            if(ItemId != null)
            {
                string[] SeperatedIds = ItemId.Split(',');
                List<Items> item = new List<Items>();
                foreach (var Id in SeperatedIds)
                {
                    Items? items = await _context.Items
                        .Include(x => x.itemUnit)
                        .SingleOrDefaultAsync(x => x.Id == Id);

                    if (items != null)
                    {
                        item.Add(items);
                    }
                }

                return Json(item);
            }
            else
            {
                return Json(0);
            }
            

            
        }

        //Index
        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index()
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);


            List<Department> departmentList = _context.Departments
                .Where(x => x.IsActive.Equals(true))
                .ToList();

            List<Items> items = _context.Items
                .OrderBy(x => x.ItemName)
                .Where(x => x.itemQuantity != null && x.IsEnabled == true)
                .ToList();

            ReleaseItemViewModel model = new ReleaseItemViewModel();
            model.departments = departmentList;
            model.items = items;

            ViewBag.alert = TempData["alert"];
            ViewBag.Released = TempData["releaseId"];
            ViewBag.releaseInfo = TempData["releaseInfo"];
            return View(model);
        }
    }
}
