using Microsoft.Data.Sqlite;

public static class Database
{
    private const string ConnectionString = "Data Source=sistema.db";

    public static void Inicializar()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Login TEXT NOT NULL UNIQUE,
                    Senha TEXT NOT NULL
                );";
            command.ExecuteNonQuery();
        }
    }

    public static SqliteConnection GetConnection() => new SqliteConnection(ConnectionString);
}