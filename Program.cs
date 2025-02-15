using HospitalPatientSystem.Data;
using HospitalPatientSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with the dependency injection container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Adjust for your DB provider

// Add Identity services
builder.Services.AddIdentity<Doctor, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add RoleManager & UserManager to Dependency Injection
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<UserManager<Doctor>>();

// Add other services like controllers, session, and authentication
builder.Services.AddControllersWithViews();

// Add session services for session management
builder.Services.AddDistributedMemoryCache(); // For in-memory session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Ensure session cookies are HTTP-only
    options.Cookie.IsEssential = true; // Mark session cookies as essential for the application
});

// Add authorization and authentication
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// 🔹 Create a service scope and seed roles before running the app
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable session middleware
app.UseSession();

// Enable authentication middleware (must be after UseRouting and before UseAuthorization)
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes (default route)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ✅ Move SeedRoles outside Program.cs main logic
async Task SeedRoles(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<Doctor>>();

    string[] roleNames = { "Admin", "Doctor" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create a default admin user if it doesn't exist
    var adminUser = await userManager.FindByEmailAsync("admin@hospital.com");
    if (adminUser == null)
    {
        var newAdminUser = new Doctor
        {
            UserName = "admin@hospital.com",
            Email = "admin@hospital.com"
        };
        var result = await userManager.CreateAsync(newAdminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdminUser, "Admin");
        }
    }

    // Create a default doctor user if it doesn't exist
    var doctorUser = await userManager.FindByEmailAsync("doctor@hospital.com");
    if (doctorUser == null)
    {
        var newDoctorUser = new Doctor
        {
            UserName = "doctor@hospital.com",
            Email = "doctor@hospital.com"
        };
        var result = await userManager.CreateAsync(newDoctorUser, "Doctor123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newDoctorUser, "Doctor");
        }
    }
}
