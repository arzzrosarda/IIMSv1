using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Data;
using IIMSv1.Migrations;
using IIMSv1.Models;
using IIMSv1.Models.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IIMSv1.Controllers
{
    public class ProfileController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public ProfileController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _ChangePassword(_UserChangePassword model)
        {
            AccountUser? currentUser = await _userManager.GetUserAsync(User);

            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.Id.Equals(model.UserId));
            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);
            if (user == null)
                return NotFound();

            var ChangePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!ChangePasswordResult.Succeeded)
            {
                TempData["alert"] = Global.GenerateToast("ERROR", "The system encountered at least 1 error when processing data: Password failed to change", "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.Danger);
                return RedirectToAction("Index", "Profile");
            }
            user.Employee.DateUpdated = date;
            user.Employee.TimeUpdated = time;
            await _context.SaveChangesAsync(currentUser.Id, "Changed Password");
            //await _signInManager.RefreshSignInAsync(user);
            TempData["alert"] = Global.GenerateToast("PASSWORD", "User Password Successfully Changed", "topRight", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
            return RedirectToAction("Index", "Profile");
        }

        //ResetCredentials
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _ResetCredentials(_UserUpdateCredentials model)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Profile", new { Id = model.UserId });
            }

            AccountUser currUser = await _userManager.GetUserAsync(User);

            AccountUser? accUser = await _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.Id.Equals(model.UserId));

            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);

            if (accUser == null)
                return NotFound();

            accUser.Employee.DateUpdated = date;
            accUser.Employee.TimeUpdated = time;
            accUser.UserName = model.UserName.Trim();
            accUser.NormalizedUserName = model.UserName.Trim().ToUpper();
            await _context.SaveChangesAsync(currUser.Id, "User Credentials Reset");

            var result = await _userManager.RemovePasswordAsync(accUser);
            if (result.Succeeded)
            {
                var result1 = await _userManager.AddPasswordAsync(accUser, model.Password);
                if (result1.Succeeded)
                {
                    TempData["alert"] = Global.GenerateToast("ACCOUNT CREDENTIALS", "Successfully Changed", "topRight", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
                    return LocalRedirect("~/Identity/Account/Logout");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    string errors = Global.GetModelStateErrors(ModelState);
                    TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                    return RedirectToAction("Index", "Profile");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Profile");
            }

        }

        //Edit 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(_UserEditInputModels model)
        {
            AccountUser currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return RedirectToAction("Index", "Profile", new { Id = model.Id });
            }
            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .SingleOrDefaultAsync(x => x.Id == model.Id);
            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);
            if (user == null)
            {
                return NotFound();
            }

            user.Employee.lastName = model.lastName.Trim();
            user.Employee.firstName = model.firstName.Trim();
            user.Employee.middleName = model.middleName != null ? model.middleName.Trim() : "";
            user.Employee.nameExt = model.nameExt != null ? model.nameExt.Trim() : "";
            user.Employee.displayName = model.displayName.Trim();
            user.Email = model.Email.Trim();
            user.NormalizedEmail = model.Email.Trim().ToUpper();
            user.Employee.Designation = model.Designation.Trim();
            user.Employee.departmentID = model.departmentId.Trim();
            user.Employee.DateUpdated = date;
            user.Employee.TimeUpdated = time;

            await _context.SaveChangesAsync(currUser.Id, "Updated user profile");
            TempData["alert"] = Global.GenerateToast("UPDATE", "Changes Successfully Saved", "topRight", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
            return RedirectToAction("Index", "Profile");
        }

        //Profile Page
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);

            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .ThenInclude(x => x.DepartmentCluster)
                .SingleOrDefaultAsync(x => x.Id.Equals(currUser.Id));

            if (user == null)
            { }
                NotFound();

            List<Department> depts = _context.Departments
               .Where(x => x.IsActive)
               .OrderBy(x => x.Name)
               .Where(x => x.IsActive.Equals(true))
               .ToList();

            _UserEditInputModels model = new _UserEditInputModels(user);
            ViewBag.User = user;
            ViewBag.Departments = depts;
            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
