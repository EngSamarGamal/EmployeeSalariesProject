using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using App.Common.Response;

namespace App.MVC.Interfaces
{
	public interface IApiPoster
	{
		Task<IResponseModel> CreateEmp(string module, EmployeeDto model, CancellationToken ct = default);
		Task<IResponseModel> CreateEmpSalaryAsync(string module, CreateSalary model, CancellationToken ct = default);
		Task<IResponseModel> GetSalaryDetialsApi(Guid SalaryId, CancellationToken ct = default);
		Task<IResponseModel> EditEmpSalaryAsync(string module, CreateSalary model, CancellationToken ct = default);

	}
}

