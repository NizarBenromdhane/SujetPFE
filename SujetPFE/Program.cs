using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Data;
using SujetPFE.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Utilisateur>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI(); // Use the default Identity UI for login, registration, etc.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Default route to the login page

app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Route de test pour la connexion � la base de donn�es
app.MapControllerRoute(
    name: "test",
    pattern: "test",
    defaults: new { controller = "Account", action = "TestConnexion" });

app.MapRazorPages();

// --- AJOUT DU CODE POUR CR�ER UN UTILISATEUR PAR D�FAUT ---
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Utilisateur>>();
    var user = await userManager.FindByEmailAsync("test@example.com"); // Ou un autre email

    if (user == null)
    {
        user = new Utilisateur
        {
            UserName = "test@example.com",
            Email = "test@example.com",
            Matricule = "test1234" // Ou un autre matricule
        };

        var result = await userManager.CreateAsync(user, "MotDePasse123!"); // Mot de passe par d�faut
        if (!result.Succeeded)
        {
            // G�rer les erreurs de cr�ation d'utilisateur (vous pouvez ajouter un logging ici)
            Console.WriteLine("Erreur lors de la cr�ation de l'utilisateur :");
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
}
// --- FIN DE L'AJOUT ---

app.Run();