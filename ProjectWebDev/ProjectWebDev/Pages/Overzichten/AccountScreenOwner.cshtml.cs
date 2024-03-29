﻿using AngleSharp.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AccountScreenOwner : PageModel
{
    public string Gebruiker_ID;
    public int Warning { get; set; }
    public IEnumerable<Gebruiker> Gebruikers { get; set; }
    public IEnumerable<Gebruiker> GebruikersLijst { get; set; }
    public SiteSettings settings { get; set; }
    public string HREF3 { get; set; }
    public string HREF4{ get; set; }
    public string LINKNAAM3 { get; set; }
    public string LINKNAAM4 { get; set; }
    public bool NSFW { get; set; }
    
    public KleurenSchema Kleuren { get; set; }
    
    public IActionResult OnGet(int warning, string action = "")
    {
        Kleuren = new KleurenSchema();
        
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
           return RedirectToPage("/Login/Loginscreen");
        string userrol =
            new GebruikerRepository().GetUserRol(
                Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        if (userrol != "o")
            return RedirectToPage("/Overzichten/HomeScreen");
        NSFW = Request.Cookies["nsfw"].ToBoolean();
        Warning = warning;
        Gebruikers = new GebruikerRepository().GetUser(Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        ButtonNamer namer = new ButtonNamer();
        HREF3 = namer.Button3Href(userrol);
        LINKNAAM3 = namer.Button3Name(userrol);
        HREF4 = namer.Button4Href(userrol);
        LINKNAAM4 = namer.Button4Name(userrol);
        
        settings = new SiteSettings();
        string json;
        //Roept de settings string op voor gebruik.
        string settingsStr = Request.Cookies["settings"];

        //leest welke action word uitgevoerd en zet het om in lowercase voor gemak.
        action = action.ToLower();

        //Als settingsStr == null, dat betekend dat er geen cookie bestaat, dus maakt hij een nieuwe cookie aan voor settings.
        if (settingsStr == null)
        {
            Response.Cookies.Append("settings", settings.ToString(), new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(30)
            });
            
        }

        //Als de cookie wel bestaat dan deserialized de hele cookie van een Json (soort string code) naar een Object. (De klasse SiteSettings)
        else
        {
            settings = JsonConvert.DeserializeObject<SiteSettings>(settingsStr);
        }
        
        settings.Totalpage = (((new GebruikerRepository().GetCount(settings.searchitem) / settings.perpage) + 1));


        //Kijkt welke actie je uit voert, welke knop je drukt en gaat vervolgens naar die pagina.
        switch (action)
        {
            //Zet de current pagina één verder.
            case "nextpage":
                settings.page++;
                break;
            //Zet de current pagina twee verder.
            case "nextnextpage":
                settings.page += 2;
                break;
            //Zet de current pagina één terug.
            case "previouspage":
                settings.page--;
                break;
            //Zet de current pagina twee terug.
            case "previouspreviouspage":
                settings.page -= 2;
                break;
        }

        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());
        
        GebruikersLijst = new GebruikerRepository().GetAllUser((settings.page * settings.perpage - settings.perpage),settings.searchitem,settings.perpage);
        Gebruiker_ID = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        return Page();
    }
    
    public IActionResult OnPostSearch([FromForm] string search)
    {
        //Vraagt de settings cookie hier op.
        settings = new SiteSettings();
        string json;
        string settingsStr = Request.Cookies["settings"];
        settings = JsonConvert.DeserializeObject<SiteSettings>(settingsStr);

        //Als je iets in search balk hebt gevuld, zal hij hier op zoeken,anders is searchitem leeg en searched hij op alles.
        if (search == null)
        {
            settings.searchitem = "";
        }
        else
        {
            settings.searchitem = search;
        }

        //Reset de pagina nummer weer op 1.
        settings.page = 1;

        //Update de settings cookie hier.
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());

        return RedirectToPage();
    }
    
    public IActionResult OnPostPromote([FromForm] int gebruikerid,[FromForm] string functie)
    {
 
        new GebruikerRepository().UpdateFunctie(gebruikerid,functie);
        return RedirectToPage();
    }
    
    public IActionResult OnPostDemote([FromForm] int gebruikerid,[FromForm] string functie)
    {

        new GebruikerRepository().UpdateFunctie(gebruikerid,functie);
        return RedirectToPage();
    }
    
    public IActionResult OnPostPerpage([FromForm] int Perpage)
    {
        //Vraagt de settings cookie hier op.
        settings = new SiteSettings();
        string json;
        string settingsStr = Request.Cookies["settings"];
        settings = JsonConvert.DeserializeObject<SiteSettings>(settingsStr);

        //Kijkt waar hij op moet orderen, als dat niks (b.v.b. als je de cookie hebt gedelete) dan ordert hij op titel.
        if (Perpage == null)
        {
            settings.perpage = PPConstant.PP_1;
        }
        else
        {
            settings.perpage = Perpage;
        }

        //Reset de pagina nummer weer op 1.
        settings.page = 1;

        //Update de settings cookie hier.
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());

        return RedirectToPage();
    }
    
    public IActionResult OnPostDelete()
    {
        //Verwijdert hier de settings cookie.
        Response.Cookies.Delete("settings");

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