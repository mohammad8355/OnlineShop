using BusinessEntity;
using BusinessEntity.Models;
using BusinessLogicLayer.AdjKeyService;
using BusinessLogicLayer.AdjValueService;
using BusinessLogicLayer.CategoryService;
using BusinessLogicLayer.DiscountService;
using BusinessLogicLayer.GeneralService;
using BusinessLogicLayer.ProductService;
using DataAccessLayer;
using DataAccessLayer.services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PresentationLayer.MessageSender;
using PresentationLayer.MessageSender.TotpPhoneVarification;
using System.Security.Policy;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
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

#region main repository service
builder.Services.AddScoped<MainRepository<Category>>();
builder.Services.AddScoped<MainRepository<Product>>();
builder.Services.AddScoped<MainRepository<SubCategory>>();
builder.Services.AddScoped<MainRepository<AdjKey>>();
builder.Services.AddScoped<MainRepository<AdjValue>>();
builder.Services.AddScoped<MainRepository<Discount>>();
builder.Services.AddScoped<MainRepository<Order>>();
builder.Services.AddScoped<MainRepository<OrderDetails>>();
builder.Services.AddScoped<MainRepository<General>>();
builder.Services.AddScoped<MainRepository<Weblog>>();
builder.Services.AddScoped<MainRepository<BlogSection>>();
builder.Services.AddScoped<MainRepository<KeyToProduct>>();
builder.Services.AddScoped<MainRepository<KeyToSubCategory>>();

#endregion
#region businessLogic layer service
builder.Services.AddScoped<CategoryLogic>();
builder.Services.AddScoped<SubCategoryLogic>();
//builder.Services.AddScoped<AdjKeyLogic>();
//builder.Services.AddScoped<AdjValueLogic>();
//builder.Services.AddScoped<DiscountLogic>();
//builder.Services.AddScoped<ProductLogic>();
//builder.Services.AddScoped<GeneralLogic>();

#endregion
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
app.MapRazorPages();
app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
 );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");
app.Run();
