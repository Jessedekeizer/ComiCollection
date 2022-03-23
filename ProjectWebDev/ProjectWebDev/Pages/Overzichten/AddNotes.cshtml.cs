using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AddNotes : PageModel
{
    public IEnumerable<Bezit> Notes { get; set; }
    
    public void OnGet([FromQuery] int strip_id)
    {
        BezitRepository Notes = new BezitRepository();
        Notes.GetNotes(strip_id);
    }

    public IActionResult OnPostToevoegen()
    {
        return RedirectToPage("/Overzichten/MyCollection");
    }
}