using Microsoft.EntityFrameworkCore;
using IIMSv1.Data;
using Microsoft.AspNetCore.Identity;
using IIMSv1.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConString") ?? throw new InvalidOperationException("Connection string 'ConString' not found");
builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddDefaultIdentity<AccountUser>(options =>
{
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InventoryDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();


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
  pattern: "{controller=Admin}/{action=Index}/{id?}");

app.MapRazorPages();
app.UseAntiforgery();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "System Administrator", "Inventory Manager", "Inventory Administrator" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
    DataSeed.Initialize(scope.ServiceProvider);

}
app.Run();