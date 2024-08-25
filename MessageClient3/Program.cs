var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient with BaseAddress from appsettings.json
builder.Services.AddHttpClient("MessageApiClient", client =>
{
    var baseAddress = builder.Configuration["HttpClient:BaseAddress"];

    if (!Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
    {
        throw new InvalidOperationException("Base address for HttpClient is not valid.");
    }

    client.BaseAddress = uri;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Messages}/{action=Index}/{id?}");

app.Run();
