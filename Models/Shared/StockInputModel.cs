using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IIMSv1.Models.Shared
{
    public class StockInputModel
    {
        [Required]
        public string returnUrl { get; set; } = string.Empty;
        //ItemId
        [Required]
        [Display(Name = "Item")]
        public string Item { get; set; } = string.Empty;

        //Quantity
        [Required]
        [Display(Name = "Quantity")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(0.01, int.MaxValue, ErrorMessage = "The quantity must be greater than or equal to 1")]
        public int Quantity { get; set; }

        public List<Items> items { get; set; } = new List<Items>();
    }

    public class StockEditModel
    {
        [Required]
        public string returnUrl { get; set; } = string.Empty;
        //ItemId
        [Required]
        public string ItemId { get; set; } = string.Empty;
        //Quantity Before 
        [Required]
        public int QuantityBefore { get; set; }

        //Quantity
        [Required]
        [Display(Name = "Quantity")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int Quantity { get; set; }
    }
}
