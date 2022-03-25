using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace ProjectWebDev.Pages.Databasestuff
{
    public class DbUtils
    {   
        public IDbConnection Connect()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=Webdev;Uid=root;Pwd=Test@1234!;";
            return new MySqlConnection(connectionString);
        }
    }
}