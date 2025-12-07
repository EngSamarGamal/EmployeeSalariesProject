using App.Common.Dtos.Models.API.Employees;
using FluentValidation;


public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
	public EmployeeDtoValidator( )
	{
		RuleLevelCascadeMode = CascadeMode.Stop;

		RuleFor(x => x.EmployeeNumber)
		   .NotEmpty().WithMessage("رقم الموظف مطلوب.")
		   //.MustAsync(async (number, cancellation) =>
		   //{
			  // return !await repo.ExistsEmployeeNumber(number);
		   //})
		   .WithMessage("رقم الموظف مستخدم بالفعل.");
		RuleFor(x => x.FullName)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.EmployeeNumber)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.PhoneNumber)
			.NotEmpty()
			.WithMessage("*")
			.Matches(@"^(?:\+971|971|0)?5[0-9]{8}$")
			.WithMessage("Phone number must be a valid UAE number (e.g., 05XXXXXXXX).");

		RuleFor(x => x.Address)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.Email)
			.NotEmpty()
			.WithMessage("*")
			.EmailAddress()
			.WithMessage("الايميل غير صحيح");

		RuleFor(x => x.Nationality)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.JobTitle)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.PassportNumber)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.PassportExpiry)
			.NotEmpty()
			.WithMessage("*")
			.Must(d => d.HasValue && d.Value.Date > DateTime.Today)
			.WithMessage("ناريخ جواز السفر يجب ان يكون قادم");

		RuleFor(x => x.NationalIdNumber)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.NationalIdExpiry)
			.NotEmpty()
			.WithMessage("*")
			.Must(d => d.HasValue && d.Value.Date > DateTime.Today)
			.WithMessage("تاريخ انتهاء الهوية يجب أن يكون في المستقبل.");

		RuleFor(x => x.IdPlaceOfIssue)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.Salary)
			.NotEmpty()
			.WithMessage("*")
			.GreaterThanOrEqualTo(0)
			.WithMessage("المرتب يجب ان يكون قيمة موجبه");

		RuleFor(x => x.BankAccountNumber)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.BankName)
			.NotEmpty()
			.WithMessage("*");

		RuleFor(x => x.Notes)
			.NotEmpty()
			.WithMessage("*");
	}
}
