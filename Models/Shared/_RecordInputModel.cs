namespace IIMSv1.Models.Shared
{
    public class _RecordInputModel
    {
        public int num {  get; set; }
        public string Id { get; set; } = string.Empty;
        public string Item {  get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Date {  get; set; } = string.Empty;
        public bool IsReceived { get; set; }
        public bool IsPullout { get; set; }
    }
}
