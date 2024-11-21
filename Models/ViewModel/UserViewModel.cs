using IIMSv1.Models.Shared;

namespace IIMSv1.Models.ViewModel
{
    public class UserViewModel
    {
        public List<AccountUser> accountUsers { get; set; } = new List<AccountUser>();
        public string SearchText { get; set; } = string.Empty;
        public PaginationModel PaginationModel { get; set; }
        public _UserInputModels userAddInputModel { get; set; } = new _UserInputModels();
        public string dateFrom { get; set; } = string.Empty;
        public string dateTo { get; set; } = string.Empty;
        public string dateStart { get; set; } = string.Empty;
        public string dateEnd { get; set; } = string.Empty;

        public string status { get; set; } = string.Empty;
        public string department { get; set; } = string.Empty;
        public string dept { get; set; } = string.Empty;
        public string perPage {  get; set; } = string.Empty;
        public string roletxt {  get; set; } = string.Empty;
    }
    public class UserDetailsModel
    {
        public AccountUser AccountUser { get; set; }

        public _UserEditInputModels userEditInputModel { get; set; }

        public UserDetailsModel(AccountUser accountUser, _UserEditInputModels userEditInputModel)
        {
            AccountUser = accountUser;
            this.userEditInputModel = userEditInputModel;
        }
    }
}
