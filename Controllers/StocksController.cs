using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.Shared;
using IIMSv1.Models.ViewModel;
using System.ComponentModel;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IIMSv1.Controllers
{
    public class StocksController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public StocksController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Edit Stocks
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _EditStocks(StockEditModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(model.returnUrl);
            }
            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id.Equals(model.ItemId));

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);

            if (item != null)
            {
                item.itemQuantity = model.Quantity;
                await _context.SaveChangesAsync(currUser.Id, "Edit Stocks");

                ItemQuantityHistory newQuantityHistory = new ItemQuantityHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = currUser.Id.ToString(),
                    ItemId = item.Id.ToString(),
                    QuantityFrom = model.QuantityBefore,
                    QuantityTo = model.Quantity,
                    Activity = "Edited Stocks",
                    Date = date,
                    Time = time
                };
                _context.AddAsync(newQuantityHistory);
                _context.SaveChanges();
                TempData["alert"] = Global.GenerateToast("",
                    "<b>ITEM: </b>" + item.ItemName
                    + "<ul>"
                    + "<li> Quantity Before: " + model.QuantityBefore + " </li> "
                    + "<li> New Quantity: " + model.Quantity + "</li> "
                    + "</ul>",
                    "topRight",
                    Global.BsStatusColor.Success,
                    Global.BsStatusIcon.None);

                return LocalRedirect(model.returnUrl);
            }
            else
            {
                TempData["alert"] = Global.GenerateToast("STOCKS", "Something went wrong", "topRight", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                return LocalRedirect(model.returnUrl);
            }

        }

        //Add More Stocks
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddStocks(StockEditModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(model.returnUrl);
            }
            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id.Equals(model.ItemId));

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);

            if (item != null)
            {
                var addQuantity = model.QuantityBefore + model.Quantity;
                item.itemQuantity = addQuantity;
                await _context.SaveChangesAsync(currUser.Id, "Added Stocks");

                ItemQuantityHistory newQuantityHistory = new ItemQuantityHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = currUser.Id.ToString(),
                    ItemId = item.Id.ToString(),
                    QuantityFrom = model.QuantityBefore,
                    QuantityTo = addQuantity,
                    Activity = "Added Stocks",
                    Date = date,
                    Time = time
                };
                _context.AddAsync(newQuantityHistory);
                _context.SaveChanges();
                TempData["alert"] = Global.GenerateToast("",
                    "<b>ITEM: </b>" + item.ItemName
                    + "<ul>"
                    + "<li> Quantity Before: " + model.QuantityBefore + " </li> " 
                    + "<li> Added Quantity: " + model.Quantity + "</li> " 
                    + "<li> Total Quantity: " + addQuantity + "</li> "
                    + "</ul>",
                    "topRight",
                    Global.BsStatusColor.Success,
                    Global.BsStatusIcon.None);

                return LocalRedirect(model.returnUrl);
            }
            else
            {
                TempData["alert"] = Global.GenerateToast("STOCKS", "Something went wrong", "topRight", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                return LocalRedirect(model.returnUrl);
            }

        }


        //Add Stocks
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddItem(StockInputModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(model.returnUrl);
            }
            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id.Equals(model.Item));

            if (item != null)
            {
               
                var YearNow = DateTime.Now.Year;
                DateOnly date = DateOnly.FromDateTime(DateTime.Now);
                TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
                ItemQuantityHistory newQuantityHistory = new ItemQuantityHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = currUser.Id.ToString(),
                    ItemId = item.Id.ToString(),
                    QuantityFrom = 0,
                    QuantityTo = model.Quantity,
                    Activity = "New Stocks",
                    Date = date,
                    Time = time
                };
                item.itemQuantity = model.Quantity;
                await _context.SaveChangesAsync(currUser.Id, "Stocks: New Item added to stocks table");

                _context.QuantityHistory.AddAsync(newQuantityHistory);
                _context.SaveChanges();
                TempData["alert"] = Global.GenerateToast("", "<b>NEW ITEM: </b>" + item.ItemName, "topRight", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                return LocalRedirect(model.returnUrl);
            }
            else
            {
                TempData["alert"] = Global.GenerateToast("NEW STOCKS", "Something went wrong", "topRight", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                return LocalRedirect(model.returnUrl);
            }

        }

        // Search Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Filter(
            string scope,
            string status,
            string search,
            string Supply,
            string Itemtype,
            int page,
            int entries)
        {
            return RedirectToAction("Index", "Stocks", new { search = search, scope = scope, Supply = Supply, type = Itemtype, status = status, page = page, entries = entries });
        }

        //Index
        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index(
            string status = "",
            string search = "",
            string scope = "",
            string Supply = "",
            string type = "",
            int page = 1,
            int entries = 10)
        {
            AccountUser? currentUser = await _userManager.GetUserAsync(User);

            AccountUser? user = await _context.AccountCredentials
               .Include(x => x.Employee)
               .ThenInclude(x => x.department)
               .ThenInclude(x => x.DepartmentCluster)
               .SingleOrDefaultAsync(x => x.Id.Equals(currentUser.Id));

            string supplytxt = Supply.Trim().ToString();
            string Itemtypetxt = type;
            string searchtxt = search.Trim();

            string scopetxt = scope.Trim().ToString();

            //Query
            IQueryable<Items> SearchItems = _context.Items
                //.Include(x => x.itemUnit)
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .Where(x => x.ItemCode.Contains(searchtxt)
                || x.ItemName.Contains(searchtxt));

            IQueryable<Items> ItemTypeItems = SearchItems
                .Where(x => x.ItemTypeId.Contains(Itemtypetxt)
               && x.itemType.SuppliesId.Contains(supplytxt));

            List<Items> CItems = new List<Items>();
            if (status != "")
            {
                if (status == "InStock")
                {
                    List<Items> items = ItemTypeItems
                        .Where(x => x.itemQuantity > 0)
                        .ToList();

                    foreach (var item in items)
                    {
                        CItems.Add(item);
                    }
                }
                else
                {
                    List<Items> items = ItemTypeItems
                        .Where(x => x.itemQuantity == 0)
                        .ToList();

                    foreach (var item in items)
                    {
                        CItems.Add(item);
                    }
                    
                }
            }
            else
            {
                List<Items> items = ItemTypeItems
                    .Where(x => x.itemQuantity != null)
                  .ToList();
                foreach (var item in items)
                {
                    CItems.Add(item);
                }
            }

            int recordsperpage = entries;
            int recordtotal = CItems.Count();
            int pagetotal = Global.GetPaginationTotalPages(recordtotal, recordsperpage);

            if (page < 1 || (page > pagetotal && recordtotal > 0))
            {
                page = 1;
            }

            List<Items> WithStockItems = CItems
                  .OrderBy(x => x.ItemName)
                  .Skip(recordsperpage * (page - 1))
                  .Take(recordsperpage)
                  .Where(x => x.IsEnabled.Equals(true))
                  .ToList();

            //List
            List<ItemPrice> itemPrices = _context.ItemPrice
                .Where(x => x.IsEnabled == true)
                .ToList();

            List<ItemSpecType> orderSpecType = _context.ItemSpecType
               .OrderBy(x => x.itemSpecType)
               .Where(x => x.IsEnabled.Equals(true))
               .ToList();

            List<ItemSpecType> itemSpecTypes = orderSpecType
                .OrderBy(x => x.itemSpecType.EndsWith("OTHERS") || x.itemSpecType.EndsWith("Others") || x.itemSpecType.EndsWith("others"))
                .Where(x => x.IsEnabled.Equals(true))
                .ToList();

            List<Supplies> itemSupplies = _context.Supplies
            .Where(x => x.IsEnabled.Equals(true))
               .ToList();

            List<ItemUnit> itemUnits = _context.ItemUnits
            .Where(x => x.IsEnabled.Equals(true))
            .ToList();

            List<ItemType> itemTypes = _context.ItemType
            .Where(x => x.IsEnabled.Equals(true))
                .ToList();

            List<Items> Items = _context.Items
                  .OrderBy(x => x.ItemName)
                  .Where(x => x.IsEnabled.Equals(true) && x.itemQuantity == null)
                  .ToList();
            
            
            //Model Binding
            StocksViewModel model = new StocksViewModel();
            model.PaginationModel = new PaginationModel("Index", "Stocks", searchtxt, recordtotal, recordsperpage, page);
            model.status = status;
            model.supplies = itemSupplies;
            model.StockInputModel.items = Items;
            model.WithStockItems = WithStockItems;

            model.perPage = entries.ToString();
            model.SearchText = searchtxt;
            model.ItemTypeValue = Itemtypetxt;
            model.SupplyTextValue = supplytxt;

            model.scope = scopetxt;

            Supplies? supplyFilter = await _context.Supplies
           .SingleOrDefaultAsync(x => x.Id.Equals(supplytxt) && x.IsEnabled.Equals(true));

            ItemType? itemTypeFilter = await _context.ItemType
            .SingleOrDefaultAsync(x => x.Id.Equals(Itemtypetxt) && x.IsEnabled.Equals(true));
            if (supplyFilter != null)
            {
                model.SupplyText = supplyFilter.supplyName;
            }
            else
            {
                model.SupplyText = "";
            }
            if (itemTypeFilter != null)
            {
                model.ItemTypeText = itemTypeFilter.itemType;
            }
            else
            {
                model.ItemTypeText = "";
            }

            ViewBag.Modal = TempData["ModalState"];
            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
