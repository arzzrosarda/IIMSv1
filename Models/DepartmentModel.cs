using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace IIMSv1.Models
{   
    [PrimaryKey(nameof(Id))]
    public class Department
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string DepartmentClusterID { get; set; } = string.Empty;
        [ForeignKey(nameof(DepartmentClusterID))]
        public DepartmentCluster DepartmentCluster { get; set; } 
        public bool IsActive { get; set; }

    }
    [PrimaryKey(nameof(Id))]
    [Index(nameof(NormalizedName), IsUnique = true)]
    public class DepartmentCluster
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;

    }
}
