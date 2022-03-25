using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class UpdateBook : PageModel
{
    [BindProperty] public BoekData BoekData { get; set; }
    public IEnumerable<Stripboek> stripboeken { get; set; }
    public IEnumerable<Rol> Rols { get; set; }
    public IEnumerable<Bijdrager> Auteurs { get; set; }
    public int Strip_id { get; set; } = 0;
    public string idStr { get; set; }
    public IEnumerable<Bijdrager> Tekenaars { get; set; }

    public void OnGet([FromQuery] int strip_id)
    {
        //Verwijderd residu cookies.
        Response.Cookies.Delete("stripid");

        //Roept de cookies op en maakt dan een nieuwe cookie aan met het strip_id

        Response.Cookies.Append("stripid", strip_id.ToString(), new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddDays(30)
        });


        Strip_id = strip_id;

        stripboeken = new StripboekRepository().Get(strip_id);

        //Hier worden alle auteurs van het boek opgevraagt en die stopt hij in een IEnumerable
        Auteurs = new BijdragerRepository().GetAuteurs(strip_id);

        //Hier worden alle tekenaars van het boek opgevraagt en die stopt hij in een IEnumerable
        Tekenaars = new BijdragerRepository().GetTekenaars(strip_id);
    }

    //Hier worden de waardes van de ingevoerde form mee gegeven aan de boek.update.
    public IActionResult OnPostUpdateBook()
    {
        idStr = Request.Cookies["stripid"];
        Strip_id = Int32.Parse(idStr);
        StripboekRepository boek = new StripboekRepository();

        boek.Update(Strip_id, BoekData.Titel, BoekData.Isbn, BoekData.Uitgavejaar, BoekData.Blzs,
            BoekData.Reeks, BoekData.Uitgeverij, BoekData.Nsfw, BoekData.Isvisible);

        return RedirectToPage(new {strip_id = Strip_id});
    }

    //Hier worden de waardes van de ingevoerde form mee gegeven van de auteur aan de bijdrager.UpdateBijdrager.
    
    public IActionResult OnPostUpdateAuteur([FromForm] int Auteur_id,
        [FromForm] string AuteurNaam, [FromForm] string Geboortedatum, [FromForm] string Sexe,
        [FromForm] string WikiLink)
    {
        idStr = Request.Cookies["stripid"];
        Strip_id = Int32.Parse(idStr);

        BijdragerRepository bijdrager = new BijdragerRepository();
        bijdrager.UpdateBijdrager(Auteur_id, AuteurNaam, Geboortedatum, Sexe, WikiLink);
        return RedirectToPage(new {strip_id = Strip_id});
    }
    
    //Hier worden de waardes van de ingevoerde form mee gegeven van de tekenaar aan de bijdrager.UpdateBijdrager.
    public IActionResult OnPostUpdateTekenaar([FromForm] int Tekenaar_id, [FromForm] string TekenaarNaam,
        [FromForm] string TekenGeboortedatum, [FromForm] string TekenSexe, [FromForm] string TekeWikiLink)
    {
        string idStr = Request.Cookies["stripid"];
        Strip_id = Int32.Parse(idStr);
        
        BijdragerRepository bijdrager = new BijdragerRepository();
        bijdrager.UpdateBijdrager(Tekenaar_id, TekenaarNaam, TekenGeboortedatum, TekenSexe, TekeWikiLink);
        return RedirectToPage(new {strip_id = Strip_id });
    } 

    //Methode om een boek te verwijderen.
    public IActionResult OnPostDelete([FromForm] int Strip_id)
    {
        StripboekRepository boek = new StripboekRepository();
        RolRepository rol = new RolRepository();
        BezitRepository bezit = new BezitRepository();
        BeoordeeltRepository Beoordeelt = new BeoordeeltRepository();

        //Als je op de delete knop drukt, wordt Alles van het stripboek verwijdert.
        rol.Delete(Strip_id);
        bezit.Delete(Strip_id);
        Beoordeelt.Delete(Strip_id);
        boek.Delete(Strip_id);
        return RedirectToPage("/Overzichten/UltimateCollection");
    }
}