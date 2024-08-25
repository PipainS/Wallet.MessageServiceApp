using FluentMigrator.Runner;
using MessageService.API.MappingProfiles;
using MessageService.API.Migrations;
using MessageService.API.Repositories;
using NLog;
using NLog.Web;
using Npgsql;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Clear default logging providers and configure NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container
    builder.Services.AddControllers();
    // Configure Swagger/OpenAPI for API documentation
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Register AutoMapper with the specified profile
    builder.Services.AddAutoMapper(typeof(MessageProfile));

    // Retrieve the connection string from configuration
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

    // Ensure that the database exists
    EnsureDatabaseExists(connectionString);


    // Register FluentMigrator with Postgres and the connection string
    builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(CreateMessagesTable).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole());

    // Register WebSocket services as Singletons
    builder.Services.AddSingleton<WebSocketConnectionManager>();

    // Register WebSocketHandler with the dependency injection container
    builder.Services.AddSingleton<IWebSocketHandler>(sp =>
    {
        var connectionManager = sp.GetRequiredService<WebSocketConnectionManager>();
        var logger = sp.GetRequiredService<ILogger<WebSocketHandler>>();
        return new WebSocketHandler(connectionManager, logger);
    });

    // Register the repository with the dependency injection container
    builder.Services.AddSingleton<IMessageRepository>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<MessageRepository>>();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        return connectionString == null
            ? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.")
            : (IMessageRepository)new MessageRepository(connectionString, logger);
    });

    var app = builder.Build();

    // Enable WebSockets middleware
    app.UseWebSockets();

    // Ensure that the database exists; create it if it does not
    static void EnsureDatabaseExists(string? connectionString)
    {
        // Validate that the connection string is not null
        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null.");
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            // Store the original database name
            Database = "postgres"
        };

        var databaseName = new NpgsqlConnectionStringBuilder(connectionString).Database;

        using var connection = new NpgsqlConnection(builder.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";
        var exists = command.ExecuteScalar() != null;

        if (!exists)
        {
            command.CommandText = $"CREATE DATABASE \"{databaseName}\"";
            command.ExecuteNonQuery();
        }
    }


    // Call this method to ensure the database exists before running migrations
    EnsureDatabaseExists(connectionString);

    // Perform database migrations
    using (var scope = app.Services.CreateScope())
    {
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        // Enable Swagger in development mode
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.UseStaticFiles();
    app.MapControllers();

    // Map WebSocket connections to the handler
    app.Map("/ws", async context =>
    {
        var handler = context.RequestServices.GetRequiredService<IWebSocketHandler>();
        await handler.HandleWebSocketConnection(context);
    });

    // Run the application
    app.Run();
}
catch (Exception ex)
{
    // Log any exceptions that occur
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure that NLog flushes and closes properly
    LogManager.Shutdown();
}
