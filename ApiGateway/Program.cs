using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add environment variables support (overrides appsettings.json)
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGateway",
        policy =>
        {
            policy.AllowAnyOrigin()  // Or specific origins like "http://localhost:5000"
                  .AllowAnyMethod()  // Includes OPTIONS, POST
                  .AllowAnyHeader();
        });
});
var jwtKey = builder.Configuration["AppSettings:Key"];
var jwtIssuer = builder.Configuration["AppSettings:issuer"];
var jwtAudience = builder.Configuration["AppSettings:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException(
        "JWT configuration is missing. Please configure the following:\n" +
        "- AppSettings:Key (JWT secret key)\n" +
        "- AppSettings:issuer (JWT issuer)\n" +
        "- AppSettings:Audience (JWT audience)\n\n" +
        "For local development, use 'dotnet user-secrets set \"AppSettings:Key\" \"your-secret-key\"'\n" +
        "For production, set environment variables: AppSettings__Key, AppSettings__issuer, AppSettings__Audience"
    );
}

// Add JWT Authentication for protected routes
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero 
        };
    });

var app = builder.Build();
Console.WriteLine(DateTime.UtcNow);

if(app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseRouting();
app.UseCors("AllowGateway");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


// Use Ocelot middleware for routing
await app.UseOcelot();

app.Run();