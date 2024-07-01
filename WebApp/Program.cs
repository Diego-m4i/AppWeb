using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.data;
using WebApp.models;
using WebApp.services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        /*// Configure DbContext with SQL Server
        builder.Services.AddDbContext<AppDb>(options =>
            options.UseSqlServer(connectionString, 
                sqlServerOptions => sqlServerOptions.MigrationsAssembly("WebApp.Context"))
                .EnableRetryOnFailure());*/

        builder.Services.AddRazorPages();

        // Configure Authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "WebAppVetrinaCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.SlidingExpiration = true;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ReturnUrlParameter = "";
            });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "WebAppVetrinaCookie";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            options.SlidingExpiration = true;
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.ReturnUrlParameter = "";
        });

        // Register ProductService with DI container
        builder.Services.AddScoped<ProductService>();

        // Configure Authorization
        builder.Services.AddAuthorization();
        builder.Services.AddSession();
        builder.Services.AddHttpContextAccessor();

        // Configure Identity options
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.User.AllowedUserNameCharacters += " ";
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        });

        /*// Configure Identity
        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AppDb>();
            */

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.Run();
    }
}
