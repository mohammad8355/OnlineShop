using BusinessLogicLayer.services;
using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PresentationLayer.MessageSender;
using PresentationLayer.MessageSender.TotpPhoneVarification;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
#region AddContext 

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
#endregion
#region SendEmail
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSetting"));
builder.Services.AddSingleton<EmailSender>();
#endregion
#region TotpOptions
builder.Services.AddTransient<IPhoneProvider, PhoneProvider>();
builder.Services.Configure<TotpPhoneProviderOptions>(options =>
{
    options.StepInSecond = 30;
    options.TotpSize = 6;
});
#endregion
#region Addcookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(15);
    options.LogoutPath = "/Account/Logout";
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
 );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");
app.Run();
