using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class EditBooks : PageModel
{
    public string HREF3 { get; set; }
    public string HREF4 { get; set; }
    public string LINKNAAM3 { get; set; }
    public string LINKNAAM4 { get; set; }
    
    public IEnumerable<Stripboek> Stripboeken { get; set; }

    public IActionResult OnGet()
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in != null)
        {
            string userrol =
                new GebruikerRepository().GetUserRol(
                    Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
            if (userrol == "u")
                return RedirectToPage("/Overzichten/MyCollection");
            ButtonNamer namer = new ButtonNamer();
            HREF3 = namer.Button3Href(userrol);
            LINKNAAM3 = namer.Button3Name(userrol);
            HREF4 = namer.Button4Href(userrol);
            LINKNAAM4 = namer.Button4Name(userrol);
        }
        else
        {
            return RedirectToPage("/Login/LoginScreen");
        }

        Stripboeken = new StripboekRepository().GetNonVisible();
        return Page();
    }

    public IActionResult OnPostCheckBook(int Strip_id)
    {
        return RedirectToPage("/Overzichten/UpdateBook", new {strip_id = Strip_id });
    }
}