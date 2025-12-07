using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Repositories.Developers;
using App.Application.Services.BaseService;
using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using App.Common.Helpers.ExpressionExtentions;
using App.Common.Helpers.FileManagers;
using App.Common.Response;
using App.Domain.Models.Employees;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.ProjectServer.Client;
using Microsoft.SharePoint.Client.Publishing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Services.Developers
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _developerRepository;
        private readonly IFileManager _fileManager;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IStringLocalizer<EmployeeService> _localizer;

        public EmployeeService(IUnitOfWork unitOfWork,
            IEmployeeRepository developerRepository,
            IFileManager fileManager,
            ILogger<EmployeeService> logger,
            IStringLocalizer<EmployeeService> localizer
           ) : base(unitOfWork, developerRepository)
        {
            _unitOfWork = unitOfWork;
            _developerRepository = developerRepository;
            _fileManager = fileManager;
            _logger = logger;
            _localizer = localizer;
        }
/// <summary>
/// t write error message here with arabic bexous it reflect to user
/// </summary>
/// <param name="dto"></param>
/// <param name="ct"></param>
/// <returns></returns>
		public async Task<IResponseModel> CreateAsync(EmployeeDto dto, CancellationToken ct = default)
		{
			try
			{
				if (dto == null)
					return ResponseModel.Error("البيانات المرسلة غير صحيحة.",400);
				var entity = dto.Adapt<Employee>();

				if (dto.File != null)
				{
					var saved = await _fileManager.SaveToApiWwwrootAsync(dto.File, "EmployeesPhoto", "admin");
					entity.PhotoPath = saved.Url;

				}

				await AddAsync(entity, userId: "system", ct);

				return ResponseModel.Success("تم حفظ الموظف بنجاح", entity);
			}
			catch (Exception ex)
			{
				return ResponseModel.Error($"حدث خطأ أثناء إضافة الموظف: {ex.Message}");
			}
		}


		//public async Task<IResponseModel> DeleteAsync(Guid id, CancellationToken ct = default)
		//{
		//	try
		//	{
		//		var entity = await GetByIdAsync(id, cancellationToken: ct);

		//		if (entity == null)
		//			return ResponseModel.Error("الموظف غير موجود.");

		//		UpdateIsDeleted(entity,"admin");

			

		//		return ResponseModel.Success("تم حذف الموظف بنجاح.");
		//	}
		//	catch (Exception ex)
		//	{
		//		return ResponseModel.Error($"حدث خطأ أثناء الحذف: {ex.Message}");
		//	}
		//}

		public async Task<IResponseModel> GetAllEmployee(CancellationToken ct = default)
		{
			try
			{
				ct.ThrowIfCancellationRequested();

				IEnumerable<Employee> query = await GetAllAsync(true, ct);

				var dtoItems = query.Select(e => new EmpList
				{
					FullName = e.FullName,
					EmployeeId = e.Id,    
					BaseSalary=(decimal)e.Salary,
					job=e.JobTitle,
				}).ToList();

				

				return ResponseModel.Success("data fetched succ", data: dtoItems);
			}
			catch (OperationCanceledException)
			{
				return ResponseModel.Error("Operation cancelled");
			}
			catch (Exception ex)
			{
				return ResponseModel.Error("An error occurred while getting employees" + ex);
			}
		}

		public async Task<IResponseModel> GetAllPaginatedAsync(string? search = null, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
		{
			try
			{
				ct.ThrowIfCancellationRequested();

				IEnumerable<Employee> query = await GetAllAsync(true, ct);

				if (!string.IsNullOrWhiteSpace(search))
				{
					string s = search.Trim();

					query = query.Where(e =>
						
						(!string.IsNullOrEmpty(e.PhoneNumber) && e.PhoneNumber.Contains(s)) ||  // or PhoneNumber
						(!string.IsNullOrEmpty(e.FullName) && e.FullName.Contains(s)) ||
						(!string.IsNullOrEmpty(e.EmployeeNumber) && e.EmployeeNumber.Contains(s))
					);
				}

				var totalCount = query.Count();

				query = query.OrderBy(e => e.Id);

				int safePageNumber = Math.Max(pageNumber, 1);
				int safePageSize = Math.Max(pageSize, 1);

				var items = query
					.Skip((safePageNumber - 1) * safePageSize)
					.Take(safePageSize)
					.ToList();

				var dtoItems = items.Select(e => new EmployeeDto
				{
					FullName = e.FullName,
					EmployeeNumber = e.EmployeeNumber,     // adjust if your entity uses another name
					PhoneNumber = e.PhoneNumber , // try PhoneNumber first, fallback to FullNumber
					Address = e.Address,
					Email = e.Email,
					Nationality = e.Nationality,
					JobTitle = e.JobTitle,
					PassportNumber = e.PassportNumber,
					PassportExpiry = e.PassportExpiry,
					NationalIdNumber = e.NationalIdNumber,
					NationalIdExpiry = e.NationalIdExpiry,
					IdPlaceOfIssue = e.IdPlaceOfIssue,
					Salary = e.Salary,
					BankAccountNumber = e.BankAccountNumber,
					BankName = e.BankName,
					Notes = e.Notes,
					PhotoPath = e.PhotoPath
				}).ToList();

				// Build paginated result
				var paginated = new PaginationResponse<EmployeeDto>
				{
					Result = dtoItems,
					TotalCount = totalCount,
					PageNumber = safePageNumber,
					PageSize = safePageSize,
				};

				return ResponseModel.Success("data fetched succ",data: paginated);
			}
			catch (OperationCanceledException)
			{
				return ResponseModel.Error("Operation cancelled");
			}
			catch (Exception ex)
			{
				return ResponseModel.Error("An error occurred while getting employees"+ ex);
			}
		}

		public async Task<IResponseModel> GetEmployeeByIdAsync(Guid id, CancellationToken ct = default)
		{
			try
			{
				var entity = await GetByIdAsync(id, cancellationToken: ct);

				if (entity == null)
					return ResponseModel.Error("الموظف غير موجود.");

				var dto = entity.Adapt<EmployeeDto>();

				return ResponseModel.Success("تم جلب بيانات الموظف بنجاح.", dto);
			}
			catch (Exception ex)
			{
				return ResponseModel.Error($"حدث خطأ أثناء جلب بيانات الموظف: {ex.Message}");
			}
		}


		public Task<IResponseModel> UpdateAsync(Guid id, EmployeeDto dto, string userId, CancellationToken ct = default)
		{
			throw new NotImplementedException();
		}
	}
}