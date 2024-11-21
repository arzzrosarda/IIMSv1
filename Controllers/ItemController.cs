using IIMSv1.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Data;
using IIMSv1.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Azure;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using System.Globalization;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Humanizer.Localisation;
using System;
using Humanizer;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using Microsoft.AspNetCore.Http.Extensions;
using IIMSv1.Models.ViewModel;

namespace IIMSv1.Controllers
{
    public class ItemController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public ItemController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Check Item Type
        [Route("{controller}/{action}")]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckDuplicateItemType(string SupplyId, string ItemType)
        {
            ItemType? Itemtype = await _context.ItemType
                .SingleOrDefaultAsync(x => x.SuppliesId != SupplyId && x.itemType.Equals(ItemType));

            if (Itemtype != null)
            {
                return Json("The name of the Item Type already exists.");
            }
            else
            {
                return Json(true);
            }
        }

        //Check Supply Name
        [Route("{controller}/{action}")]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckDuplicateSupply(string Supply)
        {
            Supplies? supply = await _context.Supplies
                .SingleOrDefaultAsync(x => x.supplyName.Equals(Supply));

            if (supply != null)
            {
                return Json("The name of the supply already exists.");
            }
            else
            {
                return Json(true);
            }
        }

        //Check Unit Name
        [Route("{controller}/{action}")]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckDuplicateUnit(string Unit)
        {
            ItemUnit? unit = await _context.ItemUnits
                .SingleOrDefaultAsync(x => x.UnitName.Equals(Unit));

            if (unit != null)
            {
                return Json("The name of the unit already exists.");
            }
            else
            {
                return Json(true);
            }
        }

        //Check Spec Type
        [Route("{controller}/{action}")]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckDuplicateSpecType(string SpecType)
        {
            AccountUser? user = await _userManager.GetUserAsync(User);

            AccountUser? currUser = await _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.Id.Equals(user.Id));

            ItemSpecType? type = await _context.ItemSpecType
                .SingleOrDefaultAsync(x => x.itemSpecType.Equals(SpecType));

