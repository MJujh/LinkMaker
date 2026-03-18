using LinkMaker.Common.Contants;
using LinkMaker.Data;
using LinkMaker.Data.Interfaces;
using LinkMaker.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Register your Service and its Interface
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<LinkMakerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<LinkMakerIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LinkMakerIdentityDbContextConnection")));

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy(Configs.IPPolicyLimiter, httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress!.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromSeconds(1)
            }));
});

// Đăng ký Redis Distributed Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // hoặc connection string của Redis server
    options.InstanceName = "linkmakerredis";
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    //options.SignIn.RequireConfirmedAccount = true;
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    // User settings
    options.User.RequireUniqueEmail = true;
    // Sign-in settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}
)
.AddEntityFrameworkStores<LinkMakerIdentityDbContext>()
.AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<LinkMakerIdentityDbContext>();

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
