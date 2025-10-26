using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskPulse.Core.DTOs;
using TaskPulse.Core.Entities;
using TaskPulse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options=>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/products", async (AppDbContext db) =>
{
    return await db.Products.ToListAsync();
});

app.MapGet("api/products/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Products.FindAsync(id)
        is Product product
            ? Results.Ok(product)
            : Results.NotFound();
});

app.MapPost("api/prodcts", async (ProductDto dto, AppDbContext db, IMapper mapper) =>
{
    var product = mapper.Map<Product>(dto);
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});

app.MapPut("api/products/{id:int}", async (int id, ProductDto dto, AppDbContext db, IMapper mapper) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null) return Results.NotFound();

    mapper.Map(dto, product);

    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("api/product/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
