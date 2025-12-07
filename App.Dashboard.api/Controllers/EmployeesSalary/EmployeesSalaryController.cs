using App.Application.Services.EmployeeSalariesService;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Dashboard.api.Controllers.EmployeesSalary
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesSalaryController : ControllerBase
	{
		private readonly IEmployeeSalaryService _employeeSalaryService;

		public EmployeesSalaryController(IEmployeeSalaryService  employeeSalaryService)
		{
			_employeeSalaryService = employeeSalaryService;
		}
		[HttpPost]
		public async Task<IActionResult> Create( CreateSalary dto, CancellationToken ct)
		{
			var response = await _employeeSalaryService.CreateAsync(dto, ct);
			return Ok(response);
		}
		[HttpPut]
		public async Task<IActionResult> Edit(CreateSalary dto, CancellationToken ct)
		{
			var response = await _employeeSalaryService.EditAsync(dto, ct);
			return Ok(response);
		}
		[HttpGet("GetAllEmpSalary")]
		public async Task<IActionResult> GetAllEmpSalary(Guid? employeeId = null, int? month = null, int? year = null, CancellationToken ct = default)
		{
			var response = await _employeeSalaryService.GetSalariesAsync(employeeId,month,year,ct);
			return Ok(response);
		}
		[HttpGet("GetSalaryDetials")]
		public async Task<IActionResult> GetSalaryDetials(Guid SalaryId , CancellationToken ct = default)
		{
			var response = await _employeeSalaryService.GetSalaryDetials(SalaryId,  ct);
			return Ok(response);
		}
		[HttpDelete("Delete")]

		public async Task<IResponseModel> Delete(Guid SalaryId, CancellationToken ct = default)
		{
			var response = await _employeeSalaryService.DeleteSalary(SalaryId, ct);
			return response;	
		}

	}
}
