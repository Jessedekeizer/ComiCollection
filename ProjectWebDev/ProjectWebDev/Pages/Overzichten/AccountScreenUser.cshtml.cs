using AngleSharp.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AccountScreenUser : PageModel
{
    public string Mail { get; set; }
    
    public string Gebruikersnaam { get; set; }
    public IEnumerable<Gebruiker> Gebruikers { get; set; }
    
    public string Gebruiker_ID;
    
    public IActionResult OnGet()
    {
        
        Gebruikers = new GebruikerRepository().GetUser(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        
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

    public IActionResult OnPostUpdateMail([FromForm] string EmailUpd)
    {
        Mail = new GebruikerRepository().UpdateEmail(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), EmailUpd);

        return RedirectToPage();
    }
    
    public IActionResult OnPostUpdateUsern([FromForm] string UsernUpdate)
    {
        Gebruikersnaam = new GebruikerRepository().UpdateUsername(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), UsernUpdate);

        return RedirectToPage();
    }
}