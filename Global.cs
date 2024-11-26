using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Drawing;

namespace IIMSv1
{
    public static class Global
    {
        public static string GenerateFullName(string LastName, string FirstName, string MiddleName, string NameExt, bool FirstNameFirst = false)
        {
            string fullname = "";

            if (!FirstNameFirst)
            {
                fullname += LastName + ", " + FirstName;
                fullname += NameExt != "" ? ", " + NameExt : "";
                fullname += MiddleName != "" ? " " + MiddleName : "";
            }
            else
            {
                fullname += FirstName;
                fullname += MiddleName != "" ? " " + MiddleName : "";
                fullname += " " + LastName;
                fullname += NameExt != "" ? ", " + NameExt : "";
            }

            return fullname;
        }

        /// <summary>
        /// Generate a simplified exception message (will get the Exception message and InnerException message if it exists)
        /// </summary>
        /// <param name="ex">The exception to get the messages from.</param>
        /// <returns></returns>
        public static string processErrorMessage(Exception ex)
        {
            string retstr = "";
            if (ex.InnerException != null)
            {
                retstr = "ERROR: " + ex.Message + "\nInner Exception: " + ex.InnerException.Message;
            }
            else
            {
                retstr = "ERROR: " + ex.Message;
            }

            return retstr;
        }

        public enum BsStatusColor
        {
            Primary,
            Secondary,
            Success,
            Info,
            Warning,
            Danger,
            Dark,
            Light
        }

        public enum BsStatusIcon
        {
            Success,
            Info,
            Warning,
            Danger,
            None
        }

        /// <summary>
        /// Generate an HTML/Bootstrap alert markup.
        /// </summary>
        /// <param name="message">The HTML markup for the message to display.</param>
        /// <param name="color">A bootstrap status color (may change depending on the bootstrap theme in use)</param>
        /// <param name="dismissible">Flag whether the generated alert box will have a close button.</param>
        /// <param name="icon">(optional) The bootstrap icon class to display alongside the message.</param>
        /// <returns></returns>
        public static string GenerateToast(string title, string message, string position, BsStatusColor color, BsStatusIcon Icon)
        {
            string colorclass;
            switch (color)
            {
                case BsStatusColor.Primary: colorclass = "alert-primary"; break;
                case BsStatusColor.Secondary: colorclass = "alert-secondary"; break;
                case BsStatusColor.Success: colorclass = "alert-success"; break;
                case BsStatusColor.Info: colorclass = "alert-info"; break;
                case BsStatusColor.Warning: colorclass = "alert-warning"; break;
                case BsStatusColor.Danger: colorclass = "alert-danger"; break;
                case BsStatusColor.Dark: colorclass = "alert-dark"; break;
                case BsStatusColor.Light: colorclass = "alert-light"; break;
                default: colorclass = ""; break;
            }

            string iconClass;
            switch(Icon)
            {
                case BsStatusIcon.Success: iconClass = "ri-lg ri-check-double-fill"; break;
                case BsStatusIcon.Info: iconClass = "ri-lg ri-error-warning-fill"; break;
                case BsStatusIcon.Warning: iconClass = "ri-lg ri-alert-fill"; break;
                case BsStatusIcon.Danger: iconClass = "ri-lg ri-spam-2-fill"; break;
                case BsStatusIcon.None: iconClass = ""; break;
                default: iconClass = ""; break;
            }

            string alert = "<div class=\"alert " + colorclass + " alert-border-left alert-dismissible fade shadow show\" role=\"alert\">";
             if(iconClass != "")
            {
                alert += "<i class=\"" + iconClass + " me-3 align-middle\"></i>";
            } 
            alert += "<strong>"+ title + "</strong> " + message + "<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>" + "</div>";

            return alert;
        }

        public static int GetPaginationTotalPages(int RecordCount, int RecordsPerPage)
        {
            return (int)Math.Ceiling((decimal)RecordCount/(decimal)RecordsPerPage);
        }

        public static string GetModelStateErrors(ModelStateDictionary modelState)
        {
            string errors = "<ul>";
            foreach (var v in modelState.Values)
            {
                foreach (var err in v.Errors)
                {
                    errors += $"<li>{err.ErrorMessage}</li>";
                }
            }
            errors += "</ul>";

            return errors;
        }

    }
}
