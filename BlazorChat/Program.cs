using BlazorChat.Components;
using BlazorChat.Infra;
using BlazorChat.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Azure;
using System.Configuration;
using Azure.Search.Documents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ChatService>();

builder.Services.AddDbContext<CourseDbContext>(options =>
{
  string connectionString = builder.Configuration.GetConnectionString("U2UCourseDb");
  options.UseSqlServer(connectionString);
});

builder.Services.AddAzureClients(options =>
{
  options.AddSearchClient(builder.Configuration.GetSection("AzureAISearch"));
});

builder.Services.AddQuickGridEntityFrameworkAdapter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
