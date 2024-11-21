using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using IIMSv1.Data;
using IIMSv1.Migrations;
using IIMSv1.Models;
using IIMSv1.Models.Shared;
using IIMSv1.Models.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IIMSv1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public DepartmentController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        

        //Edit Department 
        public async Task<IActionResult> _EditDepartment(DepartmentEditModel model, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }
            Department? department = await _context.Departments
                .SingleOrDefaultAsync(x => x.Id.Equals(model.deptId));

            if(department != null)
            {
                department.Name = model.DisplayName.Trim().ToString();
                department.NormalizedName = model.Name.Trim().ToString();
                department.DepartmentClusterID = model.Cluster.ToString();
                await _context.SaveChangesAsync(currUser.Id, "Department: user edited department");
            }
            TempData["alert"] = Global.GenerateToast(department.NormalizedName, "Changes Successfully Saved", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
            return LocalRedirect(returnUrl);
        }


        //Delete Dept
        [HttpPost]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _DeleteDept(string DeptId, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }
            Department? department = await _context.Departments
                .SingleOrDefaultAsync(x => x.Id.Equals(DeptId));

            if (department != null)
            {
                _context.Remove(department);
            }
            await _context.SaveChangesAsync(currUser.Id, "Department: Deleted");
            TempData["alert"] = Global.GenerateToast(department.Name, "Deleted Successfully", "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
            return LocalRedirect(returnUrl);
        }

        //Enable Dept
        [HttpPost]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _EnableDept(string DeptId, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }
            Department? department = await _context.Departments
                .SingleOrDefaultAsync(x => x.Id.Equals(DeptId));

            if (department != null)
            {
                List<AccountUser> DeptUser = _context.AccountCredentials
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.department)
                    .Where(x => x.Employee.departmentID.Equals(DeptId))
                    .ToList();

                foreach (var user in DeptUser)
                {
                    user.IsActive = true;
                }
                department.IsActive = true;
                await _context.SaveChangesAsync(currUser.Id, "Department: Enabled");
                TempData["alert"] = Global.GenerateToast(department.Name, "Enabled Successfully", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
            }
            return LocalRedirect(returnUrl);
        }


        //Disable Dept
        [HttpPost]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _DisableDept(string DeptId, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }

            Department? department = await _context.Departments
                .SingleOrDefaultAsync(x => x.Id.Equals(DeptId));

            if (department != null)
            {
                List<AccountUser> DeptUser = _context.AccountCredentials
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.department)
                    .Where(x => x.Employee.departmentID.Equals(DeptId))
                    .ToList();

                foreach (var user in DeptUser)
                {
                    user.IsActive = false;
                }
                department.IsActive = false;
                await _context.SaveChangesAsync(currUser.Id, "Department: Disabled");
                TempData["alert"] = Global.GenerateToast(department.Name, "Disabled Successfully", "topRight", Global.BsStatusIcon.Warning, Global.BsStatusColor.Warning);
            }
            return LocalRedirect(returnUrl);
        }
        //Check Department Exists
        [Route("{controller}/{action}")]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> CheckDepartmentDuplicate(string deptId, string Name, string DisplayName)
        {

            Department? department = await _context.Departments
                .SingleOrDefaultAsync(x => x.Name.Equals(DisplayName) && x.Id != deptId
                || x.NormalizedName.Equals(Name) && x.Id != deptId);

            if (department != null)
            {
                return Json("The department is already exists");
            }
            else
            {
                return Json(true);
            }
        }

        //Check Department Exists
        [Route("{controller}/{action}")]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> CheckDepartment(string Name, string Cluster, string DisplayName)
        {
            Department? department = await _context.Departments
                .SingleOrDefaultAsync(x => x.Name.Equals(DisplayName) 
                || x.NormalizedName.Equals(Name)
                && x.DepartmentClusterID.Equals(Cluster));

            if(department != null)
            {
                return Json("The department is already exists");
            }
            else
            {
                return Json(true);
            }
        }

        //Add Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> _AddDepartment(DepartmentInputModel model, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "topRight", Global.BsStatusIcon.Danger, Global.BsStatusColor.Danger);
                return LocalRedirect(returnUrl);
            }

            Department newdept = new Department()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.DisplayName.Trim().ToString(),
                NormalizedName = model.Name.Trim().ToString(),
                DepartmentClusterID = model.Cluster.Trim().ToString(),
                IsActive = true,

            };

            _context.AddAsync(newdept);
            await _context.SaveChangesAsync(currUser.Id, "Department: Added a new department");
            TempData["alert"] = Global.GenerateToast(model.Name, "Successfully Added Department", "topRight", Global.BsStatusIcon.Success, Global.BsStatusColor.Success);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> FilterDepartment(string search, string status, string cluster, int page, int entries)
        {
            return RedirectToAction("Index", "Department", new { search = search, status = status, cluster = cluster, page = page, entries = entries });
        }

        [HttpGet]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> Index(string search = "", string status = "", string cluster = "", int page = 1, int entries = 10)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            AccountUser? user = await _context.AccountCredentials
                .Include(x => x.Employee)
                .ThenInclude(x => x.department)
                .SingleOrDefaultAsync(x => x.Id.Equals(currUser.Id));

            string clustertxt = cluster.ToString();
            string searchtxt = search.Trim().ToString();
            string statustxt = status.Trim().ToString();
            IQueryable<Department> deptSearch = _context.Departments
                .Include(x => x.DepartmentCluster)
                .Where(x => x.Name.Contains(searchtxt)
                || x.NormalizedName.Contains(searchtxt));

            List<Department> clusterFilter = new List<Department>();
            if(clustertxt != "")
            {
                List<Department> clusterDept = deptSearch
                    .Where(x => x.DepartmentClusterID.Equals(clustertxt))
                    .ToList();

                foreach(var dept in clusterDept)
                {
                    clusterFilter.Add(dept);
                }
            }
            else
            {
                foreach (var dept in deptSearch)
                {
                    clusterFilter.Add(dept);
                }
            }

            List<Department> department = new List<Department>();
            if(statustxt == "true")
            {
                List<Department> statusDept = clusterFilter
                .OrderBy(x => x.Name)
                .ThenBy(x => x.DepartmentCluster.Name)
                .Where(x => x.IsActive.Equals(true))
                .ToList();

                foreach(var dept in statusDept)
                {
                    department.Add(dept);
                }
            }
            else if(statustxt == "false")
            {
                List<Department> statusDept = clusterFilter
                .OrderBy(x => x.Name)
                .ThenBy(x => x.DepartmentCluster.Name)
                .Where(x => x.IsActive.Equals(false))
                .ToList();
                foreach (var dept in statusDept)
                {
                    department.Add(dept);
                }
            }
            else
            {
                List<Department> statusDept = clusterFilter
                .OrderBy(x => x.Name)
                .ThenBy(x => x.DepartmentCluster.Name)
                .ToList();
                foreach (var dept in statusDept)
                {
                    department.Add(dept);
                }
            }
            int recordsperpage = entries;
            int recordtotal = department.Count();
            int pagetotal = Global.GetPaginationTotalPages(recordtotal, recordsperpage);

            if (page < 1 || (page > pagetotal && recordtotal > 0))
            {
                page = 1;
            }
            List<Department> SkipDept = department
                .Skip(recordsperpage * (page - 1))
                .Take(recordsperpage)
                .ToList();

            List<DepartmentCluster> clusters = _context.departmentClusters
                .ToList();

            DepartmentCluster? clusterName = await _context.departmentClusters
                .SingleOrDefaultAsync(x => x.Id.Equals(clustertxt));

            DepartmentViewModel model = new DepartmentViewModel();
            model.PaginationModel = new PaginationModel("Index", "Department", searchtxt, recordtotal, recordsperpage, page);
            model.cluster = clustertxt;
            if(clusterName != null)
            {
                model.clusterName = clusterName.Name;
            }
            else
            {
                model.clusterName = "";
            }
            model.departmentEditModel.Clusters = clusters;
            model.departmentInputModel.Clusters = clusters;
            model.Department = SkipDept;
            model.SearchTxt = searchtxt;
            model.status = status;
            model.perPage = entries.ToString();
            
            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
