using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IIMSv1.Models.Shared
{
    public class _UserInputModels
    {
        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; } = string.Empty;

        [Display(Name = "Middle Name/Initial")]
        public string? middleName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Name Ext.")]
        public string? nameExt {  get; set; } = string.Empty;

        [Required]
        [Display(Name = "Display/Signatory Name")]
        public string displayName {  get; set; } = string.Empty;
        [Required]
        [Display(Name = "Designation/Position")]
        public string Designation { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Department/Unit")]
        public string departmentID { get; set; } = string.Empty;
        [Required]
        [Display(Name = "User Role")]
        public string UserRole {  get; set; } = string.Empty;

        [Required]
        [Display(Name = "Username")]
        [Remote("CheckUsername", "User")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password is invalid.")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public List<Department> departments { get; set; } = new List<Department>();
    }
    public class _UserEditInputModels
    {
        [HiddenInput]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; } = string.Empty;

        [Display(Name = "Middle Name/Initial")]
        public string? middleName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email Address")]
        public string Email {  get; set; } = string.Empty;  

        [Display(Name = "Name Ext.")]
        public string? nameExt { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Display/Signatory Name")]
        public string displayName { get; set; } = string.Empty;

        [Display(Name = "Department/Unit")]
        public string departmentId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Designation/Position")]
        public string Designation { get; set; } = string.Empty;
        public _UserEditInputModels() { }

        public _UserEditInputModels(AccountUser user)
        {
            this.Id = user.Id;
            this.lastName = user.Employee.lastName;
            this.firstName = user.Employee.firstName;
            this.middleName = user.Employee.middleName;
            this.Email = user.Email;
            this.nameExt = user.Employee.nameExt;
            this.displayName = user.Employee.displayName;
            this.Designation = user.Employee.Designation;
            this.departmentId = user.Employee.departmentID;
        }
    }

    public class _UserUpdateCredentials
    {
        [HiddenInput]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "New Username")]
        [Remote("checkUserName", "User")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password is invalid.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class _UserChangePassword
    {
        [HiddenInput]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [Remote("CheckOldPassword", "User", AdditionalFields = "UserId")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password is invalid.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

    }
}
