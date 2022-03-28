using System.Collections.Generic;
using System.Data;
using Dapper;
using ProjectWebDev.Helpers;
using ProjectWebDev.Pages.Databasestuff.Models;

namespace ProjectWebDev.Pages.Databasestuff.Repository;

public class RolRepository
{
    private IDbConnection GetConnection()
    {
        return new DbUtils().Connect();
    }
    
    public IEnumerable<Rol> Get()
    {
        //Haalt alles op van de tabel Rol (Bijdrager_id, strip_id en rol)
        string sql = "SELECT * FROM Rol";

        using var connection = GetConnection();
        var rol = connection.Query<Rol>(sql);
        return rol;
    }

    public void AddRol(string Rol)
    {
        //Voegt een rol toe aan de database. Bijdrager_id en strip_id pakt hij uit een andere tabel
        string sql = @"
                
                INSERT INTO rol(bijdrager_id, strip_id, rol) 
                VALUES ((SELECT bijdrager_id
                    FROM bijdrager WHERE bijdrager_id = LAST_INSERT_ID()),
                        (SELECT MAX(strip_id) FROM stripboek), @Rol)";
        using var connection = GetConnection();
        connection.Query<Bijdrager>(sql, new{Rol});
    }

    public void UpdateRol(int Strip_id, int bijdrager_id, string Rol)
    {
        //Hier kan je de rol van iemand aanpassen
        string sql = @"
                UPDATE rol SET 
                    Rol = @Rol
                WHERE Strip_id = @Strip_id AND bijdrager_id = @bijdrager_id";

        using var connection = GetConnection();
        connection.QuerySingle<Stripboek>(sql, new{Strip_id, bijdrager_id, Rol});
    }

    public void Delete(int strip_id)
    {
        //Verwijdert de rol van een bepaalde strip
        string sql = @"DELETE FROM Rol WHERE Strip_id = @strip_id";

        using var connection = GetConnection();
        connection.Query(sql, new {strip_id});
    }
    
}