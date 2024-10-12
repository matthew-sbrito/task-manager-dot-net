using DbUp;
using DbUp.Engine;
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
        var scriptPath = Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}\DatabaseScripts\DLL");
        
        EnsureDatabase.For.PostgresqlDatabase(connectionString);
        EnsureSchemaVersionsTableExists(connectionString);
        
        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(scriptPath)
            .JournalToPostgresqlTable("public", "schemaversions")
            .LogToConsole()
            .Build();
        
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
        var result = ExecuteHelperScript(connectionString, "CreateSchemaVersionsTable");

        if (!result.Successful)
            throw new ApplicationException("Failed to ensure schema_versions table exists", result.Error);
    }

    
    /// <summary>
    /// Clean database using DbUp library.
    /// </summary>
    /// <param name="connectionString">The connection string of current database.</param>
    public static void CleanDatabase(string connectionString)
    {
        var result = ExecuteHelperScript(connectionString, "CleanDatabaseTables");

        if (!result.Successful)
            throw new ApplicationException("Clean database failed", result.Error);
    }

    private static DatabaseUpgradeResult ExecuteHelperScript(string connectionString, string scriptName)
    {
        var scriptPath = Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}\DatabaseScripts\Helper\{scriptName}.sql");
        var contentScript = File.ReadAllText(scriptPath);
        
        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScript(scriptName, contentScript)
            .LogToConsole()
            .Build();

        return upgradeEngine.PerformUpgrade();
    }
}