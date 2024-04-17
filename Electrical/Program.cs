using DataAccessLayer.Models;
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
using Infrustructure.MessageSender;
using Infrustructure.MessageSender.TotpPhoneVarification;
using System.Security.Policy;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Utility.ReturnMultipleData;
using BusinessLogicLayer.ProductPhotoService;
using BusinessLogicLayer.KeyToSubCategoryService;
using BusinessLogicLayer.CategoryToProductService;
using BusinessLogicLayer.ValueToProductService;
using BusinessLogicLayer.KeyToProductService;
using Utility.DiscountCodeGenerator;
using BusinessLogicLayer.DiscountToProductService;
using BusinessLogicLayer.ContactService;
using BusinessLogicLayer.BlogPostService;
using BusinessLogicLayer.TagService;
using BusinessLogicLayer.TagToBlogPostService;
using Infrustructure.uploadfile;
using BusinessLogicLayer.CommentService;
using BusinessLogicLayer.TicketService;
using BusinessLogicLayer.OrderDetailsService;
using BusinessLogicLayer.OrderService;
using BusinessLogicLayer.BrandService;
using Utility.ProductCodeGenerator;
using BusinessLogicLayer.favoriteProductService;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using PresentationLayer.Areas.Identity.Claims;
using Infrustructure.Payment;
using BusinessLogicLayer.NotificationLogic;
using Infrustructure.SignalR;
using Microsoft.AspNet.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    //opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
#region AddContext 

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{

    options.SignIn.RequireConfirmedAccount = true;
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
    


} ).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
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
builder.Services.AddScoped<MainRepository<KeyToProduct>>();
builder.Services.AddScoped<MainRepository<DiscountToProduct>>();
builder.Services.AddScoped<MainRepository<KeyToSubCategory>>();
builder.Services.AddScoped<MainRepository<Product>>();
builder.Services.AddScoped<MainRepository<AdjKey>>();
builder.Services.AddScoped<MainRepository<AdjValue>>();
builder.Services.AddScoped<MainRepository<Discount>>();
builder.Services.AddScoped<MainRepository<Order>>();
builder.Services.AddScoped<MainRepository<Notification>>();
builder.Services.AddScoped<MainRepository<OrderDetails>>();
builder.Services.AddScoped<MainRepository<General>>();
builder.Services.AddScoped<MainRepository<BlogPost>>();
builder.Services.AddScoped<MainRepository<ProductPhoto>>();
builder.Services.AddScoped<MainRepository<Category>>();
builder.Services.AddScoped<MainRepository<CategoryToProduct>>();
builder.Services.AddScoped<MainRepository<ValueToProduct>>();
builder.Services.AddScoped<MainRepository<KeyToProduct>>();
builder.Services.AddScoped<MainRepository<Contact>>();
builder.Services.AddScoped<MainRepository<Tag>>();
builder.Services.AddScoped<MainRepository<TagToBlogPost>>();
builder.Services.AddScoped<MainRepository<Brand>>();
builder.Services.AddScoped<MainRepository<Commnet>>();
builder.Services.AddScoped<MainRepository<Ticket>>();
builder.Services.AddScoped<MainRepository<FavoriteProduct>>();
builder.Services.AddTransient<IClaimsTransformation,RoleClaimsTransformation>();
#endregion
#region businessLogic layer service
builder.Services.AddScoped<AdjKeyLogic>();
builder.Services.AddScoped<AdjValueLogic>();
builder.Services.AddScoped<DiscountLogic>();
builder.Services.AddScoped<ProductLogic>();
builder.Services.AddScoped<GeneralLogic>();
builder.Services.AddScoped<ProductPhotoLogic>();
builder.Services.AddScoped<CategoryLogic>();
builder.Services.AddScoped<KeyToSubCategoryLogic>();
builder.Services.AddScoped<CategoryToProductLogic>();
builder.Services.AddScoped<ValueToProductLogic>();
builder.Services.AddScoped<NotificationLogic>();
builder.Services.AddScoped<KeyToProductLogic>();
builder.Services.AddScoped<DiscountToProductLogic>();
builder.Services.AddScoped<ContactsLogic>();
builder.Services.AddScoped<BlogPostLogic>();
builder.Services.AddScoped<TagLogic>();
builder.Services.AddScoped<TagToBlogPostLogic>();
builder.Services.AddScoped<CommentLogic>();
builder.Services.AddScoped<TicketLogic>();
builder.Services.AddScoped<favoriteProductLogic>();
builder.Services.AddScoped<OrderLogic>();
builder.Services.AddScoped<BrandLogic>();
builder.Services.AddScoped<OrderDetailsLogic>();
#endregion
builder.Services.AddScoped<ReturnMultipleData<UploadFile>>();
builder.Services.AddScoped<UploadFile>();
builder.Services.AddScoped<ProductCodeGenerator>();
builder.Services.AddScoped<HubConfig>();
builder.Services.AddScoped<ZarinPalPay>();
builder.Services.AddScoped<CodeGenerator>();
builder.Services.AddSignalR();
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
app.MapHub<HubConfig>("/notification");
//app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");
app.Run();
