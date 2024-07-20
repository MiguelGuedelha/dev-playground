using AspirePlayground.FusionCache;
using AspirePlayground.Typesense;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddRedisDistributedCache("Cache");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add test playground projects related services
builder.Services.AddFusionCacheProjectServices();
builder.Services.AddFusionCacheOpenTelemetry();

builder.Services.AddTypesenseProjectServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("./swagger/v1/swagger.json", "v1");
        o.RoutePrefix = string.Empty;
    });
}

// Map test playground projects related endpoints
app.MapFusionCacheEndpoints();
app.MapTypesenseEndpoints();

app.MapDefaultEndpoints();

app.Run();