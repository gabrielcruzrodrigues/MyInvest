using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Disable the automatic redirect to Https
builder.Services.AddHttpsRedirection(options => options.HttpsPort = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string postgreSqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyInvestContext>(options =>
    options.UseNpgsql(postgreSqlConnection));

var app = builder.Build();

//update the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MyInvestContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyInvestAPI");
            c.RoutePrefix = "api-doc";
        });
    }

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
