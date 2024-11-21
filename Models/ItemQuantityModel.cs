using System.ComponentModel.DataAnnotations.Schema;

namespace IIMSv1.Models
{
    public class ItemQuantityHistory
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        [ForeignKey(nameof(ItemId))]
        public Items items { get; set; }
        public int QuantityFrom { get; set; }
        public int QuantityTo { get; set; }
        public string Activity { get; set; } = string.Empty;
        public DateOnly Date {  get; set; }
        public TimeOnly Time { get; set; }
        
    }
}
