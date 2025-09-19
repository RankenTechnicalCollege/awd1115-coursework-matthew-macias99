using ContactManager.Models;
using Microsoft.EntityFrameworkCore;    

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var cs = builder.Configuration.GetConnectionString("Default")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=ContactManagerDB;Trusted_Connection=True;TrustServerCertificate=True";

builder.Services.AddDbContext<ContactManagerContext>(opt => opt.UseSqlServer(cs));

builder.Services.AddRouting(o =>
{
    o.LowercaseUrls = true;
    o.AppendTrailingSlash = true;
});

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
    pattern: "{controller=Contact}/{action=Index}/{id?}/{slug?}");

app.Run();
