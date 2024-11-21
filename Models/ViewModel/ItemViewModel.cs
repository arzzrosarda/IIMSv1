﻿using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class ItemViewModel
    {
        public List<Items> items { get; set; } = new List<Items>();
        public _ItemInputModel ItemInputModel { get; set; } = new _ItemInputModel();
        public EditItemInputModel EditItemInputModel { get; set; } = new EditItemInputModel();
        public PaginationModel PaginationModel { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public string SupplyTextValue { get; set; } = string.Empty;
        public string SupplyText { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string perPage { get; set; } = string.Empty;
        public string dateFrom { get; set; } = string.Empty;
        public string dateTo { get; set; } = string.Empty;
        public string dateStart { get; set; } = string.Empty;
        public string dateEnd { get; set; } = string.Empty;
    }
}