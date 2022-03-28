using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using Dapper;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;

namespace ProjectWebDev.Pages.Databasestuff.Repository;

public class StripboekRepository
{
    private IDbConnection GetConnection()
    {
        return new DbUtils().Connect();
    }


    // THE GET ZONE ALLE GET QUERIES
    public IEnumerable<Stripboek> Get(int strip_Id)
    {
        //Haalt alles op van een bepaald stripboek
        string sql = "SELECT * FROM Stripboek WHERE Strip_id = @strip_Id";

        using var connection = GetConnection();
        var stripboek = connection.Query<Stripboek>(sql, new {strip_Id});
        return stripboek;
    }

    public IEnumerable<Stripboek> GetBySearch(string search)
    {
        string sql = @"SELECT * FROM Stripboek
            WHERE isbn LIKE @search
               OR titel LIKE @search
               OR uitgavejaar LIKE @search
               OR blzs LIKE @search 
               OR reeks LIKE @search
               OR uitgeverij LIKE @search";

        using var connection = GetConnection();
        search = "%" + search + "%";
        var stripboek = connection.Query<Stripboek>(sql, new {search});
        return stripboek;
    }

    public IEnumerable<Stripboek> GetNonVisible()
    {
        //Haalt alles op van Stripboek
        string sql = "SELECT strip_id, titel FROM stripboek WHERE isvisible = false ORDER BY strip_id";

        using var connection = GetConnection();
        var stripboek = connection.Query<Stripboek>(sql);
        return stripboek;
    }


    // S.strip_id, S.isbn, S.titel,S.uitgavejaar, 
    // S.blzs, S.Reeks, S.uitgeverij, AVG(Bt.score) as ratings, S.nsfw, S.isvisible,
    public IEnumerable<Stripboek> CollectionGet(int limiter, string search, string order, string direction, int perpage)
    {
        string Orderresult;
        string Directionresult;
        switch (order)
        {
            case SearchConstant.Search_blzs: Orderresult = "ORDER BY S.blzs"; break;
            case SearchConstant.Search_reeks: Orderresult = "ORDER BY S.reeks"; break;
            case SearchConstant.Search_uitgeverij: Orderresult = "ORDER BY S.uitgeverij"; break;
            case SearchConstant.Search_rating: Orderresult = "ORDER BY Ratings"; break;
            default: Orderresult = "ORDER BY S.titel"; break;
        }
        switch (direction)
        {
            case "ASC": Directionresult = "ASC"; break;
            default: Directionresult = "DESC"; break;
        }
        
        string sql = @"SELECT S.*, AVG(Bt.score) as Ratings
        FROM stripboek S
            
        LEFT JOIN Beoordeelt Bt ON Bt.strip_id = S.strip_id
        INNER JOIN Rol R ON R.strip_id = S.strip_id
        INNER JOIN Bijdrager B ON B.bijdrager_id = R.bijdrager_id

        WHERE Isvisible = true AND 
              R.rol = 'auteur' AND
        (isbn LIKE @search
        OR titel LIKE @search
        OR uitgavejaar LIKE @search
        OR blzs LIKE @search 
        OR reeks LIKE @search
        OR uitgeverij LIKE @search
        OR B.naam LIKE @search)
        GROUP BY S.strip_id " + Orderresult + " " + Directionresult + " LIMIT @limiter, @perpage";

        using var connection = GetConnection();
        search = "%" + search + "%";

        var stripboek = connection.Query<Stripboek>(sql, new {limiter, search, Orderresult, Directionresult, perpage});
        return stripboek;
    }

    public IEnumerable<Stripboek> AdminGet()
    {
        //Haalt alle boeken op die niet visible zijn voor normale gebruikers
        string sql = "SELECT * FROM Stripboek WHERE Isvisible = false ORDER BY Titel";

        using var connection = GetConnection();
        var stripboek = connection.Query<Stripboek>(sql);
        return stripboek;
    }

    public int GetCount(string search)
    {
        string sql = @"SELECT COUNT(Strip_id) FROM Stripboek
        WHERE Isvisible = true AND
        (isbn LIKE @search
        OR titel LIKE @search
        OR uitgavejaar LIKE @search
        OR blzs LIKE @search 
        OR reeks LIKE @search
        OR uitgeverij LIKE @search)";

        using var connection = GetConnection();
        search = "%" + search + "%";
        int amount = connection.ExecuteScalar<int>(sql, new {search});
        return amount;
    }
    
    public void AddBook(String Titel, Int64 ISBN, int Uitgavejaar, int Blzs, string Reeks, string Uitgeverij, bool Nsfw)
    {
        //Voeg een stripboek toe
        string sql = @"
                INSERT INTO stripboek (titel, isbn, uitgavejaar, blzs, reeks, uitgeverij, nsfw) 
                VALUES (@Titel, @ISBN, @Uitgavejaar, @Blzs, @Reeks, @Uitgeverij, @Nsfw)";

        using var connection = GetConnection();
        connection.Query<Stripboek>(sql, new{Titel, ISBN, Uitgavejaar, Blzs, Reeks,Uitgeverij, Nsfw });
    }

