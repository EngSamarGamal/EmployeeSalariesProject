using App.Application.Services.BaseService;
using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using App.Common.Response;
using App.Domain.Models.Salaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Services.EmployeeSalariesService	
{
	public interface IEmployeeSalaryService:IBaseService<Domain.Models.Salaries.EmployeeSalaries>
	{
		Task<IResponseModel> CreateAsync(CreateSalary dto, CancellationToken ct = default);
		/// <summary>
		/// Get salaries filtered by optional employeeId, month and year.
		/// If a parameter is null it will not be used as a filter.
		/// </summary>
		Task<IResponseModel> GetSalariesAsync(
			Guid? employeeId = null,
			int? month = null,
			int? year = null,
			CancellationToken ct = default);
		Task<IResponseModel> DeleteSalary(Guid SalaryId, CancellationToken ct = default);
		Task<IResponseModel> GetSalaryDetials(Guid SalaryId, CancellationToken ct = default);

	}
}
