namespace IIMSv1.Models.ViewModel
{
    public class InventoryViewModel
    {
        public List<Department> departments { get; set; } = new List<Department>();
        public int year { get; set; }
        public int items { get; set; }
        public int InStock { get; set; }
        public int OutofStock { get; set; }
        public int Releases { get; set; }
        public int ToReceive { get; set; }
        public int Received { get; set; }
        public int Pullout { get; set; }
    }
}
