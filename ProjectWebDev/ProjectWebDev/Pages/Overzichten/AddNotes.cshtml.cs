using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AddNotes : PageModel
{
    public KleurenSchema Kleuren { get; set; }
    public string Notes { get; set; }
    
    public int Strip_id { get; set; }
    
    public void OnGet([FromQuery] int strip_id)
    {
        int gebruiker_id = Convert.ToInt32(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID));

        Strip_id = strip_id;
        Kleuren = new KleurenSchema();
        
        Notes = new BezitRepository().GetNotes(strip_id, gebruiker_id);
    }

    public IActionResult OnPostToevoegen([FromForm] int strip_id, string Notities)
    {
        string note = Request.Form["Text1"];
        int gebruiker_id = Convert.ToInt32(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID));
        Notes = new BezitRepository().UpdateNotes(1, gebruiker_id, note);
        
        return RedirectToPage("/Overzichten/MyCollection");
    }
}