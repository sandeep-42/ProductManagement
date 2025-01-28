using AutoMapper;
using ProductManagementAPI.AutoMapper;
using FluentValidation.AspNetCore;
using ProductManagementAPI.Validators;
using FluentValidation;
using ProductManagementAPI.Data;
using ProductManagementAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductDbContext, ProductDbContext>();
builder.Services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AutoMapper with the profile
builder.Services.AddAutoMapper(typeof(ProductAutoMapperProfile));

// Making sure that AutoMapper configurations are valid
var mapperConfig = new MapperConfiguration(cfg => 
{
    cfg.AddProfile<ProductAutoMapperProfile>();
});
mapperConfig.AssertConfigurationIsValid();

// Add Fluent Validations
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Register Validators
builder.Services.AddValidatorsFromAssemblyContaining<ProductModelValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
