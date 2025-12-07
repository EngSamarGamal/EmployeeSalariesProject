using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using App.MVC.Interfaces;
using App.MVC.Service;
using Microsoft.AspNetCore.Mvc;

namespace App.MVC.Controllers
{
	public class EmployeesSalaryController : Controller
	{
		private readonly IApiPoster _ApiPoster;

		public EmployeesSalaryController(IApiPoster apiPoster)
		{
			_ApiPoster = apiPoster;
		}


	

		[HttpGet]
		public IActionResult AddSalaries() => View();
		[HttpGet]
		public IActionResult EditSalaries(Guid salaryId)
		{
			var result =  _ApiPoster.GetSalaryDetialsApi(salaryId).Result;	
			return View(result.Result);
		}

		[HttpGet]
		public IActionResult ViewEmployeeSalary() => View();

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddSalaries(CreateSalary model,CancellationToken ct)
		{
			if (!ModelState.IsValid)
			{
				
				return View(model);
			}

			
				var createResult = await _ApiPoster.CreateEmpSalaryAsync("EmployeesSalary",model,ct);
				
					TempData["ToastMessage"] = "تم حفظ الراتب بنجاح";
					return RedirectToAction("AddSalaries");
				

		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditSalaries(CreateSalary model, CancellationToken ct)
		{
			if (!ModelState.IsValid)
			{

				return View(model);
			}


			var createResult = await _ApiPoster.EditEmpSalaryAsync("EmployeesSalary", model, ct);

			TempData["ToastMessage"] = "تم تعديل الراتب بنجاح";
			return RedirectToAction("EditSalaries");


		}




		[HttpGet]
		public IActionResult PrintSalaries(Guid? employeeId, int? month, int? year)
		{
			ViewBag.EmployeeId = employeeId;
			ViewBag.Month = month;
			ViewBag.Year = year;

			return View();
		}

	}
}
