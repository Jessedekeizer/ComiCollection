using AngleSharp.Text;
using Microsoft.AspNetCore.Mvc;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Databasestuff.Models;

public class RedirectUser
{

    public string Ultimate_collection(int Gebruiker_id)
    {
        string User = new GebruikerRepository().GetUserRol(Gebruiker_id);
        if (User == "o" || User == "a")
            return "/Overzichten/UltimateCollection";
        else
            return "/Overzichten/UltimateCollectionUser";
    }
    
}

