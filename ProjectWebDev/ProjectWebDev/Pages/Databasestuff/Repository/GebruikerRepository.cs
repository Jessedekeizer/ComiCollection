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
}