using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly;
using ConnectionDb;
using Services;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning); 

    builder.Services.AddHttpClient("PollyHttpClient")
        .AddTransientHttpErrorPolicy(policyBuilder =>
            policyBuilder.WaitAndRetryAsync(
                5, // Numero de reintentos
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            )
        );

builder.Services.AddHttpClient("Bulkhead")
    .AddPolicyHandler(Policy.BulkheadAsync<HttpResponseMessage>(
        maxParallelization: 5,//Numero de aceptados
        maxQueuingActions: 6, //Cola  
        onBulkheadRejectedAsync: context =>
        {
            Console.WriteLine("Bulkhead rechazado: demasiadas solicitudes concurrentes");
            return Task.CompletedTask;
        }
    ));


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Tutorial"))
);


builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<ProductService>();   
builder.Services.AddScoped<OrderService>();
 
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend de Ejemplo de E-Commerce C#",
        Version = "v1",
        Description = "API en base al Ing. Elvin, con orders, services, y demás",
        Contact = new OpenApiContact
        {
            Name = "Johnny Melgar",
            Email = "johnny@unitec.edu"
        }
    });
});


var app = builder.Build();

app.UseHttpsRedirection();

// Endpoint de health check
app.MapHealthChecks("/health");

// Endpoints de controladores
app.MapControllers();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }
}
catch (Exception e) { }

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
        c.RoutePrefix = string.Empty; 
    });
}

app.Run();
