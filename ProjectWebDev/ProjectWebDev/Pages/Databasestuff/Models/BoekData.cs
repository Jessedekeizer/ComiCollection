using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebDev.Pages.Databasestuff.Models;

public class BoekData
{ 
    //Boek
    public int Strip_id { get; set; }

    [Required, Range(1000000000, 9999999999999)]
    public Int64 Isbn { get; set; }

    [Required, MaxLength(128)]
    [DefaultValue("Geen beschrijving aanwezig")]
    public string Titel { get; set; }

    [Required, Range(1, 9999)] public int Uitgavejaar { get; set; }
    [Required, Range(0, 10000)] public int Blzs { get; set; }
    [Required] public string Reeks { get; set; }
    [Required] public string Uitgeverij { get; set; }

    public bool Nsfw { get; set; }
    
    public bool Isvisible { get; set; }

    //Auteur 1
    public int Id1 { get; set; }
    [Required, MaxLength(128)] public string Naam1 { get; set; }
    [Required] public string Geboortedatum1 { get; set; }
    [Required, MaxLength(1)] public string Sexe1 { get; set; }

    [MaxLength(128)] public string Wikilink1 { get; set; }

    //Auteur 2
    public int Id2 { get; set; }
    public string Naam2 { get; set; }
    public string Geboortedatum2 { get; set; }
    public string Sexe2 { get; set; }
    public string Wikilink2 { get; set; }

    //Tekenaar
    public int TekenaarId { get; set; }
    public string TekenNaam { get; set; }
    public string TekenGeboortedatum { get; set; }
    public string TekenSexe { get; set; }
    public string TekenWikilink { get; set; }
}