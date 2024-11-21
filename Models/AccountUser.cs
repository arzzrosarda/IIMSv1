
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace IIMSv1.Models
{
    [PrimaryKey(nameof(Id))]
    public class Employee
    {
        public string Id { get; set; } = string.Empty;

        [PersonalData]
        public string lastName { get; set; } = string.Empty;

        [PersonalData]
        public string firstName { get; set; } = string.Empty;

        [PersonalData]
        public string middleName { get; set; } = string.Empty;

        [PersonalData]
        public string nameExt { get; set; } = string.Empty;

        [PersonalData]
        public string displayName { get; set; } = string.Empty;

        [PersonalData]
        public string Designation { get; set; } = string.Empty;

        public string departmentID { get; set; }

        [ForeignKey(nameof(departmentID))]
        public Department department { get; set; }
        public string fullName_LF
        {
            get
            {
                return lastName + ", " + firstName;
            }
        }
        public string fullName_LFM
        {
            get
            {
                return lastName + ", " + firstName + " " + middleName;
            }
        }
        public DateOnly DateCreated { get; set; }
        public TimeOnly TimeCreated { get; set; }
        public DateOnly DateUpdated { get; set; }
        public TimeOnly TimeUpdated { get; set; }

    }

    [Index(nameof(UserID), IsUnique = true)]
    public class AccountUser : IdentityUser
    {
        public string UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public Employee Employee { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        [DefaultValue(false)]
        public bool IsAdmin { get; set; }
    }

}
