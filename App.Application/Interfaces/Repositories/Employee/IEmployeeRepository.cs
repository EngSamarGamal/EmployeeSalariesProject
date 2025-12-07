using App.Domain.Models.Employees;
using System.Threading.Tasks;

namespace App.Application.Interfaces.Repositories.Developers
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
		Task<bool> ExistsEmployeeNumber(string number);


	}
}