            if (type != null)
            {
                return Json("The type of description already exists.");
            }
            else
            {
                return Json(true);
            }
        }



        //CheckItemTypeDuplicate
        [Route("{controller}/{action}")]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckItemTypeDuplicate(string OtherItemType, string Supply)
        {
            ItemType? itemtype = await _context.ItemType
                .Include(x => x.supplies)
                .SingleOrDefaultAsync(x => x.itemType.Equals(OtherItemType) && x.SuppliesId.Equals(Supply));

            if (itemtype != null)
            {
                return Json("This Item Type is already exists");
            }
            else
            {
                return Json(true);
            }
        }

        //Deactivate Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _ActivateItem(string ItemId, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }


            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id == ItemId);

            item.IsEnabled = true;
            await _context.SaveChangesAsync(currUser.Id, "Item: Enabled");
            TempData["alert"] = Global.GenerateToast(item.ItemName, "Enabled Successfully", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
            return LocalRedirect(returnUrl);
        }

        //Deactivate Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _DeactivateItem(string ItemId, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }


            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id == ItemId);

            item.IsEnabled = false;
            await _context.SaveChangesAsync(currUser.Id, "Item: Disabled");
            TempData["alert"] = Global.GenerateToast(item.ItemName, "Disabled Successfully", "topRight", Global.BsStatusIcon.Warning, Global.BsStatusColor.Warning);
            return LocalRedirect(returnUrl);
        }

        //Update Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _UpdateItem(EditItemInputModel model, string returnUrl, string[] NewItemSpecs)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }

            Items? item = await _context.Items
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .SingleOrDefaultAsync(x => x.Id == model.ItemId);

            ItemPrice? price = await _context.ItemPrice
                .SingleOrDefaultAsync(x => x.itemId == model.ItemId);

            if (model.ItemType == "others")
            {
                var date = DateOnly.FromDateTime(DateTime.Now);
                var time = TimeOnly.FromDateTime(DateTime.Now);
                ItemType newItemType = new ItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    itemType = model.OtherItemType.Trim().ToString(),
                    SuppliesId = model.Supply.ToString(),
                    IsEnabled = true,
                };
                _context.AddAsync(newItemType);
                _context.SaveChanges();

                ItemType? itemType = await _context.ItemType
                    .Include(x => x.supplies)
                    .SingleOrDefaultAsync(x => x.Id.Equals(newItemType.Id));

                string[] Supptype = itemType.supplies.supplyName.Split(" ");
                string Supplytype = "";
                foreach (var ty in Supptype)
                {
                    Supplytype += ty.Substring(0, 1);
                }

                
                string[] splitItemCode = item.ItemCode.Split("_");


                        string ItemCode = Supplytype + "_" + itemType.itemType.Trim().ToUpper();
                        List<Items> getNewitem = _context.Items
                            .Where(x => x.ItemCode.Contains(ItemCode))
                            .ToList();

                        var itemCodeCount = getNewitem.Count() + 1;
                        item.ItemCode = ItemCode + "_" + itemCodeCount.ToString().Trim();

                item.ItemName = itemType.itemType + " ";
                item.ItemTypeId = newItemType.Id;
                item.itemUnitId = model.Unit.Trim().ToString();
                item.dateUpdated = date;
                item.timeUpdated = time;

                List<ItemSpecs> getItemSpecs = _context.ItemSpecs
                    .Where(x => x.itemId.Equals(item.Id))
                    .ToList();

                foreach (var specitem in getItemSpecs)
                {
                    _context.Remove(specitem);
                }
                foreach (var specs in NewItemSpecs)
                {
                    if (specs != null)
                    {
                        string[] Value = specs.Split(',');
                        string SpecTypeId = Value[0];
                        string SpecValue = "";
                        foreach (var val in Value)
                        {
                            if (val != SpecTypeId)
                            {
                                SpecValue += val.ToString();
                            }
                        }
                        ItemSpecValue? itemSpecValue = await _context.ItemSpecValue
                            .Include(x => x.ItemSpecType)
                            .SingleOrDefaultAsync(x => x.itemSpecValue.Equals(SpecValue) && x.SpecTypeId.Equals(SpecTypeId));

                        if (itemSpecValue != null)
                        {
                                ItemSpecs newSpecs = new ItemSpecs()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    itemId = item.Id,
                                    SpecValueId = itemSpecValue.Id,
                                };
                                _context.AddAsync(newSpecs);
                        }
                        else
                        {
                            ItemSpecValue newItemSpecValue = new ItemSpecValue()
                            {
                                Id = Guid.NewGuid().ToString(),
                                itemSpecValue = SpecValue,
                                SpecTypeId = SpecTypeId,
                                IsEnabled = true
                            };
                            ItemSpecs newSpecs = new ItemSpecs()
                            {
                                Id = Guid.NewGuid().ToString(),
                                itemId = item.Id,
                                SpecValueId = newItemSpecValue.Id,
                            };
                            _context.AddAsync(newItemSpecValue);
                            _context.AddAsync(newSpecs);
                        }
                    }
                }

                await _context.SaveChangesAsync(currUser.Id, "Item: Update Changes");
                Items? getItem = await _context.Items
                        .SingleOrDefaultAsync(x => x.Id.Equals(item.Id));

                List<ItemSpecs> getspec = _context.ItemSpecs
                    .Include(x => x.itemSpecValue)
                    .ThenInclude(x => x.ItemSpecType)
                    .OrderBy(x => x.itemSpecValue.ItemSpecType.itemSpecType)
                    .Where(x => x.itemId.Equals(item.Id))
                    .ToList();

                if (getspec.Count() > 0)
                {
                    foreach (var spec in getspec)
                    {
                        if (item != null)
                        {
                            item.ItemName = item.ItemName + spec.itemSpecValue.itemSpecValue + " ";
                        }
                    }
                }
                
                await _context.SaveChanges(currUser.Id, "Item: Update Changes");

                TempData["alert"] = Global.GenerateToast(getItem.ItemName, "Changes Successfully Saved", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
                return LocalRedirect(returnUrl);

            }
            else
            {
                var date = DateOnly.FromDateTime(DateTime.Now);
                var time = TimeOnly.FromDateTime(DateTime.Now);

                ItemType? itemType = await _context.ItemType
                        .Include(x => x.supplies)
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.Item));

                string[] Supptype = itemType.supplies.supplyName.Split(" ");
                string Supplytype = "";
                foreach (var ty in Supptype)
                {
                    Supplytype += ty.Substring(0, 1);
                }

                string ItemCode = Supplytype + "_" + itemType.itemType.Trim().ToUpper();
                List<Items> getNewitem = _context.Items
                    .Where(x => x.ItemCode.Contains(ItemCode))
                    .ToList();

                var itemCodeCount = getNewitem.Count() + 1;

                string[] splitItemCode = item.ItemCode.Split("_");
                if (splitItemCode[0] != Supplytype || splitItemCode[1] != itemType.itemType.Trim().ToUpper())
                {
                    item.ItemCode = ItemCode + "_" + itemCodeCount.ToString().Trim();
                }
                
                item.ItemName = itemType.itemType + " ";
                item.ItemTypeId = model.ItemType.ToString();
                item.itemUnitId = model.Unit.Trim().ToString();
                item.dateUpdated = date;
                item.timeUpdated = time;

                List<ItemSpecs> getItemSpecs = _context.ItemSpecs
                    .Where(x => x.itemId.Equals(item.Id))
                    .ToList();

                foreach(var specitem in getItemSpecs)
                {
                    _context.Remove(specitem);
                }
                foreach (var specs in NewItemSpecs)
                {
                    if (specs != null)
                    {
                        string[] Value = specs.Split(',');
                        string SpecTypeId = Value[0];
                        string SpecValue = "";
                        foreach (var val in Value)
                        {
                            if (val != SpecTypeId)
                            {
                                SpecValue += val.ToString();
                            }
                        }
                        ItemSpecValue? itemSpecValue = await _context.ItemSpecValue
                            .Include(x => x.ItemSpecType)
                            .SingleOrDefaultAsync(x => x.itemSpecValue.Equals(SpecValue) && x.SpecTypeId.Equals(SpecTypeId));

                        if (itemSpecValue != null)
                        {
                                ItemSpecs newSpecs = new ItemSpecs()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    itemId = item.Id,
                                    SpecValueId = itemSpecValue.Id,
                                };
                                _context.AddAsync(newSpecs);
                        }
                        else
                        {
                            ItemSpecValue newItemSpecValue = new ItemSpecValue()
                            {
                                Id = Guid.NewGuid().ToString(),
                                itemSpecValue = SpecValue,
                                SpecTypeId = SpecTypeId,
                                IsEnabled = true
                            };
                            ItemSpecs newSpecs = new ItemSpecs()
                            {
                                Id = Guid.NewGuid().ToString(),
                                itemId = item.Id,
                                SpecValueId = newItemSpecValue.Id,
                            };
                            _context.AddAsync(newItemSpecValue);
                            _context.AddAsync(newSpecs);
                        }
                    }
                }
                await _context.SaveChangesAsync(currUser.Id, "Item: Update Changes");
                Items? getItem = await _context.Items
                    .SingleOrDefaultAsync(x => x.Id.Equals(item.Id));

                List<ItemSpecs> getspec = _context.ItemSpecs
                    .Include(x => x.itemSpecValue)
                    .ThenInclude(x => x.ItemSpecType)
                    .OrderBy(x => x.itemSpecValue.ItemSpecType.itemSpecType)
                    .Where(x => x.itemId.Equals(item.Id))
                    .ToList();

                if (getspec.Count() > 0)
                {
                    foreach (var spec in getspec)
                    {

                        if (item != null)
                        {
                            item.ItemName = item.ItemName + spec.itemSpecValue.itemSpecValue + " ";
                        }
                    }
                }
                await _context.SaveChanges(currUser.Id, "Item: Update Changes");

                TempData["alert"] = Global.GenerateToast(getItem.ItemName, "Changes Successfully Saved", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
                return LocalRedirect(returnUrl);
            }
        }

        //GetSpecs
        [HttpPost]
        public async Task<IActionResult> GetSpecs(string ItemId)
        {
            List<ItemSpecs> itemSpecs = _context.ItemSpecs
                .Include(x => x.itemSpecValue)
                .ThenInclude(x => x.ItemSpecType)
                .AsNoTracking()
                .Where(x => x.itemId.Equals(ItemId))
                .ToList();

            return Json(itemSpecs);
        }

        //Delete Item 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _DeleteItem(string ItemId, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }
            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id == ItemId);

            _context.Items.Remove(item);
            await _context.SaveChangesAsync(currUser.Id, "Item: Deleted");
            TempData["alert"] = Global.GenerateToast(item.ItemName, "Deleted Successfully", "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
            return LocalRedirect(returnUrl);
        }

        //GetSupplyId 
        [HttpPost]
        public IActionResult GetSupplyId(string supplyId)
        {
            List<ItemType> itemTypes = _context.ItemType
                .OrderBy(x => x.itemType)
                .Where(x => x.SuppliesId == supplyId)
                .ToList();

            return Json(itemTypes);
        }
        
        //Add Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddItem(_ItemInputModel model, string returnUrl, string[] NewItemSpecs)
        {
            AccountUser? currentUser = await _userManager.GetUserAsync(User);

            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.Id.Equals(currentUser.Id));

            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }

            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);
            ItemType? itemType = await _context.ItemType
                .Include(x => x.supplies)
                .SingleOrDefaultAsync(x => x.Id.Equals(model.ItemType));

            string[] Supptype = itemType.supplies.supplyName.Split(" ");

            string Supplytype = "";
            foreach (var ty in Supptype)
            {
                Supplytype += ty.Substring(0, 1);
            }

            string ItemCode = Supplytype + "_" + itemType.itemType.Trim().ToUpper();
            List<Items> getNewitem = _context.Items
                .Where(x => x.ItemCode.Contains(ItemCode))
                .ToList();

            var itemCodeCount = getNewitem.Count() + 1;
            Items newItem = new Items()
            {
                Id = Guid.NewGuid().ToString(),
                ItemCode = ItemCode + "_" + itemCodeCount,
                ItemName = itemType.itemType.Trim().ToString() + " ",
                ItemTypeId = model.ItemType,
                itemUnitId = model.Unit.Trim().ToString(),
                dateCreated = date,
                timeCreated = time,
                dateUpdated = date,
                timeUpdated = time,
                IsEnabled = true,
            };
            foreach (var specs in NewItemSpecs)
            {
                if (specs != null)
                {
                    string[] Value = specs.Split(',');
                    string SpecTypeId = Value[0];
                    string SpecValue = "";
                    foreach (var val in Value)
                    {
                        if (val != SpecTypeId)
                        {
                            SpecValue += val.ToString();
                        }
                    }
                    ItemSpecValue? itemSpecValue = await _context.ItemSpecValue
                        .Include(x => x.ItemSpecType)
                        .SingleOrDefaultAsync(x => x.itemSpecValue.Equals(SpecValue) && x.SpecTypeId.Equals(SpecTypeId));

                    if (itemSpecValue != null)
                    {
                        ItemSpecs newSpecs = new ItemSpecs()
                        {
                            Id = Guid.NewGuid().ToString(),
                            itemId = newItem.Id,
                            SpecValueId = itemSpecValue.Id,
                        };
                        _context.AddAsync(newSpecs);
                    }
                    else
                    {
                        ItemSpecValue newItemSpecValue = new ItemSpecValue()
                        {
                            Id = Guid.NewGuid().ToString(),
                            itemSpecValue = SpecValue,
                            SpecTypeId = SpecTypeId,
                            IsEnabled = true
                        };
                        ItemSpecs newSpecs = new ItemSpecs()
                        {
                            Id = Guid.NewGuid().ToString(),
                            itemId = newItem.Id,
                            SpecValueId = newItemSpecValue.Id,
                        };

                        _context.AddAsync(newItemSpecValue);
                        _context.AddAsync(newSpecs);
                    }
                }
            }
            _context.AddAsync(newItem);
            await _context.SaveChangesAsync(currentUser.Id, "Item : Added a new item");

            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id.Equals(newItem.Id));

            var getSpecs = "";
            List<ItemSpecs> getspec = _context.ItemSpecs
                .Include(x => x.itemSpecValue)
                .ThenInclude(x => x.ItemSpecType)
                .OrderBy(x => x.itemSpecValue.ItemSpecType.itemSpecType)
                .Where(x => x.itemId.Equals(item.Id))
                .ToList();

            if (getspec.Count() > 0)
            {
                foreach (var spec in getspec)
                {
                    if (item != null)
                    {
                        item.ItemName = item.ItemName + spec.itemSpecValue.itemSpecValue + " ";

                    }
                }
            }
            await _context.SaveChanges(currentUser.Id, "Item: Update Changes");
            TempData["alert"] = Global.GenerateToast(newItem.ItemName, "Added Successfully", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
            return LocalRedirect(returnUrl);
        }

        // Search Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Filter(
            string dateFrom,
            string dateTo,
            string status,
            string search,
            string Supply,
            int page,
            int entries)
        {
                return RedirectToAction("Index", "Item", new { search = search, dateFrom = dateFrom, dateTo = dateTo, Supply = Supply, status = status, page = page, entries = entries });
        }

        //Index
        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index(
            string status = "",
            string search = "",
            string dateFrom = "",
            string dateTo = "",
            string Supply = "",
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
            string searchtxt = search.Trim();

            //Full Date Range
            string DateFullFrom = dateFrom;
            string DateFullTo = dateTo;
            DateOnly? DateStart = null;
            DateOnly? DateEnd = null;
            if (dateFrom != "" && dateTo != "")
            {
                DateTime FullDate1 = Convert.ToDateTime(dateFrom);
                DateOnly start = DateOnly.FromDateTime(FullDate1);
                DateStart = start;

                DateTime FullDate2 = Convert.ToDateTime(dateTo);
                DateOnly end = DateOnly.FromDateTime(FullDate2);
                DateEnd = end;
            }

            IQueryable<Items> SearchItems = _context.Items
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .OrderBy(x => x.ItemName)
                .ThenBy(x => x.dateCreated)
                .Where(x => x.ItemCode.Contains(searchtxt)
                || x.ItemName.Contains(searchtxt));

            IQueryable<Items> ItemTypeItems = SearchItems
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .Where(x => x.itemType.SuppliesId.Contains(supplytxt));

            List<Items> Containeritems = new List<Items>();
            if (DateStart != null && DateEnd != null)
            {
                List<Items> items = ItemTypeItems
              .Where(x => x.dateCreated >= DateStart
                && x.dateCreated <= DateEnd)
              .ToList();
                foreach (var item in items)
                {
                    Containeritems.Add(item);
                }
            }
            else
            {
                List<Items> items = ItemTypeItems
                .ToList();

                foreach (var item in items)
                {
                    Containeritems.Add(item);
                }
            }

            List<Items> EDItem = new List<Items>();
            if (status == "false")
            {
                List<Items> getitems = Containeritems
                  .Where(x => x.IsEnabled.Equals(false))
                   .ToList();
                foreach (var item in getitems)
                {
                    EDItem.Add(item);
                }
            }
            else if (status == "true")
            {
                List<Items> getitems = Containeritems
                  .Where(x => x.IsEnabled.Equals(true))
                   .ToList();
                foreach (var item in getitems)
                {
                    EDItem.Add(item);
                }
            }
            else
            {
                List<Items> getitems = Containeritems
                    .ToList();
                foreach (var item in getitems)
                {
                    EDItem.Add(item);
                }
            }

            int recordsperpage = entries;
            int recordtotal = EDItem.Count();
            int pagetotal = Global.GetPaginationTotalPages(recordtotal, recordsperpage);

            if (page < 1 || (page > pagetotal && recordtotal > 0))
            {
                page = 1;
            }
            //List

            List<ItemSpecType> orderSpecType = _context.ItemSpecType
               .OrderBy(x => x.itemSpecType)
               .Where(x => x.IsEnabled.Equals(true))
               .ToList();

            List<ItemSpecType> itemSpecTypes = orderSpecType
                .OrderBy(x => x.itemSpecType.EndsWith("OTHERS"))
                .Where(x => x.IsEnabled.Equals(true))
                .ToList();

            List<Supplies> itemSupplies = _context.Supplies
                .OrderBy(x => x.supplyName)
               .Where(x => x.IsEnabled.Equals(true))
               .ToList();

            List<ItemUnit> itemUnits = _context.ItemUnits
                .OrderBy(x => x.UnitName)
                 .Where(x => x.IsEnabled.Equals(true))
                .ToList();

            Supplies? supplyFilter = await _context.Supplies
            .SingleOrDefaultAsync(x => x.Id.Equals(supplytxt) && x.IsEnabled.Equals(true));

            List<Items> enabledDisabledItem = EDItem
                .Skip(recordsperpage * (page - 1))
                .Take(recordsperpage)
                .ToList();

            //Model Binding
            ItemViewModel model = new ItemViewModel();
            model.PaginationModel = new PaginationModel("Index", "Item", searchtxt, recordtotal, recordsperpage, page);
            model.status = status;
            model.items = enabledDisabledItem;

            model.SearchText = searchtxt;
            model.SupplyTextValue = supplytxt;
            if (supplyFilter != null)
            {
                model.SupplyText = supplyFilter.supplyName;
            }
            else
            {
                model.SupplyText = "";
            }
            model.perPage = entries.ToString();
            model.dateFrom = DateStart.ToString();
            model.dateTo = DateEnd.ToString();
            model.dateStart = DateFullFrom;
            model.dateEnd = DateFullTo;

            model.ItemInputModel.itemSpecTypes = itemSpecTypes;
            model.ItemInputModel.supplies = itemSupplies;
            model.ItemInputModel.unit = itemUnits;

            model.EditItemInputModel.supplies = itemSupplies;
            model.EditItemInputModel.itemSpecTypes = itemSpecTypes;
            model.EditItemInputModel.unit = itemUnits;

            ViewBag.alert = TempData["alert"];
            return View(model);
        }

    }
}
