using App.Common.Dtos.Models.API.Developers;
using App.Common.Dtos.Models.API.Employees;
using App.Common.Dtos.Models.API.Salaries;
using App.Common.Response;
using App.MVC.Interfaces;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;

namespace App.MVC.Service
{
	public class ApiPoster : IApiPoster
	{
		private readonly IHttpClientFactory _httpFactory;
		private readonly string _baseUrl;
		private readonly ILogger<ApiPoster> _logger;

		public ApiPoster(IHttpClientFactory httpFactory, IConfiguration config, ILogger<ApiPoster> logger)
		{
			_httpFactory = httpFactory;
			_logger = logger;
			_baseUrl = (config["ApiSettings:BaseUrl"] ?? "http://localhost:7037/api/").TrimEnd('/') + "/";
		}

		public async Task<IResponseModel> CreateEmp(string module, EmployeeDto model, CancellationToken ct = default)
		{
			string url = _baseUrl + module; // e.g. http://localhost:7037/api/Employee
			_logger.LogInformation("Posting to {Url}", url);

			try
			{
				using var form = new MultipartFormDataContent();

				void AddString(string name, string? value)
				{
					if (!string.IsNullOrEmpty(value))
						form.Add(new StringContent(value), name);
				}

				AddString("NationalIdNumber", model.NationalIdNumber);
				AddString("NationalIdExpiry", model.NationalIdExpiry?.ToString("o"));
				AddString("PassportNumber", model.PassportNumber);
				AddString("PassportExpiry", model.PassportExpiry?.ToString("o"));
				AddString("FullName", model.FullName);
				AddString("PhoneNumber", model.PhoneNumber);
				AddString("Email", model.Email);
				AddString("Address", model.Address);
				AddString("JobTitle", model.JobTitle);
				// ensure invariant culture for decimals
				AddString("Salary", model.Salary.ToString());
				AddString("BankName", model.BankName);
				AddString("BankAccountNumber", model.BankAccountNumber);
				AddString("EmployeeNumber", model.EmployeeNumber);
				AddString("Nationality", model.Nationality);
				AddString("Notes", model.Notes);
				AddString("IdPlaceOfIssue", model.IdPlaceOfIssue);

				// file: use Memory copy (safe). For large files consider StreamContent but DO NOT dispose stream before PostAsync completes.
				if (model.File != null && model.File.Length > 0)
				{
					using var ms = new MemoryStream();
					await model.File.CopyToAsync(ms, ct);
					var bytes = ms.ToArray();
					var fileContent = new ByteArrayContent(bytes);
					fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.File.ContentType ?? "application/octet-stream");
					form.Add(fileContent, "File", model.File.FileName);
				}

				var client = _httpFactory.CreateClient();
				using var response = await client.PostAsync(url, form, ct);

				var respText = await response.Content.ReadAsStringAsync(ct);

				if (response.IsSuccessStatusCode)
				{
					try
					{
						var apiResp = JsonSerializer.Deserialize<ApiResponse>(respText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
						return ResponseModel.Success(apiResp?.Message ?? "تم حفظ الموظف بنجاح");
					}
					catch
					{
						return ResponseModel.Success(string.IsNullOrWhiteSpace(respText) ? "تم حفظ الموظف بنجاح" : respText);
					}
				}
				else
				{
					try
					{
						var apiResp = JsonSerializer.Deserialize<ApiResponse>(respText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
						return ResponseModel.Error(apiResp?.Message ?? $"API returned {(int)response.StatusCode} {response.ReasonPhrase}");
					}
					catch
					{
						return ResponseModel.Error($"API returned {(int)response.StatusCode} {response.ReasonPhrase}");
					}
				}
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "HttpRequestException posting to {Module}", module);
				return ResponseModel.Error($"Cannot connect to API. ({ex.Message})");
			}
			catch (TaskCanceledException)
			{
				return ResponseModel.Error("Request timed out while contacting API.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error posting to {Module}", module);
				return ResponseModel.Error("Unexpected error: " + ex.Message);
			}
		}

		// 2) New method: send JSON to EmployeesSalary endpoint
		public async Task<IResponseModel> CreateEmpSalaryAsync(string module, CreateSalary model, CancellationToken ct = default)
		{
			string url = _baseUrl + module; // e.g. http://localhost:7037/api/EmployeesSalary
			_logger.LogInformation("Posting JSON to {Url}", url);

			try
			{
				var client = _httpFactory.CreateClient();

				// Configure serializer to use camelCase (to match your API's JSON names)
				var jsonOptions = new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					PropertyNameCaseInsensitive = true
				};

				var json = JsonSerializer.Serialize(model, jsonOptions);
				using var content = new StringContent(json, Encoding.UTF8, "application/json");

				using var response = await client.PostAsync(url, content, ct);
				var respText = await response.Content.ReadAsStringAsync(ct);

				if (response.IsSuccessStatusCode)
				{
					try
					{
						var apiResp = JsonSerializer.Deserialize<ApiResponse>(respText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
						return ResponseModel.Success(apiResp?.Message ?? "تم حفظ راتب الموظف بنجاح");
					}
					catch
					{
						// fallback if response is plain text or not in ApiResponse shape
						return ResponseModel.Success(string.IsNullOrWhiteSpace(respText) ? "تم حفظ راتب الموظف بنجاح" : respText);
					}
				}
				else
				{
					try
					{
						var apiResp = JsonSerializer.Deserialize<ApiResponse>(respText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
						return ResponseModel.Error(apiResp?.Message ?? $"API returned {(int)response.StatusCode} {response.ReasonPhrase}");
					}
					catch
					{
						return ResponseModel.Error($"API returned {(int)response.StatusCode} {response.ReasonPhrase}");
					}
				}
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "HttpRequestException posting to {Module}", module);
				return ResponseModel.Error($"Cannot connect to API. ({ex.Message})");
			}
			catch (TaskCanceledException)
			{
				return ResponseModel.Error("Request timed out while contacting API.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error posting to {Module}", module);
				return ResponseModel.Error("Unexpected error: " + ex.Message);
			}
		}

