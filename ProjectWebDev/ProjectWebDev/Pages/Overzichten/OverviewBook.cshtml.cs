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
    public string HREF3 { get; set; }
    public string HREF4 { get; set; }
    public string LINKNAAM3 { get; set; }
    public string LINKNAAM4 { get; set; }
    public void OnGet([FromQuery] int strip_id)
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in != null)
        {
            string userrol =
                new GebruikerRepository().GetUserRol(
                    Int32.Parse(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID)));
           
            ButtonNamer namer = new ButtonNamer();
            HREF3 = namer.Button3Href(userrol);
            LINKNAAM3 = namer.Button3Name(userrol);
            HREF4 = namer.Button4Href(userrol);
            LINKNAAM4 = namer.Button4Name(userrol);
        }
        else
        {
            HREF3 = "/Overzichten/MyCollection";
            LINKNAAM3 = "My Collection";
            HREF4 = "/Login/LoginScreen";
            LINKNAAM4 = "Login";
        }
        
        Strip_id = strip_id;

        stripboeken = new StripboekRepository().Get(strip_id);

        //Hier worden alle auteurs van het boek opgevraagt en die stopt hij in een IEnumerable
        Auteurs = new BijdragerRepository().GetAuteurs(strip_id);

        //Hier worden alle tekenaars van het boek opgevraagt en die stopt hij in een IEnumerable
        Tekenaars = new BijdragerRepository().GetTekenaars(strip_id);
        
        Kleuren = new KleurenSchema();
    }
    
    
}