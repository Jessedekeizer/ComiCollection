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
    
    public string UpdateNotes(int strip_id, int gebruiker_id, string Notes)
    {
        string sql = @"UPDATE Bezit SET notes = @Notes WHERE Strip_id = @strip_id AND gebruiker_id = @gebruiker_id";

        using var connection = GetConnection();
        string notes = connection.ExecuteScalar<string>(sql, new {strip_id, gebruiker_id, Notes});
        return notes;
    }
}