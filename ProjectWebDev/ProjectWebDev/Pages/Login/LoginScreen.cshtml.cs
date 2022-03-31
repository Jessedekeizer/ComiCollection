using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
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
                Warning = "De gebruikersnaam of het wachtwoord is incorrect.";
                break;
            case 2:
                Warning = "Er is iets misgegaan probeer het later opnieuw of neem contact op.";
                break;
        }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        int Gebruiker_id;
        GebruikerRepository gebruiker = new GebruikerRepository();
        if (gebruiker.checkUsername(LoginCredential.Username))
        {
            Gebruiker_id = gebruiker.Gebruiker_ID(LoginCredential.Username);
        }
        else
        {
            return RedirectToPage(new {warning = 1});
        }
        
        var hashedPassword = gebruiker.GetPassword(Gebruiker_id);
        //vergelijkt opgegeven password met het hashed password 
        var passwordVerificationResult = new PasswordHasher<object?>().VerifyHashedPassword(null, hashedPassword, LoginCredential.Password);
        switch (passwordVerificationResult)
        {
            case PasswordVerificationResult.Failed:
                return RedirectToPage(new {warning = 1});
    
            case PasswordVerificationResult.Success:
                
                HttpContext.Session.SetString(SessionConstant.Gebruiker_ID, Gebruiker_id.ToString());
                return RedirectToPage(new RedirectUser().Ultimate_collection(Gebruiker_id));
            //Als de hash niet veilig is, maar je kan wel inloggen
            case PasswordVerificationResult.SuccessRehashNeeded:
                HttpContext.Session.SetString(SessionConstant.Gebruiker_ID, Gebruiker_id.ToString());
                return RedirectToPage(new RedirectUser().Ultimate_collection(Gebruiker_id));
        }
        //als geen van de cases wordt uitgevoerd
        return RedirectToPage(new {warning = 2});
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
