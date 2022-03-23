namespace ProjectWebDev.Pages.Databasestuff.Models;

public class SiteSettings
{
    
        public int Totalpage { get; set; } = 0;
        public int page { get; set; } = 1;

        public string searchitem = "";

        public string orderitem = "";

        public string direction { get; set; } = "ASC";

        public int perpage = 10;

}