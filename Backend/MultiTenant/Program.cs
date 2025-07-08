using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Api.Middleware;
using MultiTenant.Application.Interfaces;
using MultiTenant.Application.Mappings;
using MultiTenant.Application.Services;
using MultiTenant.Infrastructure;
using MultiTenant.Infrastructure.Tenant;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddInfrastructure(builder.Configuration);
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };
    });
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMultiTenantApp",
        policy => policy
            .WithOrigins("http://localhost:4200") // Angular dev URL
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // shows 404 routing info
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("AllowMultiTenantApp");

app.UseAuthentication();

app.UseMiddleware<TenantMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
