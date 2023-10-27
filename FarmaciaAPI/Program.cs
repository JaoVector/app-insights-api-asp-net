using Azure.Storage.Blobs;
using FarmaciaAPI.Context;
using FarmaciaAPI.Middlewares;
using FarmaciaAPI.Repository.Classes;
using FarmaciaAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<FContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("FarmConnection")));
builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration.GetConnectionString("ConnectionStringBlob"));
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware(typeof(MappingErrorMiddleware));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
