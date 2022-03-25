using System.Data;
using Dapper;
using ProjectWebDev.Pages.Databasestuff.Models;

namespace ProjectWebDev.Pages.Databasestuff.Repository;

public class GebruikerRepository
{
    private IDbConnection GetConnection()
    {
        return new DbUtils().Connect();
    }

    public int Gebruiker_ID (string Username)
    {
        string sql = @"SELECT gebruiker_id FROM gebruiker WHERE gebruikers_naam = @Username";
        using var connection = GetConnection();
        int Gebruiker_ID = connection.ExecuteScalar<int>(sql, new {Username});
        return Gebruiker_ID;
    }
    
    public int checkUsername(string Username)
    {
        string sql = "SELECT COUNT(gebruiker_id) FROM gebruiker WHERE gebruikers_naam = @Username";
            
        using var connection = GetConnection();
        int amount = connection.ExecuteScalar<int>(sql, new{Username});
        return amount;
    }
    public int checkEmail(string Email)
    {
        string sql = "SELECT COUNT(gebruiker_id) FROM gebruiker WHERE email = @Email";
            
        using var connection = GetConnection();
        int amount = connection.ExecuteScalar<int>(sql, new{Email});
        return amount;
    }
    public void AddUser(string Username, string Email, String Password, char Functie)
    {
        string sql = @" INSERT INTO gebruiker (gebruikers_naam, email, wachtwoord, functie)
                        VALUES (@Username, @Email, @Password, @Functie)";
        using var connection = GetConnection();
        connection.Query<Gebruiker>(sql, new{Username, Email, Password, Functie});
    }
    
    public int checkPassword(string Password)
    {
        string sql = "SELECT COUNT(gebruiker_id) FROM gebruiker WHERE wachtwoord = @Password";

        using var connection = GetConnection();
        int amount = connection.ExecuteScalar<int>(sql, new {Password});
        return amount;
    }

    public string GetUserRol(int Gebruiker_id)
    {
        string sql = @"SELECT functie FROM gebruiker WHERE gebruiker_id = @Gebruiker_id";
        using var connection = GetConnection();
        string amount = connection.ExecuteScalar<string>(sql, new {Gebruiker_id});
        return amount;
    }

    public IEnumerable<Gebruiker> GetUser(int Gebruiker_id)
    {
        string sql = @"SELECT * FROM gebruiker WHERE gebruiker_id = @Gebruiker_id";
        using var connection = GetConnection();
        var gebruiker = connection.Query<Gebruiker>(sql, new {Gebruiker_id});
        return gebruiker;
    }
    public IEnumerable<Gebruiker> GetAllUser(int limiter,string search, int perpage)
        {
            string sql = @"SELECT * FROM gebruiker 
            WHERE (functie = 'a' OR functie = 'u')
            AND gebruikers_naam LIKE @search
            ORDER BY functie ASC 
            LIMIT @limiter, @perpage"; 
            
            using var connection = GetConnection();
            
            search = "%" + search + "%";
            var gebruiker = connection.Query<Gebruiker>(sql, new {limiter, search, perpage});
            return gebruiker;
        }
    public int GetCount(string search)
    {
        string sql = @"SELECT COUNT(gebruiker_id) FROM gebruiker
        WHERE (functie = 'a' OR functie = 'u')
        AND gebruikers_naam LIKE @search";

        using var connection = GetConnection();
        int amount = connection.ExecuteScalar<int>(sql, new{search});
        return amount;
    }
    
    public string UpdateEmail(int Gebruiker_id, string EmailUpd)
    {
        string sql = @"UPDATE gebruiker SET email = @EmailUpd WHERE gebruiker_id = @Gebruiker_id";
        using var connection = GetConnection();
        string email = connection.ExecuteScalar<string>(sql, new {Gebruiker_id, EmailUpd});
        return email;
    }
    
    public string UpdateUsername(int Gebruiker_id, string UsernUpdate)
    {
        if (!checkUsername2(UsernUpdate))
        {
            string sql = @"UPDATE gebruiker SET gebruikers_naam = @UsernUpdate WHERE gebruiker_id = @Gebruiker_id";
            using var connection = GetConnection();
            string username = connection.ExecuteScalar<string>(sql, new {Gebruiker_id, UsernUpdate});
            return username;
        }

        return "used";
    }
    
    public string UpdatePassword(int Gebruiker_id, string UsernUpdate, string OldPassword)
    {
        if(checkPassword2(Gebruiker_id, OldPassword))
        {
            return "succes";
        }
        else
        {
            string sql = @"UPDATE gebruiker SET wachtwoord = @UsernUpdate WHERE gebruiker_id = @Gebruiker_id";
            using var connection = GetConnection();
            string password = connection.ExecuteScalar<string>(sql, new {Gebruiker_id, UsernUpdate});
            return password;
        }
        
            
        
    }
    
    public void UpdateFunctie(int gebruikerid, string newfunctie)
    {
        if(checkFunctie(gebruikerid, newfunctie))
        {
            
        }
        else
        {
            //Hier kan je de velden van een stripboek aanpassen.
        string sql = @" UPDATE gebruiker 
                SET functie = @newfunctie
                WHERE gebruiker_id = @gebruikerid;";
            
        using var connection = GetConnection();
        connection.Query<Gebruiker>(sql, new{gebruikerid, newfunctie});
        }
    }
    
    public bool checkPassword2(int Gebruiker_id, string Password)
    {
        string sql = "SELECT COUNT(gebruiker_id) FROM gebruiker WHERE wachtwoord = @Password AND gebruiker_id = @Gebruiker_id";

        using var connection = GetConnection();
        bool amount = connection.ExecuteScalar<bool>(sql, new {Gebruiker_id, Password});
        return amount;
    }
    
    public bool checkUsername2(string Username)
    {
        string sql = "SELECT COUNT(gebruiker_id) FROM gebruiker WHERE gebruikers_naam = @Username";
            
        using var connection = GetConnection();
        bool amount = connection.ExecuteScalar<bool>(sql, new{Username});
        return amount;
    }
    public bool checkFunctie(int gebruikerid, string newfunctie)
    {
        string sql = "SELECT * FROM gebruiker WHERE gebruiker_id = @gebruikerid AND functie = @newfunctie";

        using var connection = GetConnection();
        bool amount = connection.ExecuteScalar<bool>(sql, new {gebruikerid, newfunctie});
        return amount;
    }
}