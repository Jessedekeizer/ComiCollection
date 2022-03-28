using System.Data;
using Dapper;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;

namespace ProjectWebDev.Pages.Databasestuff.Repository;

public class BezitRepository
{
    private IDbConnection GetConnection()
    {
        return new DbUtils().Connect();
    }

    public int CheckInCollection(int stripid, int gebruikerid)
    {
        string sql = "SELECT COUNT(gebruiker_id) FROM Bezit WHERE gebruiker_id = @gebruikerid AND strip_id = @stripid";
            
        using var connection = GetConnection();
        int amount = connection.ExecuteScalar<int>(sql, new{gebruikerid, stripid});
        return amount;
    }
    
    public void AddToCollection(int strip_id, int gebruikerid)
    {
        if (CheckInCollection(strip_id, gebruikerid) < 1)
        {
            string sql = @"INSERT INTO Bezit(strip_id, gebruiker_id)
            VALUES (@strip_id, @gebruikerid)";

            using var connection = GetConnection();
            connection.Query(sql, new {strip_id, gebruikerid});
        }
    }
    public void DeleteFromMyCollection(int strip_id, int gebruikerid)
    {
        string sql = @"DELETE FROM Bezit WHERE Strip_id = @strip_id AND gebruiker_id = @gebruikerid";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id, gebruikerid});
    }

    public void Delete(int strip_id)
    {
        //Query voor het verwijderen van een bezit(of het gelezen is en de notes)
        string sql = @"DELETE FROM Bezit WHERE Strip_id = @strip_id";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id});
    }

    
    public string GetNotes(int strip_id, int gebruiker_id)
    {
        string sql = @"SELECT notes FROM Bezit WHERE Strip_id = @strip_id AND gebruiker_id = @gebruiker_id";

        using var connection = GetConnection();
       string notes = connection.ExecuteScalar<string>(sql, new {strip_id, gebruiker_id});
       return notes;
    }
    
    public int UpdateNotes(int strip_id, int gebruiker_id, string Notes)
    {
        string sql = @"UPDATE Bezit SET notes = @Notes WHERE Strip_id = @strip_id AND gebruiker_id = @gebruiker_id";

        using var connection = GetConnection();
        int added = connection.Execute(sql, new {strip_id, gebruiker_id, Notes});
        return added;
    }
    
    public void AddNotes(int strip_id, int gebruiker_id, string Notes)
    {
        string sql = @"INSERT INTO Bezit(notes, gebruiker_id, strip_id) VALUES (@Notes, @gebruiker_id, @strip_id)";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id, gebruiker_id, Notes});
        
    }
}