		public async Task<IResponseModel> GetSalaryDetialsApi( Guid SalaryId, CancellationToken ct = default)
		{
			var model = new CreateSalary();
			string urlApi = _baseUrl + "EmployeesSalary"; // e.g. http://localhost:7037/api/EmployeesSalary


			try
			{
				var client = _httpFactory.CreateClient();
				var url = $"{urlApi}/GetSalaryDetials?SalaryId={SalaryId}";

				using var resp = await client.GetAsync(url);
				var text = await resp.Content.ReadAsStringAsync();

				if (!resp.IsSuccessStatusCode)
				{
					return ResponseModel.Error("فشل جلب بيانات الراتب ");

				}

				// parse JSON in flexible way
				var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
				using var doc = JsonDocument.Parse(text);
				var root = doc.RootElement;

				// root.result contains our object (based on example you provided)
				if (!root.TryGetProperty("result", out var resultEl) || resultEl.ValueKind == JsonValueKind.Null)
				{
					return ResponseModel.Error("لا توجد بيانات لهذا الراتب. ");

				}

				// helper local funcs to read properties safely
				static Guid ReadGuid(JsonElement el, string name)
				{
					if (el.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String && Guid.TryParse(p.GetString(), out var g))
						return g;
					return Guid.Empty;
				}
				static string ReadString(JsonElement el, string name)
				{
					if (el.TryGetProperty(name, out var p) && p.ValueKind != JsonValueKind.Null)
						return p.ToString().Trim('"');
					return string.Empty;
				}
				static int ReadInt(JsonElement el, string name)
				{
					if (el.TryGetProperty(name, out var p))
					{
						if (p.ValueKind == JsonValueKind.Number && p.TryGetInt32(out var v)) return v;
						if (p.ValueKind == JsonValueKind.String && int.TryParse(p.GetString(), out var vv)) return vv;
					}
					return 0;
				}
				static decimal ReadDecimal(JsonElement el, string name)
				{
					if (el.TryGetProperty(name, out var p))
					{
						if (p.ValueKind == JsonValueKind.Number && p.TryGetDecimal(out var v)) return v;
						if (p.ValueKind == JsonValueKind.String && decimal.TryParse(p.GetString(), out var vv)) return vv;
					}
					return 0m;
				}

				// map data -> model
				model.SalaryId = ReadGuid(resultEl, "salaryId");
				// fall back to salaryId property name variants
				if (model.SalaryId == Guid.Empty)
					model.SalaryId = ReadGuid(resultEl, "SalaryId");

				model.EmployeeId = ReadGuid(resultEl, "employeeId");

				model.Month = ReadInt(resultEl, "month");
				model.year = ReadInt(resultEl, "year");

				model.BasicSalary = ReadDecimal(resultEl, "basicSalary");
				model.Allowances = ReadDecimal(resultEl, "allowances");
				model.TotalSalary = ReadDecimal(resultEl, "totalSalary");

				model.WorkDays = ReadInt(resultEl, "workDays");
				model.AbsentDays = ReadInt(resultEl, "absentDays");
				model.SickLeaveDays = ReadInt(resultEl, "sickLeaveDays");
				model.AnnualLeaveDays = ReadInt(resultEl, "annualLeaveDays");

				model.TotalDeductions = ReadDecimal(resultEl, "totalDeductions");
				model.TasksAndRewards = ReadDecimal(resultEl, "tasksAndRewards");
				model.FinalSalary = ReadDecimal(resultEl, "netSalary"); // final/net

				// job and employee full name (nested employee object)
				if (resultEl.TryGetProperty("employee", out var empEl) && empEl.ValueKind == JsonValueKind.Object)
				{
					// job
					var jobTitle = ReadString(empEl, "jobTitle");
					// if your view model has 'job' property (string), set via ViewData or ModelBinder; here model has job?
					// you used asp-for="job" in the view; set it via ViewData since CreateSalary may have 'job' property:
					// assuming CreateSalary has 'job' property:
					try
					{
						// if CreateSalary has property 'job' set it via reflection
						var prop = model.GetType().GetProperty("job");
						if (prop != null && prop.CanWrite)
							prop.SetValue(model, jobTitle);
					}
					catch { /* ignore */ }

					// ensure the employee exists in dropdown list: you populated it earlier; if not, you can add via ViewBag
					var empIdStr = ReadString(empEl, "id");
					var empFullName = ReadString(empEl, "fullName");
					
				}

				return ResponseModel.Success("تم تحميل البيانات بنجاح ",model);

			}
			catch (Exception ex)
			{
				return ResponseModel.Error("حدث خطأ أثناء جلب بيانات الراتب: \" + ex.Message ");
			}
		}
	}
}
