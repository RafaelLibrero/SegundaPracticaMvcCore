using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SegundaPracticaMvcCore.Data;
using SegundaPracticaMvcCore.Helpers;
using SegundaPracticaMvcCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services
    .AddControllersWithViews(options => options.EnableEndpointRouting = false);

string connectionString = builder.Configuration.GetConnectionString("SqlLibros");

builder.Services.AddDbContext<LibrosContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<RepositoryLibros>();
builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddSession();

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

app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Libros}/{action=Index}/{id?}"
        );
});
app.Run();
