namespace IIMSv1.Models.ViewModel
{
    public class PrintReleasedViewModel
    {
        public List<Items> items { get; set; } = new List<Items>();
        public List<Released> Released { get; set; } = new List<Released>();
        public List<Department> departments { get; set; } = new List<Department>();
        public List<string> Months { get; set; } = new List<string>();
        public string year1Text { get; set; } = string.Empty;
        public string year2Text { get; set; } = string.Empty;
        public string relnumText { get; set; } = string.Empty;
        public string TotalCount { get; set; } = string.Empty;
        public string toReceiveCount {  get; set; } = string.Empty;
        public string pullOutCount { get; set; } = string.Empty;
        public string receivedCount { get; set; } = string.Empty;
    }
}
