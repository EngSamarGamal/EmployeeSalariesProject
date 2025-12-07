using App.Domain.Models.Base;
using App.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Salaries
{
	public class EmployeeSalaries:BaseEntity
	{
		public Guid EmployeeId { get; set; }

		[ForeignKey(nameof(EmployeeId))]
		public Employee Employee { get; set; }

		public int Month { get; set; }
		public int year { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal BasicSalary { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Allowances { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalSalary { get; set; }

		public int WorkDays { get; set; }
		public int AbsentDays { get; set; }
		public int SickLeaveDays { get; set; }
		public int AnnualLeaveDays { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalDeductions { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal TasksAndRewards { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal NetSalary { get; set; }

		

	}
}
