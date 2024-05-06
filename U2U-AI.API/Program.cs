using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using U2U_AI.Infra;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddServer(new OpenApiServer() { Url = "https://localhost:7250" });
  options.EnableAnnotations();
});

builder.Services.AddDbContext<CourseDbContext>(options =>
{
  string connectionString = builder.Configuration.GetConnectionString("U2UCourseDb");
  options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Yaml API Spec"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
