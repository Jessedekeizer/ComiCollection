using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Dapper;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;

namespace ProjectWebDev.Pages.Databasestuff.Repository;

public class BijdragerRepository
{
    private IDbConnection GetConnection()
    {
        return new DbUtils().Connect();
    }
    
    //Haalt alles op van de tabel bijdrager (Bijdrager_id, Naam, geboortedatum, sexe en wiki-link)
    public IEnumerable<Bijdrager> Get()
    {
        string sql = "SELECT * FROM Bijdrager";

        using var connection = GetConnection();
        var bijdrager = connection.Query<Bijdrager>(sql);
        return bijdrager;
    }

    //Voegt een Bijdrager toe aan de database
    public void AddBijdrager(string Naam, string Geboortedatum, string Sexe, string Wikilink)
    {
        string sql = @"
                INSERT INTO bijdrager (naam, geboortedatum, sexe, wikilink) 
                VALUES (@Naam, @Geboortedatum, @Sexe, @Wikilink)";
        using var connection = GetConnection();
        connection.Query<Bijdrager>(sql, new{Naam, Geboortedatum, Sexe, Wikilink,});
    }
    //Hier worden de gegevens van een bijdrager geupdate met de meegegeven waardes.
    public void UpdateBijdrager(int bijdrager_id, string Naam, string Geboortedatum,string Sexe, string Wikilink)
    {
        //Update de bijdrager. Je kan elk element van de bijdrager aanpassen
        string sql = @"UPDATE bijdrager SET
                     naam = @naam,
                     geboortedatum = @geboortedatum,
                     sexe = @Sexe,
                     wikilink = @wikilink 
                    WHERE bijdrager_id = @bijdrager_id";
        using var connection = GetConnection();
        connection.Query<Bijdrager>(sql, new{bijdrager_id, Naam, Geboortedatum, Sexe, Wikilink});
    }
    //Haalt hier alle auteurs op van een specifiek boek op en stopt hem in een IEnumerable 
    public IEnumerable<Bijdrager> GetAuteurs(int strip_id)
    {
        string sql = @"SELECT B.* FROM bijdrager as B
        INNER JOIN Rol R on R.bijdrager_id = B.bijdrager_id

        WHERE Strip_id = @strip_id AND Rol ='auteur' ";
            
        using var connection = GetConnection();
        var rols = connection.Query<Bijdrager>(sql, new { strip_id });
        return rols;
    }
    //Haalt hier alle tekenaars op van een specifiek boek op en stopt hem in een IEnumerable 
    public IEnumerable<Bijdrager> GetTekenaars(int strip_id)
    {
        string sql = @"SELECT B.* FROM bijdrager as B
        INNER JOIN Rol R on R.bijdrager_id = B.bijdrager_id

        WHERE Strip_id = @strip_id AND Rol ='tekenaar' ";
            
        using var connection = GetConnection();
        var rols = connection.Query<Bijdrager>(sql, new { strip_id });
        return rols;
    }
    
}