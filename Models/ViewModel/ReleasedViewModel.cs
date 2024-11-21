using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class ReleasedViewModel
    {
        public List<_ItemReleasedModel> items { get; set; } = new List<_ItemReleasedModel>();
        public List<Released> releasedItems { get; set; } = new List<Released>();
        public List<Department> departments { get; set; } = new List<Department>();
        public List<string> Months { get; set; } = new List<string>();
        public PaginationModel PaginationModel { get; set; }
        public int year { get; set; }
        public string DeptId { get; set; } = string.Empty;
        public string DeptTxt { get; set; } = string.Empty;
        public string perPage { get; set; } = string.Empty;
        public string searchtxt { get; set; } = string.Empty;
        public string YearFrom { get; set; } = string.Empty;
        public string YearTo { get; set;} = string.Empty;
        public string TotalCount { get; set; } = string.Empty;
        public string toReceiveCount { get; set; } = string.Empty;
        public string pullOutCount { get; set; } = string.Empty;
        public string receivedCount { get; set; } = string.Empty;
        public string year1Text { get; set; } = string.Empty;
        public string year2Text { get; set; } = string.Empty; 
        public string deptText { get; set; } = string.Empty;
    }
}
