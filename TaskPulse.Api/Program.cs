using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskPulse.Api.Mapping;
using TaskPulse.Core.DTOs;
using TaskPulse.Core.Entities;
using TaskPulse.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskPulse.Infrastructure;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите jwt token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAutoMapper(typeof(ProductProfile));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<IdentityDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddDbContext<AppDbContext>(options=>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var cfg = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = cfg["Issuer"],
            ValidateAudience = true,
            ValidAudience = cfg["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(cfg["Key"]!)
            ),
            ValidateLifetime = true,
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Основная БД (Products)
    var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (app.Environment.IsDevelopment())
    {
        // Миграции для продуктов
        appDb.Database.Migrate();
        DbInitializer.Seed(appDb);
    }

    // Identity БД (Users)
    var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    if (app.Environment.IsDevelopment())
    {
        // Миграции для пользователей
        identityDb.Database.Migrate();
        // Здесь позже добавим сид администратора
    }
}

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
}).RequireAuthorization();

app.MapGet("api/products/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Products.FindAsync(id)
        is Product product
            ? Results.Ok(product)
            : Results.NotFound();
}).RequireAuthorization(); 

app.MapPost("api/prodcts", async (ProductDto dto, AppDbContext db, IMapper mapper) =>
{
    var product = mapper.Map<Product>(dto);
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
}).RequireAuthorization();

app.MapPut("api/products/{id:int}", async (int id, ProductDto dto, AppDbContext db, IMapper mapper) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null) return Results.NotFound();

    mapper.Map(dto, product);

    await db.SaveChangesAsync();
    return Results.Ok(product);
}).RequireAuthorization();

app.MapDelete("api/product/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

app.MapGet("/auth/users", async (IdentityDbContext db) =>
{
    return Results.Ok(await db.Users.ToListAsync());
});

//mapping auth
app.MapPost("/auth/register", async (RegisterRequest request, IPasswordHasher<ApplicationUser> hasher, IdentityDbContext db) =>
{
    if (await db.Users.AnyAsync(u=>u.Email == request.Email))
        return Results.BadRequest("User already exists");
    var user = new ApplicationUser
    {
        Email = request.Email,
    };
    //хешируем пароль
    user.PasswordHash = hasher.HashPassword(user, request.Password);
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok("Users registgred");
});

app.MapPost("/auth/login", async (LoginRequest request, IPasswordHasher<ApplicationUser> hasher, IdentityDbContext db, JwtTokenGenerator jwt) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u=>u.Email == request.email);
    if (user is null)
        return Results.Unauthorized();

    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
    if (result == PasswordVerificationResult.Failed)
        return Results.Unauthorized();

    var token = jwt.Generate(user);
    return Results.Ok(new AuthResponse(token));
});

app.Run();
