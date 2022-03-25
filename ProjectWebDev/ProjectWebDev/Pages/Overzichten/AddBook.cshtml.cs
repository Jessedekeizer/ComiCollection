using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AddBook : PageModel
{
    [BindProperty] public BoekData BoekData { get; set; }

    public void OnGet()
    {
    }

    //Voegt alle waarden van de onpost in de 3 queries om ze toetevoegen in het database.
    public IActionResult OnPost()
    {
        //Voegt alle waarden van een boek en stuurt ze door naar de AddBook methode
        StripboekRepository boek = new StripboekRepository();
        boek.AddBook(BoekData.Titel, BoekData.Isbn, BoekData.Uitgavejaar, BoekData.Blzs, BoekData.Reeks,
            BoekData.Uitgeverij, BoekData.Nsfw);


        BijdragerRepository bijdrager = new BijdragerRepository();
        RolRepository rol = new RolRepository();

        //Voegt alle waarden van een Auteur en stuurt ze door naar de AddBook methode
        bijdrager.AddBijdrager(BoekData.Naam1, BoekData.Geboortedatum1, BoekData.Sexe1, BoekData.Wikilink1);
        rol.AddRol("auteur");

        if (BoekData.Naam2 != null || BoekData.Geboortedatum2 != null || BoekData.Sexe2 != null ||
            BoekData.Wikilink2 != null)
        {
            bijdrager.AddBijdrager(BoekData.Naam2, BoekData.Geboortedatum2, BoekData.Sexe2, BoekData.Wikilink2);
            rol.AddRol("auteur");
        }

        //Voegt alle waarden van een Tekenaar en stuurt ze door naar de AddBook methode
        if (BoekData.TekenNaam != null || BoekData.TekenGeboortedatum != null || BoekData.TekenSexe != null ||
            BoekData.TekenWikilink != null)
        {
            bijdrager.AddBijdrager(BoekData.TekenNaam, BoekData.TekenGeboortedatum, BoekData.TekenSexe,
                BoekData.TekenWikilink);
            rol.AddRol("tekenaar");
        }

        return RedirectToPage();
    }
}