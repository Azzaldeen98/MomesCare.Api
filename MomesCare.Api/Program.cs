using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MomesCare.Api;
using System.Text;
using MomesCare.Api.Helpers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using MomesCare.Api.ApiClient.Entitis;
using FirebaseAdmin.Auth;
using MomesCare.Api.ApiClient.Entitis;
using MomesCare.Api.Seeds;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Injection_Depandinces;
using MomesCare.Api.Installs;
using MomesCare.Api.Services.Static;
using MomesCare.Api.Repository;
using AutoMapper;
using MomesCare.Api.Services;
using MomesCare.Api.Services.SubServices;
using MomesCare.Api.Services.BackgroundServices.NewFolder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(option =>
           option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));



var firebaseApp = FirebaseApp.Create(new AppOptions()
{
    //Credential = GoogleCredential.FromFile("firebaseServiceAccount.json")
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebaseServiceAccount.json"))
});


var firebaseConfig = builder.Configuration.GetSection("FirebaseConfig").Get<FirebaseConfig>();
var firebaseServiceAccount = builder.Configuration.GetSection("FirebaseServiceAccount")
    .Get<FirebaseServiceAccount>();

builder.Services.Configure<FirebaseConfig>(builder.Configuration.GetSection("FirebaseConfig"));
builder.Services.Configure<FirebaseServiceAccount>(builder.Configuration.GetSection("FirebaseServiceAccount"));

builder.Services.AddOptions<FirebaseConfig>().BindConfiguration("FirebaseConfig");
builder.Services.AddOptions<FirebaseServiceAccount>().BindConfiguration("FirebaseServiceAccount");



//  ÂÌ∆… Firebase Admin SDK »«” Œœ«„ „·› JSON
//FirebaseApp.Create(new AppOptions
//{
//    Credential = GoogleCredential.FromFile("firebaseServiceAccount.json")
//});

// Register FirebaseAuthClient
builder.Services.AddSingleton<FirebaseAuthClient>(sp =>
{

    

    return new FirebaseAuthClient(new FirebaseAuthConfig
    {
        ApiKey = firebaseConfig.ApiKey,
        AuthDomain = firebaseConfig.AuthDomain,

        //ApiKey = "AIzaSyA7mFdEtgyHJgGYA-xzBLNs3FIxCn7BKcc",
        //AuthDomain = "momscare-8daff.firebaseapp.com",

        Providers = new FirebaseAuthProvider[]
        {
            new EmailProvider(),

        }
    });
});

//  ”ÃÌ· FirebaseAuth
builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserClaimsHelper, UserClaimsHelper>();
builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //options.SignIn.RequireConfirmedEmail = true;

}).AddRoles<IdentityRole>()
 .AddRoleManager<RoleManager<IdentityRole>>()
 .AddSignInManager<SignInManager<ApplicationUser>>()
 .AddUserManager<UserManager<ApplicationUser>>()
 .AddEntityFrameworkStores<DataContext>()
 .AddDefaultTokenProviders();


builder.Services.AddControllers();


// install 
builder.Services.InstallApiClientServices();
builder.Services.InstallApiClientRepository();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options))
.AddJwtBearer( options =>
                 {
                     options.RequireHttpsMetadata = false;
                     options.SaveToken = true;
                     options.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         ValidIssuer = builder.Configuration["Jwt:Issuer"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                     };

                 });



/// Background Services
builder.Services.AddHostedService<BackgroundWorkerService>();



builder.Services.AddAuthorization();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


SeedData.EnsureSeedData(app, app.Services.GetRequiredService<FirebaseAuthClient>());



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();
// Add session middleware
//app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 

app.Run();