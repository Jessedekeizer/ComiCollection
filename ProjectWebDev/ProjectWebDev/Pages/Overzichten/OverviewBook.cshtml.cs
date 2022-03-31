using Dapper;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class OverviewBook : PageModel
{
    [BindProperty] 
    public BoekData BoekData { get; set; }
    public IEnumerable<Stripboek> stripboeken { get; set; }
    public IEnumerable<Bijdrager> Auteurs { get; set; }
    public int Strip_id { get; set; } = 0;
    public IEnumerable<Bijdrager> Tekenaars { get; set; }

    public KleurenSchema Kleuren { get; set; }
    
    public void OnGet([FromQuery] int strip_id)
    {
        Strip_id = strip_id;

        stripboeken = new StripboekRepository().Get(strip_id);

        //Hier worden alle auteurs van het boek opgevraagt en die stopt hij in een IEnumerable
        Auteurs = new BijdragerRepository().GetAuteurs(strip_id);

        //Hier worden alle tekenaars van het boek opgevraagt en die stopt hij in een IEnumerable
        Tekenaars = new BijdragerRepository().GetTekenaars(strip_id);
        
        Kleuren = new KleurenSchema();
    }
    
    
}