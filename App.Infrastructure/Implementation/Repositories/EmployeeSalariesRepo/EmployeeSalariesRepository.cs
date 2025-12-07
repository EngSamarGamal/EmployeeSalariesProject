using App.Application.Interfaces.Repositories.Developers;
using App.Application.Interfaces.Repositories.EmployeesSalaryRepo;
using App.Domain.Models.Employees;
using App.Domain.Models.Salaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Implementation.Repositories.EmployeeSalariesRepo
{
	public class EmployeeSalariesRepository : BaseRepository<EmployeeSalaries>, IEmployeesSalaryRepository
	{
		private readonly ApplicationDbContext _Context;	
		public EmployeeSalariesRepository(ApplicationDbContext context) : base(context)
		{
			_Context = context;
		}	
	}
}
