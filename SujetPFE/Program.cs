using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Infrastructure; // This is the crucial line you need to add
using SujetPFE.Services; // Ajoutez cette ligne pour accéder à ExcelServicesemployee

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Modification ici : Utiliser la base de données aspnet-SujetPFE-...

builder.Services.AddDbContext<PcbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SujetPFE.Infrastructure")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PcbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Disable email confirmation requirement
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false; // For newer templates
});
builder.Services.AddControllersWithViews();

// Ajouter l'enregistrement de votre service ExcelServicesemployee ici
builder.Services.AddScoped<ExcelServicesemployee>();
builder.Services.AddScoped<ExcelToGroupeMapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Ajouter UseStaticFiles() avant UseRouting()
app.UseStaticFiles();

app.UseRouting();

// Ajouter UseAuthentication() avant UseAuthorization() (si vous utilisez Identity)
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();