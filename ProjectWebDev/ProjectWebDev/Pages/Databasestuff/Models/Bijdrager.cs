namespace ProjectWebDev.Pages.Databasestuff.Models;

public class Bijdrager
{
    public int Bijdrager_id { get; set; }  
    public string Naam { get; set; }
    
    public DateTime Geboortedatum { get; set; }
    public char Sexe { get; set; }
    public string Wikilink { get; set; }
}