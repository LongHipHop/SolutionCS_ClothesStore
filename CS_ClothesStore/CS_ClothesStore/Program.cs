using CS_ClothesStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<HttpClient>(sp =>
{
    var httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(30)
    };
    return httpClient;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).
AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(3);
    options.LoginPath = "/Authentication/Login";
    options.AccessDeniedPath = "/Authentication/AccessDenied";
})
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.ClaimActions.MapJsonKey("picture", "picture", "url");
    options.CallbackPath = "/signin-google";
})
.AddJwtBearer("Jwt", options =>
{
    options.Authority = "http://localhost:5013";
    options.Audience = "http://localhost:5013";
    options.RequireHttpsMetadata = false;
    options.MapInboundClaims = false;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN", policy =>
        policy.RequireRole("ADMIN"));
    options.AddPolicy("MANAGER", policy =>
        policy.RequireRole("MANAGER"));
    options.AddPolicy("CONSULTING_STAFF", policy =>
        policy.RequireRole("CONSULTING_STAFF"));
    options.AddPolicy("SALE_STAFF", policy =>
        policy.RequireRole("SALE_STAFF"));
    options.AddPolicy("CUSTOMER", policy =>
        policy.RequireRole("CUSTOMER"));
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();

    var path = context.Request.Path.Value.ToLower();

    if (context.Response.StatusCode == 401 || context.Response.StatusCode == 403 || context.Response.StatusCode == 404)
    {
        var user = context.User;

        string redirectPath = "Error/ErrorPage";

        if (user.Identity?.IsAuthenticated == true)
        {
            if (user.IsInRole("CUSTOMER"))
            {
                redirectPath = "Error/ErrorPage";
            }
        }

        context.Response.Redirect(redirectPath);
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseSession();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
