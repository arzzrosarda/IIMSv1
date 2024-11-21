using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace IIMSv1.Models
{
    [PrimaryKey(nameof(Id))]    
    public class ReleaseHistory
    {   
        public string Id {  get; set; } = string.Empty;
        public string UserId {  get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string ReleaseId { get; set; } = string.Empty;
        [ForeignKey(nameof(ReleaseId))]
        public Released released { get; set; } 
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public string Action { get; set; } = string.Empty;

    }
}
