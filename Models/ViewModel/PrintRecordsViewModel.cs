using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class PrintRecordsViewModel
    {
        public List<_RecordInputModel> ToReceiveRecords { get; set; } = new List<_RecordInputModel>();
        public List<_RecordInputModel> ReceivedRecords { get; set; } = new List<_RecordInputModel>();
        public List<_RecordInputModel> PullOutRecords { get; set; } = new List<_RecordInputModel>();
        public List<_RecordInputModel> record { get; set; } = new List<_RecordInputModel>();
        public int totalQuantity { get; set; }
        public string depttxt { get; set; } = string.Empty;
        public string dateReleased { get; set; } = string.Empty;
        public string relnumtext { get; set; } = string.Empty;

    }
}
