using Nexora.API;
using Nexora.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Port über Properties/launchSettings.json änderbar
builder.Services.AddOpenApi();

var app = builder.Build();

var api = app.MapGroup("/api");

//____________________USER_____________________




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//______________API Endpoints____________________
api.MapUserEndpoint();


app.Run();
