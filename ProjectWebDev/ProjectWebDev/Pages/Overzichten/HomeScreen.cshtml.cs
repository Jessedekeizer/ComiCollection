using AngleSharp.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class HomeScreen : PageModel
{
    public IEnumerable<Stripboek> Top10 { get; set; }

    
    public void OnGet()
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
            RedirectToPage("/Login/Loginscreen");
        
        Top10 = new StripboekRepository().GetTop10();
    }
}