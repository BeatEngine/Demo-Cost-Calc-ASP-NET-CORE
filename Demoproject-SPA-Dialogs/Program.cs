using static Demoproject_SPA_Dialogs.Database.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Demoproject_SPA_Dialogs.Database;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

string connectionString = builder.Configuration.GetConnectionString("demoproject");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<DatabaseContext>(options =>
//    options.UseInMemoryDatabase("demo1"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//For using CSS or JS form WWW-root
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
