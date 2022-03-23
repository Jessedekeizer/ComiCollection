using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Pages.Databasestuff;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Login;

public class RegisterScreen : PageModel
{
    [BindProperty] 
    public Register Register { get; set; }

    public string Warning { get; set; }

    public void OnGet([FromQuery] int warning)
    {
        switch (warning)
        {
            case 1:
                Warning = "This username is already taken.";
                break;
            case 2:
                Warning = "This email is already in use.";
                break;
        }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        int chkUser = new GebruikerRepository().checkUsername(Register.Username);
        int chkEmail = new GebruikerRepository().checkEmail(Register.Email);
        if ((chkUser == 0)&&(chkEmail == 0))
        {
            new GebruikerRepository().AddUser(Register.Username, Register.Email, Register.Password, 'u');
            return RedirectToPage();
        } 
        else if (chkUser >= 1)
        {
            return RedirectToPage(new {warning = 1});
        }
        else if (chkEmail >= 1)
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

    
}

public class Register
    {
        [Required]
        [Display(Name ="User Name")]
        public string Username { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }