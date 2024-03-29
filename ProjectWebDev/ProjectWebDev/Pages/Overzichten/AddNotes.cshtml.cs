﻿using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;
using ProjectWebDev.Pages.Databasestuff.Repository;

namespace ProjectWebDev.Pages.Overzichten;

public class AddNotes : PageModel
{
    public KleurenSchema Kleuren { get; set; }
    public string Notes { get; set; }
    
    public int Strip_id { get; set; }
    
    public IActionResult OnGet([FromQuery] int strip_id)
    {
        string Logged_in = HttpContext.Session.GetString(SessionConstant.Gebruiker_ID);
        if (Logged_in == null)
        {
            return RedirectToPage("/Login/Loginscreen");
        }
        int gebruiker_id = Convert.ToInt32(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID));
        Strip_id = strip_id;
        Kleuren = new KleurenSchema();
        
        Notes = new BezitRepository().GetNotes(strip_id, gebruiker_id);
        return Page();
    }

    public IActionResult OnPostToevoegen([FromForm] int strip_id, string Notities)
    {
        string note = Request.Form["Text1"];
        int gebruiker_id = Convert.ToInt32(HttpContext.Session.GetString(SessionConstant.Gebruiker_ID));
        if (new BezitRepository().UpdateNotes(strip_id, gebruiker_id, note) > 0)
        {
            return RedirectToPage("/Overzichten/MyCollection");
        }
        else
        {
            new BezitRepository().AddNotes(strip_id, gebruiker_id, note);
        }
        
        return RedirectToPage("/Overzichten/MyCollection");
    }
}