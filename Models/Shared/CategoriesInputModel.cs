using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IIMSv1.Models.Shared
{
    public class AddSupplyInputModel
    {
        [Required]
        [Display(Name = "Supply Name")]
        [Remote("CheckDuplicateSupply", "Categories")]
        public string Supply { get; set; } = string.Empty;

    }
    public class AddUnitInputModel
    {
        [Required]
        [Display(Name = "Unit Name")]
        [Remote("CheckDuplicateUnit", "Categories")]
        public string Unit { get; set; } = string.Empty;

    }
    public class AddSpecinputModel
    {
        [Required]
        [Display(Name = "Description Type")]
        [Remote("CheckDuplicateSpecType", "Categories")]
        public string SpecType { get; set; } = string.Empty;

    }
}
