using App.Application.Interfaces.Repositories.EmployeesSalaryRepo;
using App.Application.Services.BaseService;
using App.Application.Services.EmployeeSalariesService;
using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using App.Common.Response;
using App.Domain.Models.Employees;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Services.EmployeeSalaries
{
	public class EmployeeSalaryService : BaseService<Domain.Models.Salaries.EmployeeSalaries>, IEmployeeSalaryService
	{
		private readonly IUnitOfWork _UnitOfWork;
		private readonly IEmployeesSalaryRepository _repository;
		public EmployeeSalaryService(IUnitOfWork unitOfWork, Interfaces.Repositories.EmployeesSalaryRepo.IEmployeesSalaryRepository repository) : base(unitOfWork, repository)
		{
			_UnitOfWork = unitOfWork;
			_repository = repository;
		}

		public async Task<IResponseModel> CreateAsync(CreateSalary dto, CancellationToken ct = default)
		{
			try
			{
				if (dto == null)
					return ResponseModel.Error("البيانات المرسلة غير صحيحة.", 400);
				var entity = dto.Adapt<Domain.Models.Salaries.EmployeeSalaries>();



				await AddAsync(entity, userId: "system", ct);

				return ResponseModel.Success("تم حفظ راتب الموظف بنجاح", entity);
			}
			catch (Exception ex)
			{
				return ResponseModel.Error($"حدث خطأ أثناء إضافة راتب الموظف: {ex.Message}");
			}
		}

		public async Task<IResponseModel> DeleteSalary(Guid SalaryId, CancellationToken ct = default)
		{
			try
			{
				var entity = await GetByIdAsync(SalaryId, cancellationToken: ct);

				if (entity == null)
					return ResponseModel.Error("البيانات غير موجود.");

				UpdateIsDeleted(entity, "admin");



				return ResponseModel.Success("تم حذف الموظف بنجاح.");
			}
			catch (Exception ex)
			{
				return ResponseModel.Error($"حدث خطأ أثناء الحذف: {ex.Message}");
			}
		}		

		public async Task<IResponseModel> GetSalariesAsync(Guid? employeeId = null, int? month = null, int? year = null, CancellationToken ct = default)
		{
			try
			{
				string[] includes= new string[] { "Employee" };	
				// input validation: optional but useful
				if (month.HasValue && (month.Value < 1 || month.Value > 12))
					return ResponseModel.Error("قيمة الشهر غير صحيحة. يجب أن تكون بين 1 و 12.");

				if (year.HasValue && (year.Value < 1900 || year.Value > DateTime.UtcNow.Year + 5))
					return ResponseModel.Error("قيمة السنة غير صحيحة.");

				// get all (assumed existing method). Make sure it supports cancellation.
				IEnumerable<Domain.Models.Salaries.EmployeeSalaries> query = await GetAllAsync(includes,true);

				if (employeeId.HasValue)
					query = query.Where(s => s.EmployeeId == employeeId.Value);

				if (month.HasValue)
					query = query.Where(s => s.Month == month.Value);

				if (year.HasValue)
					query = query.Where(s => s.year == year.Value);

			

				var list = query
					.OrderByDescending(s => s.year).ThenByDescending(s => s.Month)
					.Select(s => new EmployeeSalariesDto
					{
						EmployeeId = s.EmployeeId,
						// guard against null navigation property
						Employee = s.Employee == null
							? new Employee { Id = s.EmployeeId, FullName = string.Empty, JobTitle = string.Empty }
							: new Employee
							{
								Id = s.Employee.Id,
								FullName = s.Employee.FullName ?? string.Empty,
								JobTitle = s.Employee.JobTitle ?? string.Empty
							},
						Month = s.Month,
						year = s.year,
						SalaryId=s.Id,
						BasicSalary = s.BasicSalary,
						Allowances = s.Allowances,
						TotalSalary = s.TotalSalary,
						WorkDays = s.WorkDays,
						AbsentDays = s.AbsentDays,
						SickLeaveDays = s.SickLeaveDays,
						AnnualLeaveDays = s.AnnualLeaveDays,
						TotalDeductions = s.TotalDeductions,
						TasksAndRewards = s.TasksAndRewards,
						NetSalary = s.NetSalary != 0
							? s.NetSalary
							: ((s.TotalSalary) - (s.TotalDeductions) + (s.TasksAndRewards))
					})
					.ToList();

				return ResponseModel.Success("تم تحميل البيانات", list);
			}
			catch (OperationCanceledException)
			{
				// cancellation requested
				return ResponseModel.Error("تم إلغاء العملية.");
			}
			catch (Exception ex)
			{

				// return friendly message
				return ResponseModel.Error("حدث خطأ أثناء تحميل بيانات الرواتب. حاول مرة أخرى أو تواصل مع الدعم.");
			}
		}

		public async Task<IResponseModel> GetSalaryDetials(Guid SalaryId, CancellationToken ct = default)
		{
			string[] includes = new string[] { "Employee" };	
			try
			{
				var entity = await GetByIdAsync(SalaryId, includes, cancellationToken: ct);

				if (entity == null)
					return ResponseModel.Error("الموظف غير موجود.");

				var dto = new EmployeeSalariesDto
				{
					EmployeeId = entity.EmployeeId,
					Employee = new Employee
					{
						Id = entity.Employee.Id,
						FullName = entity.Employee.FullName ?? string.Empty,
						JobTitle = entity.Employee.JobTitle ?? string.Empty	
					},
					Month = entity.Month,
					year = entity.year,
					SalaryId = entity.Id,
					BasicSalary = entity.BasicSalary,
					Allowances = entity.Allowances,
					TotalSalary = entity.TotalSalary,
					WorkDays = entity.WorkDays,
					AbsentDays = entity.AbsentDays,
					SickLeaveDays = entity.SickLeaveDays,
					AnnualLeaveDays = entity.AnnualLeaveDays,
					TotalDeductions = entity.TotalDeductions,
					TasksAndRewards = entity.TasksAndRewards,
					NetSalary = entity.NetSalary != 0
						? entity.NetSalary
						: ((entity.TotalSalary) - (entity.TotalDeductions) + (entity.TasksAndRewards))
				};
				return ResponseModel.Success("تم جلب بيانات الموظف بنجاح.", dto);
			}
			catch (Exception ex)
			{
				return ResponseModel.Error($"حدث خطأ أثناء جلب بيانات الموظف: {ex.Message}");
			}
		}
	}

}


