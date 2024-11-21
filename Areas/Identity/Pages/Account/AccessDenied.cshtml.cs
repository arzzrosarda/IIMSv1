using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IIMSv1.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {   
        
        public async Task<IActionResult> OnGetAsync()
        { 
            return LocalRedirect("~/Identity/Account/Login");
        }
    }
}

