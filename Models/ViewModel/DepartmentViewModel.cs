using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class DepartmentViewModel
    {
        public List<Department> Department = new List<Department>();
        public DepartmentInputModel departmentInputModel { get; set; } = new DepartmentInputModel();
        public DepartmentEditModel departmentEditModel { get; set; } = new DepartmentEditModel();
        public PaginationModel PaginationModel { get; set; }
        public string perPage {  get; set; }
        public string SearchTxt { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string cluster { get; set; } = string.Empty;
        public string clusterName { get; set; } = string.Empty;
    }

}
