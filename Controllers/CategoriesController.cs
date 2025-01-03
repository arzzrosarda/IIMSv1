﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.Shared;
using IIMSv1.Models.ViewModel;

namespace IIMSv1.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public CategoriesController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

        //Edit Type 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _EditType(EditTypeInputModel model)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Categories");
            }

            switch (model.actionSel)
            {
                case "Supply Type":
                    Supplies? supplies = await _context.Supplies
                        .SingleOrDefaultAsync(x => x.Id != model.typeId && x.supplyName.Equals(model.type));

                    if (supplies != null)
                    {
                        TempData["alert"] = Global.GenerateToast(model.type, "already exists", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        Supplies? nochanges = await _context.Supplies
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.supplyName.Equals(model.type));

                        if (nochanges != null)
                        {
                            TempData["alert"] = Global.GenerateToast("NO CHANGES", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                            return RedirectToAction("Index", "Categories");
                        }
                        else
                        {
                            Supplies? getType = await _context.Supplies
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if (getType != null)
                            {
                                getType.supplyName = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                TempData["alert"] = Global.GenerateToast(model.type, "Successfully saved", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                            else
                            {
                                TempData["alert"] = Global.GenerateToast("No record found", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                        }

                    }
                case "Item Type":
                    ItemType? itemType = await _context.ItemType
                        .SingleOrDefaultAsync(x => x.SuppliesId != model.supplyId && x.itemType.Equals(model.type));

                    if (itemType != null)
                    {
                        TempData["alert"] = Global.GenerateToast(model.type, "already exists", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        ItemType? nochanges = await _context.ItemType
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.SuppliesId.Equals(model.supplyId) && x.itemType.Equals(model.type));

                        if (nochanges != null)
                        {
                            TempData["alert"] = Global.GenerateToast("NO CHANGES", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                            return RedirectToAction("Index", "Categories");
                        }
                        else
                        {
                            ItemType? getType = await _context.ItemType
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if (getType != null)
                            {
                                getType.itemType = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                TempData["alert"] = Global.GenerateToast(model.type, "Successfully saved", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                            else
                            {
                                TempData["alert"] = Global.GenerateToast("No record found", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                        }
                    }
                case "Unit Type":
                    ItemUnit? Unit = await _context.ItemUnits
                        .SingleOrDefaultAsync(x => x.Id != model.typeId && x.UnitName.Equals(model.type));

                    if (Unit != null)
                    {
                        TempData["alert"] = Global.GenerateToast(model.type, "already exists", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        ItemUnit? nochanges = await _context.ItemUnits
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.UnitName.Equals(model.type));

                        if (nochanges != null)
                        {
                            TempData["alert"] = Global.GenerateToast("NO CHANGES", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                            return RedirectToAction("Index", "Categories");
                        }
                        else
                        {
                            ItemUnit? getType = await _context.ItemUnits
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if (getType != null)
                            {
                                getType.UnitName = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                TempData["alert"] = Global.GenerateToast(model.type, "Successfully saved", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                            else
                            {
                                TempData["alert"] = Global.GenerateToast("No record found", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                        }
                    }
                case "Specification Type":
                    ItemSpecType? SpecType = await _context.ItemSpecType
                        .SingleOrDefaultAsync(x => x.Id != model.typeId && x.itemSpecType.Equals(model.type));

                    if (SpecType != null)
                    {
                        TempData["alert"] = Global.GenerateToast(model.type, "already exists", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        ItemSpecType? nochanges = await _context.ItemSpecType
                        .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId) && x.itemSpecType.Equals(model.type));

                        if (nochanges != null)
                        {
                            TempData["alert"] = Global.GenerateToast("NO CHANGES", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                            return RedirectToAction("Index", "Categories");
                        }
                        else
                        {
                            ItemSpecType? getType = await _context.ItemSpecType
                                .SingleOrDefaultAsync(x => x.Id.Equals(model.typeId));

                            if (getType != null)
                            {
                                getType.itemSpecType = model.type.Trim().ToString();

                                await _context.SaveChangesAsync(currUser.Id, "Edited Type");

                                TempData["alert"] = Global.GenerateToast(model.type, "Successfully saved", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                            else
                            {
                                TempData["alert"] = Global.GenerateToast("No record found", "", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                                return RedirectToAction("Index", "Categories");
                            }
                        }
                    }
                default:
                    TempData["alert"] = Global.GenerateToast("ERROR", "Something went wrong", "", Global.BsStatusColor.Info, Global.BsStatusIcon.Info);
                    return RedirectToAction("Index", "Categories");

            }
        }


        //Enable Type 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _EnableType(string TypeId)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            ItemSpecType? itemSpecType = await _context.ItemSpecType
                .SingleOrDefaultAsync(x => x.Id.Equals(TypeId));

            ItemUnit? unit = await _context.ItemUnits
                .SingleOrDefaultAsync(x => x.Id.Equals(TypeId));

            Supplies? supplies = await _context.Supplies
                .SingleOrDefaultAsync(x => x.Id.Equals(TypeId));

            if (itemSpecType != null)
            {
                itemSpecType.IsEnabled = true;
                await _context.SaveChangesAsync(currUser.Id, "Categories Enable: Specification Type");
                TempData["alert"] = Global.GenerateToast(itemSpecType.itemSpecType, "Enabled Successfully", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                return RedirectToAction("Index", "Categories");
            }
            else if (unit != null)
            {
                unit.IsEnabled = true;
                await _context.SaveChangesAsync(currUser.Id, "Categories Enable: Unit Type");

                TempData["alert"] = Global.GenerateToast(unit.UnitName, "Enabled Successfully", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                return RedirectToAction("Index", "Categories");
            }
            else if (supplies != null)
            {
                supplies.IsEnabled = true;
                await _context.SaveChangesAsync(currUser.Id, "Categories Enable: Supply Type");
                TempData["alert"] = Global.GenerateToast(supplies.supplyName, "Enabled Successfully", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                return RedirectToAction("Index", "Categories");
            }
            else
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Categories");
            }
        }

        //Disable Type 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _DisableType(string TypeId)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            ItemSpecType? itemSpecType = await _context.ItemSpecType
                .SingleOrDefaultAsync(x => x.Id.Equals(TypeId));

            ItemUnit? unit = await _context.ItemUnits
                .SingleOrDefaultAsync(x => x.Id.Equals(TypeId));

            Supplies? supplies = await _context.Supplies
                .SingleOrDefaultAsync(x => x.Id.Equals(TypeId));

            if (itemSpecType != null)
            {
                itemSpecType.IsEnabled = false;
                await _context.SaveChangesAsync(currUser.Id, "Categories Disabled: Specification Type");
                TempData["alert"] = Global.GenerateToast(itemSpecType.itemSpecType, "Disabled Successfully", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                return RedirectToAction("Index", "Categories");
            }
            else if (unit != null)
            {
                unit.IsEnabled = false;
                await _context.SaveChangesAsync(currUser.Id, "Categories Disabled: Unit Type");
                TempData["alert"] = Global.GenerateToast(unit.UnitName, "Disabled Successfully", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                return RedirectToAction("Index", "Categories");
            }
            else if (supplies != null)
            {
                supplies.IsEnabled = false;
                await _context.SaveChangesAsync(currUser.Id, "Categories Disabled: Supply Type");
                TempData["alert"] = Global.GenerateToast(supplies.supplyName, "Disabled Successfully", "", Global.BsStatusColor.Warning, Global.BsStatusIcon.Warning);
                return RedirectToAction("Index", "Categories");
            }
            else
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Categories");
            }
        }

        //Add SpecsType
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddSpecsType(AddSpecinputModel model)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Categories");
            }
            else
            {
                AccountUser? currentUser = await _userManager.GetUserAsync(User);

                AccountUser? user = await _context.AccountCredentials
                    .Include(x => x.Employee)
                    .SingleOrDefaultAsync(x => x.Id.Equals(currentUser.Id));

                ItemSpecType itemSpecType = new ItemSpecType()
                {
                    Id = Guid.NewGuid().ToString(),
                    itemSpecType = model.SpecType,
                    IsEnabled = true,
                };

                _context.AddAsync(itemSpecType);

                await _context.SaveChangesAsync(currentUser.Id, "Categories Added: Specification Type");
                TempData["alert"] = Global.GenerateToast(model.SpecType, "Successfully added in specification type", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                return RedirectToAction("Index", "Categories");
            }

        }
        //Add Unit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddUnit(AddUnitInputModel model)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Categories");
            }
            else
            {
                AccountUser? currentUser = await _userManager.GetUserAsync(User);

                AccountUser? user = await _context.AccountCredentials
                    .Include(x => x.Employee)
                    .SingleOrDefaultAsync(x => x.Id.Equals(currentUser.Id));

                ItemUnit newItemUnit = new ItemUnit()
                {
                    Id = Guid.NewGuid().ToString(),
                    UnitName = model.Unit.Trim(),
                    IsEnabled = true,
                };

                _context.AddAsync(newItemUnit);
                await _context.SaveChangesAsync(currentUser.Id, "Categories Added: Unit Type");
                TempData["alert"] = Global.GenerateToast(model.Unit, "Successfully added in unit type", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                return RedirectToAction("Index", "Categories");
            }
        }

        //Add Supply
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> _AddSupply(AddSupplyInputModel model)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Categories");
            }
            else
            {
                AccountUser? currentUser = await _userManager.GetUserAsync(User);

                AccountUser? user = await _context.AccountCredentials
                    .Include(x => x.Employee)
                    .SingleOrDefaultAsync(x => x.Id.Equals(currentUser.Id));

                Supplies newsupplies = new Supplies()
                {
                    Id = Guid.NewGuid().ToString(),
                    supplyName = model.Supply.Trim(),
                    IsEnabled = true,
                };

                _context.AddAsync(newsupplies);
                await _context.SaveChangesAsync(currentUser.Id, "Categories Added: Supply Type");

                TempData["alert"] = Global.GenerateToast(model.Supply, "Successfully added in supply type", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                return RedirectToAction("Index", "Categories");
            }

        }
        //Index
        [HttpGet]
        [Authorize(Roles = "Inventory Administrator")]
        public async Task<IActionResult> Index()
        {
            AccountUser? currentUser = await _userManager.GetUserAsync(User);

            AccountUser? user = await _context.AccountCredentials
               .Include(x => x.Employee)
               .ThenInclude(x => x.department)
               .ThenInclude(x => x.DepartmentCluster)
               .SingleOrDefaultAsync(x => x.Id.Equals(currentUser.Id));

            List<ItemSpecType> itemSpecTypes = _context.ItemSpecType
            .ToList();

            List<ItemUnit> itemUnits = _context.ItemUnits
                .ToList();

            List<Supplies> supplies = _context.Supplies
                .ToList();
            CategoriesViewModel model = new CategoriesViewModel();
            model.itemSpecTypes = itemSpecTypes;
            model.supplies = supplies;
            model.unit = itemUnits;

            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
