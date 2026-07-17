using Microsoft.AspNetCore.Identity;
using Nexora.API;
using Nexora.cookie;
using Nexora.cookie.Nexora.Cookie;
using Nexora.Data;
using Nexora.Services;
using Nexora.utils;
using System.Security.Cryptography;
using JWT;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DI
builder.Services.AddScoped<Database>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordHasher<object>>();
builder.Services.AddScoped<ProjekteServices>();
builder.Services.AddScoped<Helpers>();

//DI Repositories
builder.Services.AddScoped<AngeboteRepository>();
builder.Services.AddScoped<AngebotGruppenRepository>();
builder.Services.AddScoped<AngebotPositionRepositorycs>();
builder.Services.AddScoped<AngebotRevisionGruppenRepository>();
builder.Services.AddScoped<AngebotRevisionPositionRepository>();
builder.Services.AddScoped<AngebotstextRepository>();
builder.Services.AddScoped<AngebotstextRevisionenRepository>();

builder.Services.AddScoped<AnsprechpartnerRepository>();

builder.Services.AddScoped<KatalogRepository>();

builder.Services.AddScoped<KundenRepository>();

builder.Services.AddScoped<MandantRepository>();

builder.Services.AddScoped<ObjekteRepository>();

builder.Services.AddScoped<ProjektdetailRepositroy>();
builder.Services.AddScoped<ProjektRepository>();

builder.Services.AddScoped<RechnungenRepository>();

builder.Services.AddScoped<TermineRepository>();

builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<VertraegeRepository>();

builder.Services.AddScoped<VorlageRepository>();

builder.Services.AddScoped<WartungRepository>();

builder.Services.AddScoped<ZahlungRepositroy>();



builder.Services.AddScoped<RefreshCookie>();
builder.Services.AddScoped<UserServices>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

var api = app.MapGroup("/api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
var key = RandomNumberGenerator.GetBytes(64);
var base64Key = Convert.ToBase64String(key);
Console.WriteLine(base64Key);
app.UseHttpsRedirection();

app.MapControllers();

// Endpoints
api.MapUserEndpoint();
api.MapProjekteEndpoint();

app.Run();