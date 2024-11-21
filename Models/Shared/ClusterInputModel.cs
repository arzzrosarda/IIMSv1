using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IIMSv1.Models.Shared
{
    public class ClusterInputModel
    {
        [Required]
        [Display(Name = "Department Cluster")]
        [Remote("CheckClusterName", "Cluster")]
        public string ClusterName { get; set; } = string.Empty;
    }

    public class ClusterEditModel
    {
        [Required]
        [HiddenInput]
        public string ClusterId { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Department Cluster")]
        [Remote("CheckClusterDuplicate", "Cluster", AdditionalFields = "ClusterId")]
        public string ClusterName { get; set; } = string.Empty;
    }
}
