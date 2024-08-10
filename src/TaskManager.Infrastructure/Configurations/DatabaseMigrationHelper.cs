using DbUp;
using DbUp.Helpers;

namespace TaskManager.Infrastructure.Configurations;

public static class DatabaseMigrationHelper
{
    /// <summary>
    /// Execute migration using DbUp library.
    /// </summary>
    /// <param name="connectionString">The connection string of current database.</param>
    public static void ExecuteMigrations(string connectionString)
    {
        var scriptPath = Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}\DatabaseScripts");

        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(scriptPath)
            .JournalToPostgresqlTable("public", "schemaversions")
            .LogToConsole()
            .Build();
        
        EnsureSchemaVersionsTableExists(connectionString);

        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
            throw new ApplicationException("Database migration failed", result.Error);
    }
    
    /// <summary>
    /// Ensure that schema versions table exists.
    /// </summary>
    /// <param name="connectionString">The connection string of current database.</param>
    private static void EnsureSchemaVersionsTableExists(string connectionString)
    {
        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScript("0000-CreateSchemaVersionsTable", @"
                CREATE TABLE IF NOT EXISTS schemaversions (
                    schemaversionsid SERIAL PRIMARY KEY,
                    scriptname VARCHAR(255) NOT NULL,
                    applied TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );")
            .LogToConsole()
            .Build();

        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
            throw new Exception("Failed to ensure schema_versions table exists", result.Error);
    }

    
    /// <summary>
    /// Clean database using DbUp library.
    /// </summary>
    /// <param name="connectionString">The connection string of current database.</param>
    public static void CleanDatabase(string connectionString)
    {
        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScript("CleanDatabase", @"
                DO $$ DECLARE
                    r RECORD;
                BEGIN
                    FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = current_schema()) LOOP
                        EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.tablename) || ' CASCADE';
                    END LOOP;
                END $$;")
            .JournalTo(new NullJournal())
            .LogToConsole()
            .Build();

        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
            throw new ApplicationException("Clean database failed", result.Error);
    }
}