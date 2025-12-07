using App.MVC.Interfaces;
using App.MVC.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
	options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// ---------- CORS (single policy) ----------
// For development: AllowAnyOrigin is easiest. Replace with specific origin(s) in production.
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy
			.AllowAnyOrigin()   // DEV ONLY - replace with .WithOrigins("http://localhost:5000") in prod
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

// ---------- HttpClient, DI, Validation, Session ----------
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiPoster, ApiPoster>();

builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeSalariesDtoValidator>();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromHours(1);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// ---------- build app ----------
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

// Apply CORS policy between UseRouting and endpoint mapping
app.UseCors("AllowAll");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Employee}/{action=AddEmployee}/{id?}");

app.Run();
