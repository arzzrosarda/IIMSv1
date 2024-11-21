using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IIMSv1.Models.Shared
{
    public class DepartmentInputModel
    {
        
        [Required]
        [Display(Name = "Department Fullname")]
        [Remote("CheckDepartment", "Department", AdditionalFields = "Cluster, DisplayName")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Department Cluster")]
        public string Cluster {  get; set; }
        public List<DepartmentCluster> Clusters { get; set; } = new List<DepartmentCluster>();
    }

    public class DepartmentEditModel
    {
        [Required]
        [HiddenInput]
        public string deptId { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Department Fullname")]
        [Remote("CheckDepartmentDuplicate", "Department", AdditionalFields = "deptId, DisplayName")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Department Name")]
        public string DisplayName { get; set; }
        [Required]
        [Display(Name = "Department Cluster")]
        public string Cluster { get; set; } = string.Empty;
        public List<DepartmentCluster> Clusters { get; set; } = new List<DepartmentCluster>();
    }

}
