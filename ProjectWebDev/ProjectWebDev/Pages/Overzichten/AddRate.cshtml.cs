using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AddRate : PageModel
{
    public KleurenSchema Kleuren { get; set; }
    public string Notes { get; set; }
    
    public string Titel { get; set; }

    public int Cijfer { get; set; } = 0;
    
    public int Strip_id { get; set; }
    
    public IActionResult OnGet([FromQuery] int strip_id)
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
        {
            return RedirectToPage("/Login/Loginscreen");
        }

        Titel = new StripboekRepository().Titel(strip_id);
        int gebruiker_id = Convert.ToInt32(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID));
        Strip_id = strip_id;
        Kleuren = new KleurenSchema();
        Cijfer = new BeoordeeltRepository().Get(strip_id, gebruiker_id);
        Notes = new BezitRepository().GetNotes(strip_id, gebruiker_id);
        return Page();
    }

    public IActionResult OnPostToevoegen([FromForm] int strip_id,[FromForm] int cijfer)
    {
        int gebruiker_id = Convert.ToInt32(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID));
        BeoordeeltRepository Beoordeelt = new BeoordeeltRepository();
        if (Beoordeelt.Get(strip_id, gebruiker_id) == 0)
        {
            Beoordeelt.AddRating(strip_id, gebruiker_id, cijfer);
        }
        else
        {
            Beoordeelt.UpdateRating(strip_id, gebruiker_id, cijfer);
        }
        
    
        
        
        return RedirectToPage("/Overzichten/MyCollection");
    }
}