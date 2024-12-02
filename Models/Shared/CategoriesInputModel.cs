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
        [Display(Name = "Specification Type")]
        [Remote("CheckDuplicateSpecType", "Categories")]
        public string SpecType { get; set; } = string.Empty;

    }
    public class EditTypeInputModel
    {
        [Required]
        [HiddenInput]
        public string actionSel { get; set; } = string.Empty;

        [HiddenInput]
        public string? supplyId { get; set; } = string.Empty;
        [Required]
        [HiddenInput]
        public string typeId { get; set; } = string.Empty;

        [Required]
        [Remote("CheckforDuplicate", "Item", AdditionalFields = "actionSel, typeId, supplyId")]
        public string type { get; set; } = string.Empty;
    }
}
