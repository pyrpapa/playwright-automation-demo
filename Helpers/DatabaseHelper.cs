using Dapper;
using Microsoft.Data.Sqlite;
using PlaywrightAutomationDemo.Models;

namespace PlaywrightAutomationDemo.Helpers;

public class DatabaseHelper
{
    private const string ConnectionString = "Data Source=testdb.sqlite";

    public static void Initialize()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Posts (
                Id INTEGER PRIMARY KEY,
                Title TEXT,
                Body TEXT,
                UserId INTEGER
            )
        ");
    }

    public static void Cleanup()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Execute("DELETE FROM Posts");
    }

    public static void SeedPost(int id, string title, string body, int userId)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Execute(
            "INSERT INTO Posts (Id, Title, Body, UserId) VALUES (@Id, @Title, @Body, @UserId)",
            new { Id = id, Title = title, Body = body, UserId = userId }
        );
    }

    public static Post? GetPost(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        return connection.QueryFirstOrDefault<Post>(
            "SELECT * FROM Posts WHERE Id = @Id", new { Id = id }
        );
    }

    public static void DeletePost(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Execute("DELETE FROM Posts WHERE Id = @Id", new { Id = id });
    }
}