using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
// Add Kendo UI services to the services container
builder.Services.AddKendo();

/// registreer elke service bij de DI container
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IMenuItemCategoryService, MenuItemCategoryService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ICookService, CookService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ITimeSlotService, TimeSlotService>();
/// Sende Email
builder.Services.AddHostedService<MailerBackgroundService>();
builder.Services.AddTransient<IMailerWorkerService, MailerWorkerService>();


/// ServiceLifetime.Transient => belangrijk wanneer de context meerdere keren per action call wordt gebruikt om een entiteit op te halen.
builder.Services.AddDbContext<TraiteurAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TraiteurAppDbContext>();

var app = builder.Build();

var serviceProvider = app.Services.CreateScope().ServiceProvider;
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

await roleManager.CreateAsync(new IdentityRole("Administrator"));
await roleManager.CreateAsync(new IdentityRole("Customer"));
await roleManager.CreateAsync(new IdentityRole("Cook"));

var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

var cookService = serviceProvider.GetRequiredService<ICookService>();
var cookUser = new IdentityUser
{
    Email = "cook@ucll.be",
    UserName = "cook@ucll.be",
    EmailConfirmed = true
};
var result = await userManager.CreateAsync(cookUser, "CookPass123..");
if (result.Succeeded)
{
    await userManager.AddToRoleAsync(cookUser, "Cook");
    var cook = new Cook
    {
        ChefName = "ChefNr1",
        YearsOfExperience = 10,
        IdentityUserId = cookUser.Id
    };
    await cookService.CreateCookAsync(cook);
}

// create an admin user
var adminUser = new IdentityUser
{
    Email = "admin@ucll.be",
    UserName = "admin@ucll.be",
    EmailConfirmed = true
};
result = await userManager.CreateAsync(adminUser, "AdminPass123..");
if (result.Succeeded)
{
    await userManager.AddToRoleAsync(adminUser, "Administrator");

}

// create an customer user
var customerService = serviceProvider.GetRequiredService<ICustomerService>();
var customerUser = new IdentityUser
{
    Email = "customer@ucll.be",
    UserName = "customer@ucll.be",
    EmailConfirmed = true
};
result = await userManager.CreateAsync(customerUser, "CustomerPass123..");
if (result.Succeeded)
{
    await userManager.AddToRoleAsync(customerUser, "Customer");
    var customer = new Customer
    {
        Name = "CustomerNr1",
        EmailAddress = "customer@ucll.be",
        IdentityUserId =
        customerUser.Id
    };
    await customerService.CreateCustomerAsync(customer);
}


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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
