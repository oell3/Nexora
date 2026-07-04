using Microsoft.AspNetCore.Identity;
using Nexora.API;
using Nexora.Data;
using Nexora.services;
using Nexora.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DI
builder.Services.AddScoped<Database>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordHasher<object>>();

builder.Services.AddScoped<UsersServices>();

var app = builder.Build();

var api = app.MapGroup("/api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Endpoints
api.MapUserEndpoint();

app.Run();