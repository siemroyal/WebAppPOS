using Microsoft.EntityFrameworkCore;
using WebAppPOS.Data;
using WebAppPOS.Repositories.Implementations;
using WebAppPOS.Repositories.Interfaces;
using WebAppPOS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IImageRepository,ImageRepository>();
builder.Services.AddScoped<ImageService>();
//Add connection
//builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
/*
1. Install Package
    -Microsoft.EntityFrameworkCore;
    -Microsoft.EntityFrameworkCore.SqlServer;
    -Microsoft.EntityFrameworkCore.Tools;
    -Microsoft.EntityFrameworkCore.Design;
2. Context class (AppDbContext.cs)
3. Connection string (appsettings.json)
4. Register service (Program.cs)
5. Entites (Category, Product, Purchase, ...)
6. Add DbSet<T> in context class
7.  //dotnet ef migrations add initialCreate
    //dotnet ef database update
    //Add-Migration initialCreate
    //Update-Database
8. CRUD Operations
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
