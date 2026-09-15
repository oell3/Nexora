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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);

// Listenport aus der Konfiguration (appsettings.json / appsettings.{Environment}.json
// oder Umgebungsvariable Server__Port). Ist nichts gesetzt, gelten die URLs aus
// launchSettings.json bzw. ASPNETCORE_URLS.
var serverPort = builder.Configuration.GetValue<int?>("Server:Port");
if (serverPort is > 0)
{
    builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(serverPort.Value));
}

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Freigegebene Frontend-Origins, mehrere durch ";" getrennt
// (Konfiguration: Cors:AllowedOrigins, in Docker z. B. Cors__AllowedOrigins).
var allowedOrigins = (builder.Configuration["Cors:AllowedOrigins"] ?? "http://localhost:4200")
    .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// DI
builder.Services.AddScoped<Database>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordHasher<object>>();
builder.Services.AddScoped<ProjekteServices>();
builder.Services.AddScoped<Helpers>();

//DI Repositories
builder.Services.AddScoped<AngebotRepository>();
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

app.UseForwardedHeaders();

var api = app.MapGroup("/api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();



app.MapControllers();

api.MapUserEndpoint();
api.MapProjekteEndpoint();
api.MapAuthEndpoint();
api.MapAngebotEndPoint();
// Endpoints


app.Run();