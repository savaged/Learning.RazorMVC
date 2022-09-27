using Microsoft.Data.Sqlite;
using System.Data;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDbConnection>(s =>
{
    return new SqliteConnection(connectionString:
        $"Data Source={Environment.CurrentDirectory}{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}Database.sqlite3;");
});
builder.Services.AddSingleton<IModelService, ModelService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
