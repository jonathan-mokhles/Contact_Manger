using ServicesContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPeopleServices, PersonService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PersonDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

var app = builder.Build();


Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot",wkhtmltopdfRelativePath:"Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
