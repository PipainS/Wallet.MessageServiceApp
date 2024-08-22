using FluentMigrator.Runner;
using MessageService.API.Repositories;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();  // ���������� NLog ��� �����������
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add services to the container.
    builder.Services.AddControllers();

    // ��������� ������ �����������
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // ����������� FluentMigrator
    builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(CreateMessagesTable).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole());

    // ����������� WebSocketConnectionManager � WebSocketHandler ��� Singleton
    builder.Services.AddSingleton<WebSocketConnectionManager>();
    builder.Services.AddSingleton<WebSocketHandler>();

    // ����������� �����������
    builder.Services.AddSingleton<MessageRepository>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<MessageRepository>>();
        return new MessageRepository(connectionString, logger);
    });


    var app = builder.Build();

    app.UseWebSockets();

    // ���������� ��������
    using (var scope = app.Services.CreateScope())
    {
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.UseStaticFiles();
    app.MapControllers();
    app.Map("/ws", async context =>
    {
        var handler = context.RequestServices.GetRequiredService<WebSocketHandler>();
        await handler.HandleWebSocketConnection(context);
    });
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();  // ���������, ��� ��� ���� �������� ����� ���������� ���������
}