﻿using IIMSv1.Models.Shared;
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
using System.Drawing.Drawing2D;
using Newtonsoft.Json;

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

        //_AddSpecType
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddSpecType(AddSpecModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);

            if (currUser == null)
            {
                return NotFound();
            }
            else
            {
                List<ItemSpecType> itemSpecType = _context.ItemSpecType
                    .Where(x => x.itemSpecType.Equals(model.SpecType))
                    .ToList();

                if (itemSpecType.Count() > 0)
                {
                    return Json("0");
                }
                else
                {
                    ItemSpecType newSpecType = new ItemSpecType()
                    {
                        Id = Guid.NewGuid().ToString(),
                        itemSpecType = model.SpecType.Trim().ToString().ToUpper(),
                        IsEnabled = true,
                    };
                    _context.Add(newSpecType);
                    await _context.SaveChanges(currUser.Id, "New Specification Type");

                    List<ItemSpecType> getType = _context.ItemSpecType
                        .OrderBy(x => x.itemSpecType)
                        .Where(x => x.IsEnabled.Equals(true))
                        .ToList();

                    List<ItemSpecType> type = getType
                        .OrderBy(x => x.itemSpecType.EndsWith("OTHERS"))
                        .Where(x => x.IsEnabled.Equals(true))
                        .ToList();

                    return Json(type);
                }

            }
        }

        //_AddUnitType
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddUnitType(AddUnitModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);

            if (currUser == null)
            {
                return NotFound();
            }
            else
            {
                List<ItemUnit> itemUnit = _context.ItemUnits
                    .Where(x => x.UnitName.Equals(model.Unit))
                    .ToList();

                if (itemUnit.Count() > 0)
                {
                    return Json("0");
                }
                else
                {
                    ItemUnit newUnit = new ItemUnit()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UnitName = model.Unit.Trim().ToString().ToUpper(),
                        IsEnabled = true,
                    };
                    _context.Add(newUnit);
                    await _context.SaveChanges(currUser.Id, "New Unit Type");

                    List<ItemUnit> type = _context.ItemUnits
                        .Where(x => x.IsEnabled.Equals(true))
                        .ToList();

                    return Json(type);
                }

            }
        }

        //_AddItemType
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddItemType(AddItemTypeModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);

            if (currUser == null)
            {
                return NotFound();
            }
            else
            {
                List<ItemType> itemType = _context.ItemType
                    .Where(x => x.Id != model.SupplyId && x.itemType.Equals(model.ItemType))
                    .ToList();

                if (itemType.Count() > 0)
                {
                    return Json("0");
                }
                else
                {
                    ItemType newItemType = new ItemType()
                    {
                        Id = Guid.NewGuid().ToString(),
                        itemType = model.ItemType.Trim().ToString(),
                        SuppliesId = model.SupplyId.Trim().ToString(),
                        IsEnabled = true,
                    };
                    _context.Add(newItemType);
                    await _context.SaveChanges(currUser.Id, "New Item Type");

                    List<ItemType> type = _context.ItemType
                        .Where(x => x.IsEnabled.Equals(true) && x.SuppliesId.Equals(model.SupplyId))
                        .ToList();

                    return Json(type);
                }

            }
        }

        //ActionEditType
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> ActionEditType(EditTypeModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            switch (model.actionSel)
            {
                case "Supply Type":
                    Supplies? supplies = await _context.Supplies
                        .SingleOrDefaultAsync(x => x.Id != model.typeId && x.supplyName.Equals(model.type));

                    if(supplies != null)
                    {
                        return Json("0");
                    }
                    else
                    {
                        Supplies? nochanges = await _context.Supplies
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.supplyName.Equals(model.type));

                        if(nochanges != null)
                        {
                            return Json("1");
                        }
                        else
                        {
                            Supplies? getType = await _context.Supplies
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if(getType != null)
                            {
                                getType.supplyName = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                return Json(getType);
                            }
                            else
                            {
                                return Json("0");
                            }
                        }
                        
                    }
                case "Item Type":
                    ItemType? itemType = await _context.ItemType
                        .SingleOrDefaultAsync(x => x.SuppliesId != model.supplyId && x.itemType.Equals(model.type));

                    if (itemType != null)
                    {
                        return Json("0");
                    }
                    else
                    {
                        ItemType? nochanges = await _context.ItemType
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.SuppliesId.Equals(model.supplyId) && x.itemType.Equals(model.type));

                        if (nochanges != null)
                        {
                            return Json("1");
                        }
                        else
                        {
                            ItemType? getType = await _context.ItemType
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if (getType != null)
                            {
                                getType.itemType = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                return Json(getType);
                            }
                            else
                            {
                                return Json("0");
                            }
                        }
                    }
                case "Unit Type":
                    ItemUnit? Unit = await _context.ItemUnits
                        .SingleOrDefaultAsync(x => x.Id != model.typeId && x.UnitName.Equals(model.type));

                    if (Unit != null)
                    {
                        return Json("0");
                    }
                    else
                    {
                        ItemUnit? nochanges = await _context.ItemUnits
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.UnitName.Equals(model.type));

                        if (nochanges != null)
                        {
                            return Json("1");
                        }
                        else
                        {
                            ItemUnit? getType = await _context.ItemUnits
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if (getType != null)
                            {
                                getType.UnitName = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                return Json(getType);
                            }
                            else
                            {
                                return Json("0");
                            }
                        }
                    }
                case "Specification Type":
                    ItemSpecType? SpecType = await _context.ItemSpecType
                        .SingleOrDefaultAsync(x => x.Id != model.typeId && x.itemSpecType.Equals(model.type));

                    if (SpecType != null)
                    {
                        return Json("0");
                    }
                    else
                    {
                        ItemSpecType? nochanges = await _context.ItemSpecType
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.itemSpecType.Equals(model.type));

                        if (nochanges != null)
                        {
                            return Json("1");
                        }
                        else
                        {
                            ItemSpecType? getType = await _context.ItemSpecType
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if (getType != null)
                            {
                                getType.itemSpecType = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                return Json(getType);
                            }
                            else
                            {
                                return Json("0");
                            }
                        }
                    }
                default:
                    return Json("0");

            }
        }

        //CheckforDuplicate
        [Route("{controller}/{action}")]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckforDuplicate(string typeId, string supplyId, string actionSel, string type)
        {
            if(actionSel == "Supply Type")
            {
                Supplies? actType = await _context.Supplies
                .SingleOrDefaultAsync(x => x.Id != typeId && x.supplyName.Equals(type));

                if (actType != null)
                {
                    return Json("The name of the " + actionSel + " already exists.");
                }
                else
                {
                    return Json(true);
                }
            }
            else if(actionSel == "Item Type")
            {
                ItemType? actType = await _context.ItemType
                .SingleOrDefaultAsync(x => x.SuppliesId != supplyId && x.itemType.Equals(type));

                if (actType != null)
                {
                    return Json("The name of the " + actionSel + " already exists.");
                }
                else
                {
                    return Json(true);
                }
            }
            else if(actionSel == "Unit Type")
            {
                ItemUnit? actType = await _context.ItemUnits
                 .SingleOrDefaultAsync(x => x.Id != typeId && x.UnitName.Equals(type));

                if (actType != null)
                {
                    return Json("The name of the " + actionSel + " already exists.");
                }
                else
                {
                    return Json(true);
                }
            }
            else if(actionSel == "Specification Type")
            {
                ItemSpecType? actType = await _context.ItemSpecType
                  .SingleOrDefaultAsync(x => x.Id != typeId && x.itemSpecType.Equals(type));

                if (actType != null)
                {
                    return Json("The name of the " + actionSel + " already exists.");
                }
                else
                {
                    return Json(true);
                }
            }
            else
            {
                return Json(true);
            }
            
        }

        //Add Supply Type
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddSupply(AddSupplyModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);

            if (currUser == null) { 
                return NotFound();
            }
            else
            {
                List<Supplies> supplies = _context.Supplies
                    .Where(x => x.IsEnabled.Equals(true) && x.supplyName.Equals(model.Supply))
                    .ToList();

                if(supplies.Count() > 0)
                {
                    return Json("0");
                }
                else
                {
                    Supplies newSupply = new Supplies()
                    {
                        Id = Guid.NewGuid().ToString(),
                        supplyName = model.Supply.Trim().ToString(),
                        IsEnabled = true,
                    };
                    _context.Add(newSupply);
                    await _context.SaveChanges(currUser.Id, "New Supply Type");

                    List<Supplies> supp = _context.Supplies
                        .Where(x => x.IsEnabled.Equals(true))
                        .ToList();

                    return Json(supp);
                }
                
            }
        }

        //Check Item Type
        [Route("{controller}/{action}")]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckDuplicateItemType(string SupplyId, string ItemType)
        {
            ItemType? Itemtype = await _context.ItemType
                .SingleOrDefaultAsync(x => x.SuppliesId == SupplyId && x.itemType.Equals(ItemType));

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
                return Json("The type of specification already exists.");
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
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(returnUrl);
            }


            Items? item = await _context.Items
                .Include(x => x.itemType)
                .SingleOrDefaultAsync(x => x.Id == ItemId);

            item.IsEnabled = true;
            await _context.SaveChangesAsync(currUser.Id, "Item: Enabled");
            TempData["alert"] = Global.GenerateToast(item.ItemName, "Enabled Successfully", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
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
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(returnUrl);
            }


            Items? item = await _context.Items
                .Include(x => x.itemType)
                .SingleOrDefaultAsync(x => x.Id == ItemId);

            item.IsEnabled = false;
            await _context.SaveChangesAsync(currUser.Id, "Item: Disabled");
            TempData["alert"] = Global.GenerateToast(item.ItemName, "Disabled Successfully", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
            return LocalRedirect(returnUrl);
        }

        //Update Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _UpdateItem(EditItemInputModel model, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(returnUrl);
            }

            Items? item = await _context.Items
                .Include(x => x.itemType)
                .ThenInclude(x => x.supplies)
                .SingleOrDefaultAsync(x => x.Id == model.ItemId);

            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);

            ItemType? itemType = await _context.ItemType
                    .Include(x => x.supplies)
                    .SingleOrDefaultAsync(x => x.Id.Equals(model.Item));

            item.ItemTypeId = model.ItemType.ToString();
            item.itemUnitId = model.Unit.Trim().ToString();
            item.dateUpdated = date;
            item.timeUpdated = time;

            List<DescriptionModel> descriptionModels = new List<DescriptionModel>();
            foreach (var specs in model.NewItemSpecs)
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
                    DescriptionModel newDescriptionModel = new DescriptionModel()
                    {
                        Description = SpecTypeId,
                        Details = SpecValue,
                    };

                    descriptionModels.Add(newDescriptionModel);
                }
            }
            string serializeDescription = JsonConvert.SerializeObject(descriptionModels);

            item.itemDescription = serializeDescription;

            await _context.SaveChangesAsync(currUser.Id, "Item: Update Changes");
            Items? getItem = await _context.Items
                .Include(x => x.itemType)
                .SingleOrDefaultAsync(x => x.Id.Equals(item.Id));

            await _context.SaveChanges(currUser.Id, "Item: Update Changes");

            TempData["alert"] = Global.GenerateToast(getItem.ItemName, "Changes Successfully Saved", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> CheckforNoChanges(EditItemInputModel model)
        {

            List<DescriptionModel> descriptionModels = new List<DescriptionModel>();
            foreach (var specs in model.NewItemSpecs)
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
                    DescriptionModel newDescriptionModel = new DescriptionModel()
                    {
                        Description = SpecTypeId,
                        Details = SpecValue,
                    };

                    descriptionModels.Add(newDescriptionModel);
                }
            }
            string serializeDescription = JsonConvert.SerializeObject(descriptionModels);
            Items? item = await _context.Items
                .Include(x => x.itemType)
                .SingleOrDefaultAsync(x => x.Id.Equals(model.ItemId)
                        && x.itemDescription.Equals(serializeDescription)
                        && x.itemType.SuppliesId.Equals(model.Supply)
                        && x.ItemTypeId.Equals(model.ItemType)
                        && x.itemUnitId.Equals(model.Unit));

            if (item != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> GetSpecs(string ItemId)
        {
            Items? item = await _context.Items
                .SingleOrDefaultAsync(x => x.Id.Equals(ItemId));

            if(item != null)
            {
                if(item.itemDescription != null)
                {
                    List<DescriptionModel> ObjOrderList = JsonConvert.DeserializeObject<List<DescriptionModel>>(item.itemDescription);

                    List<EditItemModel> newEditItem = new List<EditItemModel>();
                    foreach (var obj in ObjOrderList)
                    {
                        ItemSpecType? specType = await _context.ItemSpecType
                            .SingleOrDefaultAsync(x => x.Id.Equals(obj.Description));

                        if (specType != null)
                        {
                            EditItemModel newEdit = new EditItemModel()
                            {
                                DescriptionId = obj.Description,
                                Desc = specType.itemSpecType,
                                Value = obj.Details,
                            };
                            newEditItem.Add(newEdit);
                        }

                    }
                    return Json(newEditItem);
                }
                else
                {
                    return Json(0);
                }
               
            }
            else
            {

                return Json(0);
            }
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
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(returnUrl);
            }
            Items? item = await _context.Items
                .Include(x => x.itemType)
                .SingleOrDefaultAsync(x => x.Id == ItemId);

            _context.Items.Remove(item);
            await _context.SaveChangesAsync(currUser.Id, "Item: Deleted");
            TempData["alert"] = Global.GenerateToast(item.ItemName, "Deleted Successfully", "", Global.BsStatusColor.Danger, Global.BsStatusIcon.Danger);
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
        public async Task<IActionResult> _AddItem(_ItemInputModel model, string returnUrl)
        {
            AccountUser? currentUser = await _userManager.GetUserAsync(User);

            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.Id.Equals(currentUser.Id));

            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(returnUrl);
            }

            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);
           
            Items newItem = new Items()
            {
                Id = Guid.NewGuid().ToString(),
                ItemTypeId = model.ItemType.Trim().ToString(),
                itemUnitId = model.Unit.Trim().ToString(),
                dateCreated = date,
                timeCreated = time,
                dateUpdated = date,
                timeUpdated = time,
                IsEnabled = true,
            };
            List<DescriptionModel> descriptionModels = new List<DescriptionModel>();
            foreach (var specs in model.NewItemSpecs)
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
                    DescriptionModel newDescriptionModel = new DescriptionModel()
                    {
                        Description = SpecTypeId,
                        Details = SpecValue,
                    };

                    descriptionModels.Add(newDescriptionModel);
                }
            }
            string serializeDescription = JsonConvert.SerializeObject(descriptionModels);
            newItem.itemDescription = serializeDescription;

            _context.AddAsync(newItem);
            await _context.SaveChangesAsync(currentUser.Id, "Item : Added a new item");

            Items? item = await _context.Items
                .Include(x => x.itemType)
                .SingleOrDefaultAsync(x => x.Id.Equals(newItem.Id));

            await _context.SaveChanges(currentUser.Id, "Item: Update Changes");
            TempData["alert"] = Global.GenerateToast(item.ItemName, "Added Successfully", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
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
                .OrderBy(x => x.itemType.itemType)
                .ThenBy(x => x.dateCreated)
                .Where(x => x.itemDescription.Contains(searchtxt)
                || x.itemType.itemType.Contains(searchtxt));

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
