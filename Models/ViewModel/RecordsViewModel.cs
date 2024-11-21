using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class RecordsViewModel
    {

        public List<_RecordInputModel> releases { get; set; } = new List<_RecordInputModel>();
        public List<_RecordInputModel> record { get; set; } = new List<_RecordInputModel>();
        public List<string> year { get; set; } = new List<string>();
        public PaginationModel PaginationModel { get; set; }
        public int totalQuantity { get; set; }
        public string yeartxt { get; set; } = string.Empty;
        public string perPage {  get; set; } = string.Empty;
        public string relnum {  get; set; } = string.Empty;
        public string depttxt {  get; set; } = string.Empty;
        public string dateReleased { get; set; } = string.Empty;
        public string relnumtext { get; set; } = string.Empty;
        public string ToReceive { get; set; } = string.Empty;
        public string Pullout {  get; set; } = string.Empty;
        public string Received {  get; set; } = string.Empty;

    }
}
