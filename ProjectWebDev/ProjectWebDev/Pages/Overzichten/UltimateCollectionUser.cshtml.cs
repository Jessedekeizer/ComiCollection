using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class UltimateCollectionUser : PageModel
{
    public IEnumerable<Stripboek> Stripboeken { get; set; }
    public IEnumerable<Rol> Rollen { get; set; }
    public IEnumerable<Bijdrager> Bijdragers { get; set; }
    public SiteSettings settings { get; set; }
    public string HREF4 { get; set; }
    public string LINKNAAM4 { get; set; }

    public IActionResult OnGet(string action = "")
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in != null)
        {
            string userrol =
                new GebruikerRepository().GetUserRol(
                    Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));

            if (userrol != "u")
            {
                return RedirectToPage("/Overzichten/UltimateCollection");
            }
            ButtonNamer namer = new ButtonNamer();
            HREF4 = namer.Button4Href(userrol);
            LINKNAAM4 = namer.Button4Name(userrol);
        }
        else
        {
            HREF4 = "/Login/LoginScreen";
            LINKNAAM4 = "Login";
        }
        
        //Maakt nieuw settings object aan om te gebruiken voor de methodes.
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

        //Als orderitem leeg is, sorteerd hij in de database automatish op Titel
        if (settings.orderitem == "")
        {
            settings.orderitem = "Titel";
        }

        //Haalt op hoeveel stripboeken er zijn op een bepaalde search waarde.
        //Als er 21 boeken zijn en je wilt per pagina 10 laten zien, doet hij 21 boeken / per pagina aantal,
        //hier komt 2 uit en daarna + 1 om een extra pagina voor rest waarde.
        settings.Totalpage = ((new StripboekRepository().GetCount(settings.searchitem) / settings.perpage) + 1);


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

        // Hier worden de Ienumerables 'Stripboeken', 'Rollen' en 'Bijdragers' gegenereerd. Die van Stripboeken is afhankelijk van
        // welke pagina je hebt en waar je op gefiltert / gesearcht hebt.
        Stripboeken = new StripboekRepository().CollectionGet((settings.page * settings.perpage - settings.perpage),
            settings.searchitem, settings.orderitem, settings.direction, settings.perpage);
        Rollen = new RolRepository().Get();
        Bijdragers = new BijdragerRepository().Get();

        //Zet de settingsobject weer om in json (soort string) code.
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());
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

    public IActionResult OnPostOrder([FromForm] string order)
    {
        //Vraagt de settings cookie hier op.
        settings = new SiteSettings();
        string json;
        string settingsStr = Request.Cookies["settings"];
        settings = JsonConvert.DeserializeObject<SiteSettings>(settingsStr);

        //Kijkt waar hij op moet orderen, als dat niks (b.v.b. als je de cookie hebt gedelete) dan ordert hij op titel.
        if (order == null)
        {
            settings.orderitem = "Titel";
        }
        else
        {
            settings.orderitem = order;
        }

        //Reset de pagina nummer weer op 1.
        settings.page = 1;

        //Update de settings cookie hier.
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());

        return RedirectToPage();
    }
    
    public IActionResult OnPostDirection([FromForm] string direction)
    {
        //Vraagt de settings cookie hier op.
        settings = new SiteSettings();
        string json;
        string settingsStr = Request.Cookies["settings"];
        settings = JsonConvert.DeserializeObject<SiteSettings>(settingsStr);

        //Kijkt waar hij op moet orderen, als dat niks (b.v.b. als je de cookie hebt gedelete) dan ordert hij op titel.
        if (direction == null)
        {
            settings.direction = "DESC";
        }
        else
        {
            settings.direction = direction;
        }

        //Reset de pagina nummer weer op 1.
        settings.page = 1;

        //Update de settings cookie hier.
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());

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

    public IActionResult OnPostAdd([FromForm] int Strip_id)
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
            return RedirectToPage("/Login/Loginscreen");
        string userrol =
            new GebruikerRepository().GetUserRol(
                Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        if (userrol != "u")
            return RedirectToPage("/Overzichten/UltimateCollection");
        
        new BezitRepository().AddToCollection(Strip_id,
            Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        //Wanneer je op update klikt, geeft hij strip_id mee en word je geredirect naar Updatebook.
        //Je neemt de Strip_id mee om bij UpdateBook te vertellen welke boek je nou updaten wilt.
        
        return RedirectToPage();
    }

    public IActionResult OnPostAddScreen()
    {
        return RedirectToPage("/Overzichten/AddBook");
    }
    
    public IActionResult OnPostCheckBook([FromForm] int Strip_id)
    {
        //Wanneer je op de titel klikt, ga je naar de Overview pagina van het boek.
        return RedirectToPage("/Overzichten/OverViewBook", new {strip_id = Strip_id});
    }
}