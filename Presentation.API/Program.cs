using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.API.Interface;
using Refit;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Refit to call Business Logic Tier
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<Presentation.API.AuthHeaderHandler>();
builder.Services.AddRefitClient<IBusinessLogicClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5002"))
    .AddHttpMessageHandler<Presentation.API.AuthHeaderHandler>();

// Add Cors since this is the Frontend-facing API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