    public void Delete(int Strip_id)
    {
        //Verwijdert een bepaald stripboek
        string sql = @"DELETE FROM Stripboek WHERE strip_id = @Strip_id";

        using var connection = GetConnection(); 
        connection.Query(sql, new { Strip_id });
    }

    public void Update(int Strip_id, string Titel, Int64 Isbn, int Uitgavejaar, int Blzs, string Reeks, string Uitgeverij, bool Nsfw, bool Isvisible)
    {
        //Hier kan je de velden van een stripboek aanpassen.
        string sql = @"
                UPDATE Stripboek SET 
                    Isbn = @Isbn,
                    Titel = @Titel,
                    Uitgavejaar = @Uitgavejaar,
                    Blzs = @Blzs,
                    Reeks = @Reeks,
                    Uitgeverij = @Uitgeverij,
                    Nsfw = @Nsfw,
                    Isvisible = @Isvisible
                WHERE Strip_id = @Strip_id;";
            
        using var connection = GetConnection();
        connection.Query<Stripboek>(sql, new{Strip_id, Titel, Isbn, Uitgavejaar, Blzs, Reeks, Uitgeverij, Nsfw, Isvisible});
    }

    public IEnumerable<Stripboek> GetTop10()
    {
        //Haalt de 10 best beoordeelde boeken op
        string sql = @"SELECT S.*, AVG(Bt.score) as Ratings
        FROM stripboek S
            
        LEFT JOIN Beoordeelt Bt ON Bt.strip_id = S.strip_id
        GROUP BY S.strip_id ORDER BY Ratings desc LIMIT 0,10";
    
        
        using var connection = GetConnection();

        var stripboek = connection.Query<Stripboek>(sql);
        return stripboek;
        
    }
    
    public IEnumerable<Stripboek> GetMyCollection(int limiter, string search, string order, string direction, int perpage, int gebruikerid)
    {
        string Orderresult;
        string Directionresult;
        switch (order)
        {
            case SearchConstant.Search_blzs: Orderresult = "ORDER BY S.blzs"; break;
            case SearchConstant.Search_reeks: Orderresult = "ORDER BY S.reeks"; break;
            case SearchConstant.Search_uitgeverij: Orderresult = "ORDER BY S.uitgeverij"; break;
            case SearchConstant.Search_rating: Orderresult = "ORDER BY Ratings"; break;
            default: Orderresult = "ORDER BY S.titel"; break;
        }
        switch (direction)
        {
            case "ASC": Directionresult = "ASC"; break;
            default: Directionresult = "DESC"; break;
        }
        
        string sql = @"SELECT S.*, AVG(Bt.score) as Ratings, Bz.gelezen
        FROM stripboek S
            
        LEFT JOIN Beoordeelt Bt ON Bt.strip_id = S.strip_id
        INNER JOIN Rol R ON R.strip_id = S.strip_id
        INNER JOIN Bijdrager B ON B.bijdrager_id = R.bijdrager_id
        LEFT JOIN Bezit Bz ON Bz.strip_id = S.strip_id
        LEFT JOIN Gebruiker Gb ON Gb.gebruiker_id = Bz.gebruiker_id

        WHERE Isvisible = true AND 
              R.rol = 'auteur' AND 
              Gb.gebruiker_id = @gebruikerid AND
        (isbn LIKE @search
        OR titel LIKE @search
        OR uitgavejaar LIKE @search
        OR blzs LIKE @search 
        OR reeks LIKE @search
        OR uitgeverij LIKE @search
        OR B.naam LIKE @search)
        GROUP BY S.strip_id " + Orderresult + " " + Directionresult + " LIMIT @limiter, @perpage";

        using var connection = GetConnection();
        search = "%" + search + "%";

        var stripboek = connection.Query<Stripboek>(sql, new {limiter, search, Orderresult, Directionresult, perpage, gebruikerid});
        return stripboek;
    }
    public int GetCountMy(string search, int gebruikerid)
    {
        string sql = @"SELECT COUNT(S.Strip_id) FROM Stripboek S
    
        INNER JOIN Bezit Bz ON Bz.strip_id = S.strip_id
        INNER JOIN Gebruiker Gb ON Gb.gebruiker_id = Bz.gebruiker_id

        WHERE Isvisible = false AND
        Gb.gebruiker_id = @gebruikerid AND
        (isbn LIKE @search
        OR titel LIKE @search
        OR uitgavejaar LIKE @search
        OR blzs LIKE @search 
        OR reeks LIKE @search
        OR uitgeverij LIKE @search)";

        using var connection = GetConnection();
        search = "%" + search + "%";
        int amount = connection.ExecuteScalar<int>(sql, new {search, gebruikerid});
        return amount;
    }

    public void UpdateRead(int strip_id, int gebruikerid, string trueorfalse)
    {
        int tf;
        switch (trueorfalse)
        {
            case "true": tf = 1; break;
            default: tf = 0; break;
        }
        
        string sql = @"
                UPDATE Bezit SET 
                    gelezen = @tf
                WHERE Strip_id = @strip_id AND gebruiker_id = @gebruikerid;";
            
        using var connection = GetConnection();
        connection.Query(sql, new{strip_id, gebruikerid, tf});
    }
}