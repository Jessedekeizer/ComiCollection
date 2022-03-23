namespace ProjectWebDev.Pages.Databasestuff.Models;

public class Gebruiker
{
    public int Gebruiker_id { get; set; }
    public string Gebruikers_naam { get; set; }
    public string Email { get; set; }
    public string WachtWoord { get; set; }
    public char Functie { get; set; }
}