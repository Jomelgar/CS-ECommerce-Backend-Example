using Microsoft.EntityFrameworkCore;
using Services;
using Polly;
using SharedData;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Tutorial")));

builder.Services.AddHttpClient<ProductService>(client =>
{
    client.BaseAddress = new Uri("http://inventoryservice");
}).AddTransientHttpErrorPolicy(policyBuilder =>
            policyBuilder.WaitAndRetryAsync(
                5, // Numero de reintentos
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            )
        );

builder.Services.AddScoped<ProductService>();
builder.Services.AddHealthChecks();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
