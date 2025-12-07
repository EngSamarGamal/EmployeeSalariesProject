using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using FluentValidation;


public class EmployeeSalariesDtoValidator : AbstractValidator<CreateSalary>
{
	public EmployeeSalariesDtoValidator()
	{

		RuleFor(x => x.EmployeeId)
		.NotEmpty().WithMessage("*");

		RuleFor(x => x.Month)
			.NotEmpty().WithMessage("*");
		RuleFor(x => x.year)
		.NotEmpty().WithMessage("*");

		RuleFor(x => x.BasicSalary)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("الراتب الأساسي يجب أن يكون صفراً أو أكبر.");

		RuleFor(x => x.Allowances)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("البدلات يجب أن تكون صفر أو أكبر.");

		RuleFor(x => x.TotalSalary)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("إجمالي الراتب يجب أن يكون صفراً أو أكبر.");
			
		RuleFor(x => x.WorkDays)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("أيام العمل يجب أن تكون صفر أو أكبر.");

		RuleFor(x => x.AbsentDays)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("أيام الغياب يجب أن تكون صفر أو أكبر.");

		RuleFor(x => x.SickLeaveDays)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("أيام الإجازة المرضية يجب أن تكون صفر أو أكبر.");

		RuleFor(x => x.AnnualLeaveDays)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("أيام الإجازة السنوية يجب أن تكون صفر أو أكبر.");

		RuleFor(x => x.TotalDeductions)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("مجموع الخصم يجب أن يكون صفراً أو أكبر.");

		RuleFor(x => x.TasksAndRewards)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("المكافآت يجب أن تكون صفر أو أكبر.");

		RuleFor(x => x.FinalSalary)
			.NotEmpty().WithMessage("*")
			.GreaterThanOrEqualTo(0).WithMessage("صافي الراتب يجب أن يكون صفراً أو أكبر.");
	
		RuleFor(x => x.Notes)
			.MaximumLength(500).WithMessage("الملاحظات يجب ألا تزيد عن 500 حرف.");
	}
}
