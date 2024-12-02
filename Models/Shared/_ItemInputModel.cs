using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IIMSv1.Models.Shared
{
    public class _ItemInputModel
    {   
        [HiddenInput]
        public string Item { get; set; } = string.Empty;

        //Supply Type
        [Required]
        [Display(Name = "Supply Type")]
        public string SupplyType { get; set; } = string.Empty;

        //Item Type
        [Required]
        [Display(Name = "Item Type")]
        public string ItemType { get; set; } = string.Empty;

        //Unit
        [Required]
        [Display(Name = "Item Unit")]
        public string Unit { get; set; } = string.Empty;

        public string[] NewItemSpecs { get; set; }
        public List<ItemSpecType> itemSpecTypes { get; set; } = new List<ItemSpecType>();

        public List<Supplies> supplies { get; set; } = new List<Supplies>();

        public List<ItemUnit> unit { get; set; } = new List<ItemUnit>();
    }
    public class EditItemInputModel
    {
        [Required]
        [HiddenInput]
        public string ItemId { get; set; } = string.Empty;

        [Required]
        [HiddenInput]
        public string Item { get; set; } = string.Empty;

        //Supply Type
        [Required]
        [Display(Name = "Supply Type")]
        public string Supply { get; set; } = string.Empty;
        //Item Type
        [Required]
        [Display(Name = "Item Type")]
        public string ItemType { get; set; } = string.Empty;

        //Unit
        [Required]
        [Display(Name = "Item Unit")]
        public string Unit { get; set; } = string.Empty;

        public string[] NewItemSpecs { get; set; }

        public List<ItemSpecType> itemSpecTypes { get; set; } = new List<ItemSpecType>();

        public List<Supplies> supplies { get; set; } = new List<Supplies>();

        public List<ItemUnit> unit { get; set; } = new List<ItemUnit>();

    }
    public class AddSupplyModel
    {
        [Required]
        [Display(Name = "Supply Name")]
        [Remote("CheckDuplicateSupply", "Item")]
        public string Supply { get; set; } = string.Empty;

    }
    public class AddItemTypeModel
    {
        [Required]
        [HiddenInput]
        public string SupplyId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Item Type")]
        [Remote("CheckDuplicateItemType", "Item", AdditionalFields = "SupplyId")]
        public string ItemType { get; set; } = string.Empty;

    }
    public class AddUnitModel
    {
        [Required]
        [Display(Name = "Unit Name")]
        [Remote("CheckDuplicateUnit", "Item")]
        public string Unit { get; set; } = string.Empty;

    }
    public class AddSpecModel
    {
        [Required]
        [Display(Name = "Specification Type")]
        [Remote("CheckDuplicateSpecType", "Item")]
        public string SpecType { get; set; } = string.Empty;

    }
   
    public class EditTypeModel
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
