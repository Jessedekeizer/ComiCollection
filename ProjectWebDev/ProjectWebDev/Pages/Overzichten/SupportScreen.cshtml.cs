using AngleSharp.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class SupportScreen : PageModel
{
    public string HREF3 { get; set; }
    public string HREF4{ get; set; }
    public string LINKNAAM3 { get; set; }
    public string LINKNAAM4 { get; set; }
    public void OnGet()
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
        
    }
}