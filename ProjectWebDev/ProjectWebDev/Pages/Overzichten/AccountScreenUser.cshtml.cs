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
    
    public string Wachtwoord { get; set; }
    public string Gebruikersnaam { get; set; }
    public IEnumerable<Gebruiker> Gebruikers { get; set; }
    
    public string Gebruiker_ID;
    public string HREF3 { get; set; }
    public string HREF4{ get; set; }
    public string LINKNAAM3 { get; set; }
    public string LINKNAAM4 { get; set; }
    
    public IActionResult OnGet(string warning, string warning2)
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
            return RedirectToPage("/Login/Loginscreen");
        string userrol =
            new GebruikerRepository().GetUserRol(
                Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        
        ButtonNamer namer = new ButtonNamer();
        HREF3 = namer.Button3Href(userrol);
        LINKNAAM3 = namer.Button3Name(userrol);
        HREF4 = namer.Button4Href(userrol);
        LINKNAAM4 = namer.Button4Name(userrol);
        Wachtwoord = warning;
        Gebruikersnaam = warning2;
        Gebruikers = new GebruikerRepository().GetUser(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        
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
        Gebruikersnaam = new GebruikerRepository().UpdateUsername(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), UsernUpdate );

        return RedirectToPage(new{warning = Gebruikersnaam});
    }
    
    public IActionResult OnPostUpdatePassw([FromForm] string PasswordUpd, [FromForm] string PasswordCurrent)
    {
        Wachtwoord = new GebruikerRepository().UpdatePassword(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), PasswordUpd, PasswordCurrent);

        return RedirectToPage(new{warning = Wachtwoord});
    }
}