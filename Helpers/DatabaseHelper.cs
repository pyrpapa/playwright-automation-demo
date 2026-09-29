using Dapper;
using Microsoft.Data.Sqlite;
using PlaywrightAutomationDemo.Models;
using PlaywrightAutomationDemo.Config;


namespace PlaywrightAutomationDemo.Helpers;

public class DatabaseHelper
{
    private static readonly string ConnectionString = $"Data Source={TestConfig.DbPath}";

    public static void Initialize()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(TestConfig.DbPath)!);
        
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
        //Deletes all rows
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

    public static int CountPosts() =>
    Scalar("SELECT COUNT(*) FROM Posts");

    public static int CountDuplicateIds() =>
        Scalar("SELECT COUNT(*) FROM (SELECT Id FROM Posts GROUP BY Id HAVING COUNT(*) > 1)");

    public static int CountMissingTitles() =>
        Scalar("SELECT COUNT(*) FROM (SELECT Id FROM Posts WHERE Title IS NULL OR Title = '')");


    private class UserPostCount
    {
        public int UserId { get; set; }
        public int PostCount { get; set; }
    }

    public static Dictionary<int, int> GetPostCountsByUser()
    {
        using var connection = new SqliteConnection(ConnectionString);
        return connection
            .Query<UserPostCount>("SELECT UserId, COUNT(*) AS PostCount FROM Posts GROUP BY UserId")
            .ToDictionary(r => r.UserId, r => r.PostCount);
    }

    private static int Scalar(string sql)
    {
        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            return connection.ExecuteScalar<int>(sql);
        }
        catch (SqliteException ex)
        {
            throw new InvalidOperationException($"Query failed: {sql}\n{ex.Message}", ex);
        }
    }
}