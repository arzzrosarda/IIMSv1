using IIMSv1.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IIMSv1.Controllers
{
    public class UserController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public UserController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //CheckPassword 
        [Route("{controller}/{action}")]
        public JsonResult CheckOldPassword(string OldPassword, string UserId)
        {
            AccountUser? user = _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefault(x => x.Id.Equals(UserId));
            var result1 = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, OldPassword);
            if (result1 != PasswordVerificationResult.Success)
            {
                return Json("The current password is wrong");
            }
            else
            {
                return Json(true);
            }
        }

        //ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _ChangePassword(_UserChangePassword model)
        {
            AccountUser? currentUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Index", "Profile", new { Id = model.UserId });
            }
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
                TempData["alert"] = Global.GenerateToast("ERROR", "The system encountered at least 1 error when processing data: Password failed to change", "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = model.UserId });
            }
            user.Employee.DateUpdated = date;
            user.Employee.TimeUpdated = time;

            await _context.SaveChangesAsync(currentUser.Id, "Changed Password");
            //await _signInManager.RefreshSignInAsync(user);
            TempData["alert"] = Global.GenerateToast("PASSWORD", "User Password Successfully Changed", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
            return RedirectToAction("Details", "User", new { Id = model.UserId });
        }

        //ResetCredentials
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _ResetCredentials(_UserUpdateCredentials model)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = model.UserId });
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
                    TempData["alert"] = Global.GenerateToast("ACCOUNT CREDENTIALS", "Successfully Changed", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
                    return RedirectToAction("Details", "User", new { Id = model.UserId });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    string errors = Global.GetModelStateErrors(ModelState);
                    TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                    return RedirectToAction("Details", "User", new { Id = model.UserId });
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = model.UserId });
            }

        }

        //Delete
        [HttpGet]
        [Route("{controller}/{Id}/{Action}")]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _Delete(string UserId)
        {
            AccountUser currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = UserId });
            }
            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.Id.Equals(UserId));

            if (user == null || currUser.Id.Equals(UserId))
                return NotFound();
            var userName = user.Employee.fullName_LFM;
            _context.Employees.Remove(user.Employee);
            await _context.SaveChangesAsync(currUser.Id, "User deleted a user account and it's data");
            TempData["alert"] = Global.GenerateToast(userName, "User Account Successfully Deleted", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Warning);
            return RedirectToAction("Index", "User");
        }

        //Edit 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(_UserEditInputModels model)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = model.Id });
            }
            AccountUser currUser = await _userManager.GetUserAsync(User);

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

            TempData["alert"] = Global.GenerateToast("UPDATE", "Changes Successfully Saved", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
            return RedirectToAction("Details", new { Id = user.Id });
        }

        // Disable
        [HttpGet]
        [Route("{controller}/{Id}/{Action}")]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _Disable(string UserId, string returnUrl)
        {
            AccountUser accountUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = UserId });
            }
            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.Id.Equals(UserId));
            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);
            if (user == null || user.Id.Equals(accountUser.Id))
            {
                return NotFound();
            }
            user.Employee.DateUpdated = date;
            user.Employee.TimeUpdated = time;
            user.IsActive = false;
            await _context.SaveChangesAsync(accountUser.Id, "Disabled User Account");
            TempData["alert"] = Global.GenerateToast(user.Employee.fullName_LFM, "User Account Successfully Disabled", "topRight", Global.BsStatusIcon.Warning, Global.BsStatusColor.Warning);
            return LocalRedirect(returnUrl);
        }
        // Activate
        [HttpGet]
        [Route("{controller}/{Id}/{Action}")]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _Activate(string UserId, string returnUrl)
        {
            AccountUser accountUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = UserId });
            }
            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .SingleOrDefaultAsync(x => x.Id.Equals(UserId));

            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);

            if (user == null || user.Id.Equals(accountUser.Id))
            {
                return NotFound();
            }
            if(user.Employee.department.IsActive == true)
            {
                user.IsActive = true;
                user.Employee.DateUpdated = date;
                user.Employee.TimeUpdated = time;
                await _context.SaveChangesAsync(accountUser.Id, "Enabled User Account");
                TempData["alert"] = Global.GenerateToast(user.Employee.fullName_LFM, "User Account Successfully Enabled", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
                return LocalRedirect(returnUrl);
            }
            else
            {
                TempData["alert"] = Global.GenerateToast("ERROR", "Enable department to enable this user account", "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }
        }

        // Check Username Action
        [Route("{controller}/{action}")]
        public JsonResult checkUserName(string UserName)
        {
            if (!_context.Users.Any(x => x.UserName.Equals(UserName)))
            {
                return Json(true);
            }
            else
            {
                return Json("The username is already in use.");
            }
        }
        // Add User Account Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _AddUser(_UserInputModels model)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Index", "User");
            }
            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);

            AccountUser currentuser = await _userManager.GetUserAsync(User);

            string newuserid = Guid.NewGuid().ToString();
            string newempid = Guid.NewGuid().ToString();

           Employee employee = new Employee()
            {
                Id = newempid,
                lastName = model.lastName.Trim(),
                firstName = model.firstName.Trim(),
                middleName = model.middleName != null ? model.middleName.Trim() : "",
                nameExt = model.nameExt != null ? model.nameExt.Trim() : "",
                departmentID = model.departmentID.ToString(),
                displayName = model.displayName.Trim(),
                Designation = model.Designation.Trim(),
                DateCreated = date,
                DateUpdated = date,
                TimeCreated = time,
                TimeUpdated = time,

            };
            AccountUser newuser = new AccountUser()
            {
                Id = newuserid,
                UserName = model.UserName.Trim(),
                Email = model.Email.Trim(),
                NormalizedEmail = model.Email.Trim().ToUpper(),
                NormalizedUserName = model.UserName.Trim().ToUpper(),
                IsActive = true,
                IsAdmin = false
            };
            

            var result = await _userManager.CreateAsync(newuser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newuser, model.UserRole);
                await _context.Employees.AddAsync(employee);
                newuser = await _context.AccountCredentials.SingleAsync(x => x.Id.Equals(newuserid));
                newuser.UserID = employee.Id;
                await _context.SaveChangesAsync(currentuser.Id, "User added a new user account");
                TempData["alert"] = Global.GenerateToast(employee.fullName_LFM, "User Account Successfully Added", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
                return RedirectToAction("Details", "User", new { Id = newuserid });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Index", "User");
            }

        }
        // Details View Controller
        [HttpGet]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> Details(string Id)
        {
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return RedirectToAction("Details", "User", new { Id = Id });
            }

            AccountUser? accountUser = await _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .SingleOrDefaultAsync(x => x.Id.Equals(Id));

            if (accountUser == null)
            {
                return NotFound();
            }

            List<Department> depts = _context.Departments
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Where(x => x.IsActive.Equals(true))
                .ToList();

            _UserEditInputModels model = new _UserEditInputModels(accountUser);

            ViewBag.User = accountUser;
            ViewBag.Departments = depts;
            ViewBag.alert = TempData["alert"];
            return View(model);
        }
        // Search Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> Search(
            string role,
            string dept,
            string dateFrom,
            string dateTo,
            string status,
            string search,
            int page,
            int entries)
        {
            return RedirectToAction("Index", "User", new { search = search, role = role, dateFrom = dateFrom, dateTo = dateTo, status = status, department = dept, page = page, entries = entries });
        }
        // Index View Controller
        [HttpGet]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> Index(
            string role = "",
            string status = "",
            string department = "",
            string search = "",
            string dateFrom = "",
            string dateTo = "",
            int page = 1,
            int entries = 10)
        {
            AccountUser currentUser = await _userManager.GetUserAsync(User);

            string searchtxt = search.Trim();
            string depttxt = department.Trim();
            string roletxt = role.Trim();
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

            IQueryable<AccountUser> searchUser = _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .ThenInclude(x => x.DepartmentCluster)
                .Where(x => x.Employee.lastName.Contains(searchtxt)
                    || x.Employee.firstName.Contains(searchtxt)
                    || x.Employee.middleName.Contains(searchtxt)
                    || x.Email.Contains(searchtxt));

            IQueryable<AccountUser> DepartmentUser = searchUser
               .Where(x => x.Employee.departmentID.Contains(depttxt));

            List<AccountUser> ContainerUser = new List<AccountUser>();
            if (DateStart != null && DateEnd != null)
            {
                List<AccountUser> LyUser = DepartmentUser
                  .Where(x => x.Employee.DateCreated >= DateStart
                    && x.Employee.DateCreated <= DateEnd)
                  .ToList();
                foreach (var user in LyUser)
                {
                    ContainerUser.Add(user);
                }
            }
            else
            {
                List<AccountUser> LyUser = DepartmentUser
                .ToList();

                foreach (var user in LyUser)
                {
                    ContainerUser.Add(user);
                }
            }

            List<AccountUser> statusUser = new List<AccountUser>();
            if (status == "false")
            {
                List<AccountUser> statUser = ContainerUser
                   .OrderBy(x => x.Employee.fullName_LFM)
                    .ThenBy(x => x.Email)
                    .ThenBy(x => x.Employee.department.NormalizedName)
                  .Where(x => x.IsActive.Equals(false) && !x.Id.Equals(currentUser.Id))
                   .ToList();
                foreach (var user in statUser)
                {
                    statusUser.Add(user);
                }
            }
            else if (status == "true")
            {
                List<AccountUser> statUser = ContainerUser
                   .OrderBy(x => x.Employee.fullName_LFM)
                    .ThenBy(x => x.Email)
                    .ThenBy(x => x.Employee.department.NormalizedName)
                    .Where(x => x.IsActive.Equals(true) && !x.Id.Equals(currentUser.Id))
                   .ToList();
                foreach (var user in statUser)
                {
                    statusUser.Add(user);
                }
            }
            else if (status == "")
            {
                List<AccountUser> statUser = ContainerUser
                    .OrderBy(x => x.Employee.fullName_LFM)
                    .ThenBy(x => x.Email)
                    .ThenBy(x => x.Employee.department.NormalizedName)
                    .Where(x => !x.Id.Equals(currentUser.Id))
                    .ToList();
                foreach (var user in statUser)
                {
                    statusUser.Add(user);
                }
            }

            List<AccountUser> roleUser = new List<AccountUser>();
            if(roletxt != "")
            {
                foreach (var user in statusUser)
                {
                    bool isRightRole = await _userManager.IsInRoleAsync(user, roletxt);

                    if (isRightRole)
                    {
                        roleUser.Add(user);
                    }
                }
            }
            else
            {
                foreach (var user in statusUser)
                {
                    roleUser.Add(user);
                }
            }

            List<Department> depts = _context.Departments
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Where(x => x.IsActive.Equals(true))
                .ToList();

            Department? deptSelected = await _context.Departments
                .SingleOrDefaultAsync(x => x.Id.Equals(depttxt) && x.IsActive.Equals(true));

            int recordsperpage = entries;
            int recordtotal = roleUser.Count();
            int pagetotal = Global.GetPaginationTotalPages(recordtotal, recordsperpage);

            if (page < 1 || (page > pagetotal && recordtotal > 0))
            {
                page = 1;
            }
            List<AccountUser> accountUsers = roleUser
                .Skip(recordsperpage * (page - 1))
                .Take(recordsperpage)
                .ToList();

            UserViewModel model = new UserViewModel();
            model.PaginationModel = new PaginationModel("Index", "User", searchtxt, recordtotal, recordsperpage, page);
            model.accountUsers = accountUsers;
            model.SearchText = searchtxt;
            model.status = status;
            model.perPage = entries.ToString();
            model.roletxt = roletxt;
            model.department = depttxt;
            if (deptSelected != null)
            {

                model.dept = deptSelected.NormalizedName;
            }
            else
            {
                model.dept = "";
            }
            model.userAddInputModel.departments = depts;
            model.dateFrom = DateStart.ToString();
            model.dateTo = DateEnd.ToString();
            model.dateStart = DateFullFrom;
            model.dateEnd = DateFullTo;

            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
