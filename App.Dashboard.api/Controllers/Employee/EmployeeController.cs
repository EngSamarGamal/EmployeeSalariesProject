using App.Common.Dtos.Models.API.Employees;

namespace App.Dashboard.api.Controllers.Developers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
			_employeeService = employeeService;
        }

		/// <summary>
		/// Creates a new employee.
		/// Accepts form data including optional logo upload.
		/// </summary>
		/// <param name="dto">Developer creation data (NameEn, NameAr, DescriptionEn, DescriptionAr, Logo)</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Created employee details or error message</returns>
		[HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeDto dto, CancellationToken ct)
        {
            var response = await _employeeService.CreateAsync(dto, ct);
            return Ok(response);
        }



		
		/// <summary>
		/// Gets a employee by Id.
		/// Returns localized employee details.
		/// </summary>
		/// <param name="id">employee Id</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Developer details or not found message</returns>
		[HttpGet("GetAllEmp")]
        public async Task<IActionResult> GetAllEmp( CancellationToken ct)
        {
            var response = await _employeeService.GetAllEmployee(ct);
            return Ok(response);
        }

		/// <summary>
		/// Retrieves a paginated list of employee.
		/// Supports optional search by English or Arabic names,
		/// and optional filter by creation date in format "dd-MM-yyyy".
		/// </summary>
		/// <param name="search">Search keyword</param>
		/// <param name="pageNumber">Page number (default 1)</param>
		/// <param name="pageSize">Page size (default 10)</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Paginated list of employee</returns>
		[HttpGet("paginated")]
        public async Task<IActionResult> GetAllPaginated(
            string? search = null,
           
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            var response = await _employeeService.GetAllPaginatedAsync(
                search, pageNumber, pageSize, ct);

            return Ok(response);
        }
    }
}
