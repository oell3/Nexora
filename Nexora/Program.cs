using Microsoft.AspNetCore.Identity;
using Nexora.API;
using Nexora.Data;
using Nexora.Services;
using Nexora.utils;


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
builder.Services.AddScoped<ProjekteRepository>();

builder.Services.AddScoped<RechnungenRepository>();

builder.Services.AddScoped<TermineRepository>();

builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<VertraegeRepository>();

builder.Services.AddScoped<VorlageRepository>();

builder.Services.AddScoped<WartungRepository>();

builder.Services.AddScoped<ZahlungRepositroy>();




builder.Services.AddScoped<UsersServices>();

var app = builder.Build();

var api = app.MapGroup("/api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

// Endpoints
api.MapUserEndpoint();
api.MapProjekteEndpoint();

app.Run();