using FinalProjectHome.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<FinalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.MapAreaControllerRoute(
    name: "admin_default",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

//details routing
app.MapControllerRoute(
    name: "restaurant-details",
    pattern: "{controller=Restaurants}/{action=Details}/{id}/{slug}/"
    );

//edit routing
app.MapControllerRoute(
    name: "restaurant-edit",
    pattern: "{controller=Restaurants}/{action=Edit}/{id}/{slug}/",
    defaults: new { controller = "Restaurants", action = "Edit" }
    );

//delete routing
app.MapControllerRoute(
    name: "restaurant-delete",
    pattern: "{controller=Restaurants}/{action=Delete}/{id}/{slug}/",
    defaults: new { controller = "Restaurants", action = "Delete" }
    );

//list routing
app.MapControllerRoute(
    name: "restaurants-list",
    pattern: "restaurants/",
    defaults: new { controller = "Restaurants", action = "Index" }
);

//default routing   
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
