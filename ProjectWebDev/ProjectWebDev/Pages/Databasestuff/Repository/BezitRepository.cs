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

    public void GetNotes(int strip_id)
    {
        string sql = @"SELECT notes FROM Bezit WHERE Strip_id = @strip_id";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id});
    }
}