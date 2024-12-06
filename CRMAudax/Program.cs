using CRMAudax.Job;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Configuration;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder()
.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
.AddJsonFile("appsettings.json")
.Build();

string jwtKey = configuration.GetSection("JWTAth")["JWT_Key"] ?? "";
string jwtIssuer = configuration.GetSection("JWTAth")["JWT_Issuer"] ?? "";
string jwtAudience = configuration.GetSection("JWTAth")["JWT_Audience"] ?? "";
string jwtTokenExpiryInMinutes = configuration.GetSection("JWTAth")["JWT_TokenExpiryInMinutes"] ?? "";

builder.Services.AddQuartz(q =>
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

    string time = configuration.GetSection("RotinaNotificacao")["Periodo"];
    var startTime = DateBuilder.EvenMinuteDate(DateTimeOffset.Now.AddMonths(1));

    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("NotificacaoJob");
    q.AddJob<NotificacaoJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("NotificacaoJob-Trigger")
        //.StartAt(startTime) // Define a data de início
        .WithCronSchedule(time)); //Configuração do tempo
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.ConsentCookie.IsEssential = true;
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = "Audax";
    options.LoginPath = "/Home/Index";
    options.LogoutPath = "/Home/Index";
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,

        ValidateAudience = true,
        ValidAudience = jwtAudience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

        ValidateLifetime = true,

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
