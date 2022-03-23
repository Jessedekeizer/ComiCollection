namespace ProjectWebDev.Pages.Databasestuff.Models;

public class Stripboek
{
    public int Strip_id { get; set; }
    public Int64 Isbn { get; set; }
    public string Titel { get; set; }
    public int Uitgavejaar { get; set; }
    public int Blzs { get; set; }
    public string Reeks { get; set; }
    public string Uitgeverij { get; set; }

    public decimal Ratings { get; set; } = 0;
    
    public bool Nsfw { get; set; }
    
    public bool Isvisible { get; set; }
}