using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class StocksViewModel
    {
        public List<Items> WithStockItems { get; set; } = new List<Items>();
        public StockInputModel StockInputModel { get; set; } = new StockInputModel();
        public StockEditModel StockEditModel { get; set; } = new StockEditModel();
        public List<Supplies> supplies { get; set; } = new List<Supplies>();
        public PaginationModel PaginationModel { get; set; }
        public string status { get; set; } = string.Empty;
        public string perPage { get; set; } = string.Empty;
        public string SearchText { get; set; } = string.Empty;
        public string ItemTypeValue { get; set; } = string.Empty;
        public string SupplyTextValue { get; set; } = string.Empty;
        public string ItemTypeText { get; set; } = string.Empty;
        public string SupplyText { get; set; } = string.Empty;
        public string scope { get; set; } = string.Empty;
    }
}
