using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using IIMSv1.Data;
using IIMSv1.Models;
using IIMSv1.Models.Shared;
using IIMSv1.Models.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IIMSv1.Controllers
{
    public class ClusterController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;

        public ClusterController(InventoryDbContext context, UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AccountUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Edit Department Cluster
        public async Task<IActionResult> _EditDepartmentCluster(ClusterEditModel model, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(returnUrl);
            }
            DepartmentCluster? departmentCluster = await _context.departmentClusters
                .SingleOrDefaultAsync(x => x.Id.Equals(model.ClusterId));

            if(departmentCluster != null)
            {
                departmentCluster.Name = model.ClusterName.Trim().ToString();
                departmentCluster.NormalizedName = model.ClusterName.Trim().ToUpper();
                await _context.SaveChangesAsync(currUser.Id, "Cluster: user edited department cluster");
            }
            TempData["alert"] = Global.GenerateToast(model.ClusterName, "Changes Saved", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
            return LocalRedirect(returnUrl);
        }

        //Check Department Cluster Exists
        [Route("{controller}/{action}")]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> CheckClusterDuplicate(string ClusterId, string ClusterName)
        {

            DepartmentCluster? deptCluster = await _context.departmentClusters
                .SingleOrDefaultAsync(x => x.Name.Equals(ClusterName)
                && !x.Id.Equals(ClusterId));

            if (deptCluster != null)
            {
                return Json("The department cluster already exists");
            }
            else
            {
                return Json(true);
            }
        }

        //Check Department Cluster Exists
        [Route("{controller}/{action}")]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> CheckClusterName(string ClusterName)
        {

            DepartmentCluster? deptCluster = await _context.departmentClusters
                .SingleOrDefaultAsync(x => x.Name.Equals(ClusterName));

            if (deptCluster != null)
            {
                return Json("The department cluster already exists");
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
        public async Task<IActionResult> _AddDepartmentCluster(ClusterInputModel model, string returnUrl)
        {
            AccountUser? currUser = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                string errors = Global.GetModelStateErrors(ModelState);
                TempData["alert"] = Global.GenerateToast("", "The system encountered at least 1 error when processing data:" + errors, "", Global.BsStatusColor.Danger, Global.BsStatusIcon.None);
                return LocalRedirect(returnUrl);
            }

            DepartmentCluster newdeptcluster = new DepartmentCluster()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.ClusterName.Trim().ToString(),
                NormalizedName = model.ClusterName.Trim().ToUpper(),
            };

            _context.AddAsync(newdeptcluster);
            await _context.SaveChangesAsync(currUser.Id, "Cluser: user added new department cluster");

            TempData["alert"] = Global.GenerateToast(model.ClusterName, "Successfully Saved", "", Global.BsStatusColor.Success, Global.BsStatusIcon.Success);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> FilterCluster(string searchtxt, int page, int entries)
        {
            return RedirectToAction("Index", "Cluster", new { search = searchtxt, page = page, entries = entries});
        }

        [HttpGet]
        [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> Index(string search = "", int page = 1, int entries = 10)
        {
            string searchtxt = search.Trim().ToString();
            IQueryable<DepartmentCluster> deptClusterSearch = _context.departmentClusters
                .Where(x => x.Name.Contains(searchtxt)
                || x.NormalizedName.Contains(searchtxt));

            List<DepartmentCluster> clusters = deptClusterSearch
                .OrderBy(x => x.Name)
                .ToList();
            List<_ClusterModel> getClusters = new List<_ClusterModel>();
            int index = 1;
            foreach (var cluster in clusters)
            {
                _ClusterModel clusterModel = new _ClusterModel()
                {
                    num = index++,
                    Id = cluster.Id,
                    Cluster = cluster.Name,
                };
                getClusters.Add(clusterModel);
            }
            int recordsperpage = entries;
            int recordtotal = getClusters.Count();
            int pagetotal = Global.GetPaginationTotalPages(recordtotal, recordsperpage);

            if (page < 1 || (page > pagetotal && recordtotal > 0))
            {
                page = 1;
            }

            List<_ClusterModel> allCluster = getClusters
                .Skip(recordsperpage * (page - 1))
                .Take(recordsperpage)
                .ToList();

            ClusterViewModel model = new ClusterViewModel();
            model.PaginationModel = new PaginationModel("Index", "Cluster", searchtxt, recordtotal, recordsperpage, page);
            model.clusters = allCluster;
            model.searchtxt = searchtxt;
            model.perPage = entries.ToString();


            ViewBag.alert = TempData["alert"];
            return View(model);
        }
    }
}
