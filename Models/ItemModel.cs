using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IIMSv1.Models
{
    [PrimaryKey(nameof(Id))]
    public class Items
    {
        public string Id { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string ItemTypeId { get; set; } = string.Empty;
        [ForeignKey(nameof(ItemTypeId))]
        public ItemType itemType { get; set; }
        public int? itemQuantity { get; set; }
        public string itemUnitId { get; set; } = string.Empty;
        [ForeignKey(nameof(itemUnitId))]
        public ItemUnit itemUnit { get; set; }
        public DateOnly dateCreated { get; set; }
        public TimeOnly timeCreated { get; set; }
        public DateOnly dateUpdated { get; set; }
        public TimeOnly timeUpdated {  get; set; }
        public bool IsEnabled { get; set; }

    }
    [PrimaryKey(nameof(Id))]
    public class ItemUnit
    {
        public string Id { set; get; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
    [PrimaryKey(nameof(Id))]
    public class ItemType
    {
        public string Id { get; set; } = string.Empty;
        public string itemType { get; set; } = string.Empty;
        public string SuppliesId { get; set; } = string.Empty;

        [ForeignKey(nameof(SuppliesId))]
        public Supplies supplies { get; set; }
        public bool IsEnabled { get; set; }
    }
    [PrimaryKey(nameof(Id))]
    public class Supplies
    {
        public string Id { set; get; } = string.Empty;
        public string supplyName {  get; set; } = string.Empty;
        public bool IsEnabled {  set; get; }
    }
    [PrimaryKey(nameof(Id))]
    public class ItemPrice
    {
        public string Id { get; set; } = string.Empty;
        public string itemId {  get; set; } = string.Empty;
        [ForeignKey(nameof(itemId))]
        public Items items { get; set; }
        public decimal itemEstPrice {  get; set; }
        public string Year { get; set; } = string.Empty;
        public bool IsEnabled { set; get; }
    }
    [PrimaryKey(nameof(Id))]
    public class ItemSpecs
    {
        public string Id { set; get; } = string.Empty;
        public string itemId {  set; get; } = string.Empty;
        [ForeignKey(nameof(itemId))]
        public Items items { get; set; }
        public string SpecValueId {  get; set; } = string.Empty;
        [ForeignKey(nameof(SpecValueId))]
        public ItemSpecValue itemSpecValue { get; set; }

    }
    [PrimaryKey(nameof(Id))]
    public class ItemSpecValue
    {
        public string Id { set; get; } = string.Empty;
        public string SpecTypeId { get; set; } = string.Empty;
        [ForeignKey(nameof(SpecTypeId))]
        public ItemSpecType ItemSpecType { get; set; }
        public string itemSpecValue {  get; set; } = string.Empty;
        public bool IsEnabled { set; get; }

    }
    [PrimaryKey(nameof(Id))]
    public class ItemSpecType
    {
        public string Id { set; get; } = string.Empty;
        public string itemSpecType { set; get; } = string.Empty;
        public bool IsEnabled {  get; set; }
        
    }
}
