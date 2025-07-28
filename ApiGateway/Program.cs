using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend",
        policy =>
        {
            policy.WithOrigins("https://localhost:8080")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add JWT Authentication for protected routes
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Key"]!)),
            ValidateLifetime = true
        };
    });

var app = builder.Build();
Console.WriteLine(DateTime.UtcNow);

//app.UseCors("AllowViteFrontend");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Use Ocelot middleware for routing
await app.UseOcelot();

app.Run();