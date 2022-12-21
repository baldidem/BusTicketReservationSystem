using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkylerTourism.Business.Abstract;
using SkylerTourism.Business.Concrete;
using SkylerTourism.Data.Abstract;
using SkylerTourism.Data.Concrete.EfCore;
using SkylerTourism.WebUI.Identity;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyIdentityContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteCon")));

builder.Services.AddIdentity<MyIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MyIdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(500);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".MiniShopTekrar.Security.Cookie"
    };

});



builder.Services.AddDbContext<SkylerTourismContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteCon")));

builder.Services.AddScoped<ICityService, CityManager>();
builder.Services.AddScoped<ICityRepository, EfCoreCityRepository>();
builder.Services.AddScoped<ITripService, TripManager>();
builder.Services.AddScoped<ITripRepository, EfCoreTripRepository>();
builder.Services.AddScoped<ITicketService, TicketManager>();
builder.Services.AddScoped<ITicketRepository, EfCoreTicketRepository>();
builder.Services.AddScoped<IPassengerService, PassengerManager>();
builder.Services.AddScoped<IPassengerRepository, EfCorePassengerRepository>();
builder.Services.AddScoped<IBusService, BusManager>();
builder.Services.AddScoped<IBusRepository, EfCoreBusRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
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
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
