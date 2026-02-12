using Microsoft.EntityFrameworkCore;
using Reception.Models;
using Reception.IService;
using Reception.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ReceptionDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IServices, Services>();
builder.Services.AddScoped<IBillingServices, BillingServices>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



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
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}" 
);


app.Run();
