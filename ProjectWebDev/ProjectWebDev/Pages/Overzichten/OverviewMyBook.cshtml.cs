using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class OverviewMyBook : PageModel
{
    [BindProperty] public BoekData BoekData { get; set; }
    public IEnumerable<Stripboek> stripboeken { get; set; }
    public IEnumerable<Bijdrager> Auteurs { get; set; }
    public IEnumerable<Bijdrager> Tekenaars { get; set; }
    
    public KleurenSchema Kleuren { get; set; }
    
    public IActionResult OnGet([FromQuery] int strip_id)
    {
        Kleuren = new KleurenSchema();
        
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
            return RedirectToPage("/Login/LoginScreen");
        
        stripboeken = new StripboekRepository().Get(strip_id);

        //Hier worden alle auteurs van het boek opgevraagt en die stopt hij in een IEnumerable
        Auteurs = new BijdragerRepository().GetAuteurs(strip_id);

        //Hier worden alle tekenaars van het boek opgevraagt en die stopt hij in een IEnumerable
        Tekenaars = new BijdragerRepository().GetTekenaars(strip_id);
        return Page();
    }
    

    //Methode om een boek te verwijderen uit Collection.
    public IActionResult OnPostRemove([FromForm] int Strip_id)
    {
        
        //Als je op de delete knop drukt, wordt Alles van het stripboek verwijdert.
        new BezitRepository().DeleteFromMyCollection(Strip_id, Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
        return RedirectToPage("/Overzichten/MyCollection");
    }
}