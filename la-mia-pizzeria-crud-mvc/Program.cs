using la_mia_pizzeria_crud.Database;
using la_mia_pizzeria_crud.CustomLoggers;
using la_mia_pizzeria_crud.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("PizzeriaContextConnection") ?? throw new InvalidOperationException("Connection string 'PizzeriaContextConnection' not found.");

builder.Services.AddDbContext<PizzeriaContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PizzeriaContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PizzeriaContext, PizzeriaContext>();
//builder.Services.AddScoped<ICustomLoggers, CustomConsoleLogger>();
builder.Services.AddScoped<ICustomLogger, CustomFileLogger>();

//setting JSON directive so doesn't try to serialize the Cycliyng informations
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
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
app.UseAuthentication();

app.UseAuthorization();



//if (User.IsInRole("ADMIN")){
/*Object defaultRoute = new
{
    controller = "Pizza",
    action = "Index",
};*/
//else{ pattern = "{controller=Home}/{action=UserIndex}/{id?}";}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=UserIndex}/{id?}"
    /*pattern: "",
    defaults: defaultRoute*/
    );

app.MapRazorPages();

app.Run();
