using AngleSharp.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AccountScreenUser : PageModel
{
    

    public string Gebruiker_ID;
    
    public IActionResult OnGet()
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
           return RedirectToPage("/Login/Loginscreen");
        
        Gebruiker_ID = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        return Page();
        
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove(SessionConstant.Gebruiker_ID);
        return RedirectToPage("/Login/LoginScreen");
    }
}