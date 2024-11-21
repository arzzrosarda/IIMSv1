using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace IIMSv1.Models
{
    public class Released
    {
        public string Id { get; set; } = string.Empty;
        public string ReleaseNumber {  get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        [ForeignKey(nameof(ItemId))]
        public Items item { get; set; }
        public string DepartmentId { get; set; } = string.Empty;
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
        public int ReleaseQuantity { get; set; }
        public DateOnly DateReleased {  get; set; }
        public TimeOnly TimeReleased { get; set; }
        public bool Received { get; set; }
        public bool Pullout { get; set; }
    }
}
