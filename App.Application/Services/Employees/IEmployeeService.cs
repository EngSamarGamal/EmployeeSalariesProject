using App.Application.Services.BaseService;
using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Response;
using App.Domain.Models.Employees;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Services.Developers
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        Task<IResponseModel> CreateAsync(EmployeeDto dto,  CancellationToken ct = default);
        public Task<IResponseModel> GetAllPaginatedAsync(string? search = null, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
		Task<IResponseModel> GetAllEmployee(CancellationToken ct = default);


	}
}
