using System.Data;
using Dapper;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;

namespace ProjectWebDev.Pages.Databasestuff.Repository;

public class BeoordeeltRepository
{
    private IDbConnection GetConnection()
    {
        return new DbUtils().Connect();
    }

    public int Get(int strip_id, int gebruiker_id)
    {
        string sql= @"SELECT score FROM beoordeelt WHERE Gebruiker_id = @gebruiker_id AND Strip_id = @strip_id";

        using var connection = GetConnection();
        int cijfer = connection.ExecuteScalar<int>(sql, new {strip_id, gebruiker_id});
        return cijfer;
    }
    
    public void AddRating(int strip_id, int gebruiker_id, int cijfer)
    {
        string sql= @"Insert INTO Beoordeelt (strip_id, gebruiker_id, score) VALUES (@strip_id, @gebruiker_id, @cijfer)";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id, gebruiker_id, cijfer});
    }
    
    public void Delete(int strip_id)
    {
        //Query voor het verwijderen van een beoordeling
        string sql = @"DELETE FROM Beoordeelt WHERE Strip_id = @strip_id";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id});
    }
    
    public void UpdateRating(int strip_id, int gebruiker_id, int cijfer)
    {
        string sql= @"UPDATE Beoordeelt SET score = @cijfer WHERE Gebruiker_id = @gebruiker_id AND Strip_id = @strip_id";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id, gebruiker_id, cijfer});
    }
}