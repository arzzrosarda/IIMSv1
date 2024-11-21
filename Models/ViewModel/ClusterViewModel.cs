using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class ClusterViewModel
    {
        public List<_ClusterModel> clusters = new List<_ClusterModel>();
        public ClusterInputModel clusterInput { get; set; } = new ClusterInputModel();
        public ClusterEditModel clusterEdit { get; set; } = new ClusterEditModel();
        public PaginationModel PaginationModel { get; set; }

        public string searchtxt { get; set; } = string.Empty;
        public string perPage { get; set; } = string.Empty;

    }
}
