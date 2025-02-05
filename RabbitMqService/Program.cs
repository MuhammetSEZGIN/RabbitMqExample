using Microsoft.OpenApi.Models;
using RabbitMqService.Interfaces;
using RabbitMqService.RabbitMq;
using RabbitMqService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "RabbitMQ Service API", 
        Version = "v1" 
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMq"));


builder.Services.AddOpenApi();
builder.Services.AddSingleton<IRabbitMQManager, RabbitMQManager>();
builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
builder.Services.AddHostedService<RabbitMQConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RabbitMQ Service API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseAuthorization();
app.MapControllers();

app.UseRouting();

app.UseAuthorization();
app.Run();
