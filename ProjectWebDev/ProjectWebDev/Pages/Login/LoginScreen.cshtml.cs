using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;
using ProjectWebDev.Pages.Overzichten;

namespace ProjectWebDev.Pages.Login;

public class LoginScreen : PageModel
{
    [BindProperty] 
    public LoginCredentials LoginCredential { get; set; }

    public string Warning { get; set; }

    public void OnGet([FromQuery] int warning)
    {
        switch (warning)
        {
            case 1:
                Warning = "This username does not exist.";
                break;
            case 2:
                Warning = "Password is Incorrect.";
                break;
        }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        int chkUser = new GebruikerRepository().checkUsername(LoginCredential.Username);
        int chkPassword = new GebruikerRepository().checkPassword(LoginCredential.Password);
        if ((chkUser == 1)&&(chkPassword == 1))
        {
            bool Logged_in = true;
            int Gebruiker_id = new GebruikerRepository().Gebruiker_ID(LoginCredential.Username);
            
            HttpContext.Session.SetString(SessionConstant.Gebruiker_ID, Gebruiker_id.ToString());

            return RedirectToPage(new RedirectUser().Ultimate_collection(Gebruiker_id));
        } 
        else if (chkUser == 0)
        {
            return RedirectToPage(new {warning = 1});
        }
        else if (chkPassword == 0)
        {
            
            return RedirectToPage(new {warning = 2});
        }
        else
        {
            return RedirectToPage();
        }
    }

    private IDbConnection GetConnection()
    {
        return new DbUtils().Connect();
    }
    

   
    public class LoginCredentials
    {
        [Required]
        [Display(Name ="User Name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
