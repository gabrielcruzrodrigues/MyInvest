using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyInvestAPI.Data;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//----------------------------- Cors -----------------------------
var OriginsWithAllowedAccess = "OriginsWithAllowedAccess";

builder.Services.AddCors(options =>
    options.AddPolicy(name: OriginsWithAllowedAccess,
    policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:9090")
            .WithHeaders("Content-Type")
            .WithMethods("*");
    })
);

// Disable the automatic redirect to Https
builder.Services.AddHttpsRedirection(options => options.HttpsPort = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------- Autenticação e autorização ---------------------
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MyInvestContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Chave secreta inválida");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; //mudar para true em produção
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey);
    };
});

//----------------- Configuração Da DI no container -----------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPurseRepository, PurseRepository>();
builder.Services.AddScoped<IActiveRepository, ActiveRepository>();


//----------------------------- Database -----------------------------
string postgreSqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyInvestContext>(options =>
    options.UseNpgsql(postgreSqlConnection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyInvestAPI");
        c.RoutePrefix = "swagger";
    });
    app.ConfigureExceptionHandler();
}

if (app.Environment.IsProduction())
{
    app.ActiveUpdateDatabaseMigrations();
}

//app.UseHttpsRedirection();
app.UseCors(OriginsWithAllowedAccess);

app.UseAuthorization();

app.MapControllers();

app.Run();