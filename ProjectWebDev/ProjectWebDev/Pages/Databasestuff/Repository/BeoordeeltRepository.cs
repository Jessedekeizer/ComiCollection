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
    
    public void Delete(int strip_id)
    {
        //Query voor het verwijderen van een beoordeling
        string sql = @"DELETE FROM Beoordeelt WHERE Strip_id = @strip_id";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id});
    }
}