using COMP2139_Lab1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using COMP2139_Lab1.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using COMP2139_Lab1.Areas.ProjectManagement.Models;
using COMP2139_Lab1.Services.Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


/*
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI()
    .AddDefaultTokenProviders();
*/
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



/*    
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
*/


// This ensures that whenever an IEmailSender is injected an instance of EmailSender is provided
//builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddTransient<IEmailSender, EmailSender>();


// Setup Serilog as the logging provider
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    // Configure Serilog to read from the apps settings (appsettings.json)
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithRedirects("/Home/NotFound/?statusCode={0}");
}

using var scope = app.Services.CreateScope();
var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

try
{
    // Get services needed for role seeding
    // scope.ServiceProvider - is used to access instances of registered services
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed Roles
    await ContextSeed.SeedRolesAsync(userManager, roleManager);
    
    // Seed superAdmin
    await ContextSeed.SuperSeedRolesAsync(userManager, roleManager);


} catch (Exception e)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(e, "An error occurred when attempting to seed the roles to the system");
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.MapRazorPages();


app.MapControllerRoute(
    name: "areas",
    pattern: "{areas:exists}/{controller=Project}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{areas:exists}/{controller=ProjectTask}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
