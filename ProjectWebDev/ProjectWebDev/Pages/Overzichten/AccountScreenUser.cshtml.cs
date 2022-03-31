using System.Diagnostics;
using AngleSharp.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AccountScreenUser : PageModel
{

    public int Warning { get; set; }
    public string Gebruikersnaam { get; set; }
    public IEnumerable<Gebruiker> Gebruikers { get; set; }
    
    public string HREF3 { get; set; }
    public string HREF4{ get; set; }
    public string LINKNAAM3 { get; set; }
    public string LINKNAAM4 { get; set; }
    
    public bool NSFW { get; set; }
    
    public KleurenSchema Kleuren { get; set; }
    
    public IActionResult OnGet(int warning)
    {
        Kleuren = new KleurenSchema();
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
        
        Warning = warning;

        NSFW = Request.Cookies["nsfw"].ToBoolean(); 

        Gebruikers = new GebruikerRepository().GetUser(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        return Page();
    }

    public IActionResult OnPostNSFW([FromForm] bool Nsfw)
    {
        Response.Cookies.Delete("nsfw");

        //Roept de cookies op en maakt dan een nieuwe cookie aan met het strip_id

        Response.Cookies.Append("nsfw", Nsfw.ToString(), new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddDays(30)
        });
        return RedirectToPage();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove(SessionConstant.Gebruiker_ID);
        return RedirectToPage("/Login/LoginScreen");
    }

    public IActionResult OnPostUpdateMail([FromForm] string EmailUpd)
    {
        new GebruikerRepository().UpdateEmail(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), EmailUpd);

        return RedirectToPage();
    }
    
    public IActionResult OnPostUpdateUsern([FromForm] string UsernUpdate)
    {
        Warning = new GebruikerRepository().UpdateUsername(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), UsernUpdate );

        return RedirectToPage(new{warning = Warning});
    }
    
    public IActionResult OnPostUpdatePassw([FromForm] string PasswordUpd, [FromForm] string PasswordCurrent)
    {
        GebruikerRepository gebruiker = new GebruikerRepository();
        var hashedPassword = gebruiker.GetPassword(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        
        var passwordVerificationResult = new PasswordHasher<object?>().VerifyHashedPassword(null, hashedPassword, PasswordCurrent);
        switch (passwordVerificationResult)
        {
            case PasswordVerificationResult.Failed:
                Warning = 1;
                break;
    
            case PasswordVerificationResult.Success:
                hashedPassword = new PasswordHasher<object?>().HashPassword(null, PasswordUpd);
                gebruiker.UpdatePassword(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), hashedPassword);
            break;
            case PasswordVerificationResult.SuccessRehashNeeded:
                hashedPassword = new PasswordHasher<object?>().HashPassword(null, PasswordUpd);
                gebruiker.UpdatePassword(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)), hashedPassword);
            break;
        }
        
        return RedirectToPage(new{warning = Warning});
    }
}