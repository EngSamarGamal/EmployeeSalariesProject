using App.Application.Interfaces.Repositories.Developers;
using App.Domain.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Implementation.Repositories.Developers
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
		private readonly ApplicationDbContext _Context;


		public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
			_Context = context;
		}

		public async Task<bool> ExistsEmployeeNumber(string number)
		{
			return await _context.employees
				.AnyAsync(e => e.EmployeeNumber == number);
		}

	}
}
