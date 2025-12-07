using App.Common.Dtos.Models.API.Employees;
using App.MVC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

public class EmployeeController : Controller
{
	private readonly IApiPoster _apiPoster;
	

	public EmployeeController(IApiPoster apiPoster)
	{
		_apiPoster = apiPoster;	
	}

	[HttpGet]
	public IActionResult AddEmployee() => View();

	[HttpGet]
	public IActionResult ViewEmployee() => View();

	[HttpPost]
	public async Task<IActionResult> AddEmployee(EmployeeDto model)
	{
		var res = await _apiPoster.CreateEmp("Employee", model);

		TempData["ToastMessage"] = res.Message;

		return RedirectToAction("AddEmployee");
	}
}

public class ApiResponse
{
	public string? Message { get; set; }
}
