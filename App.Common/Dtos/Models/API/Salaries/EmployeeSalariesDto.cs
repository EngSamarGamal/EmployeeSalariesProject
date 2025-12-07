using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Models.Employees;
using FluentValidation;

namespace App.Common.Dtos.Models.API.Salaries
{
    public class EmployeeSalariesDto
    {
        public Guid EmployeeId { get; set; }
        public Guid SalaryId { get; set; }

        public Employee Employee { get; set; }

        public int Month { get; set; }
        public int year { get; set; }
        public decimal BasicSalary { get; set; }

        public decimal Allowances { get; set; }

        public decimal TotalSalary { get; set; }

        public int WorkDays { get; set; }
        public int AbsentDays { get; set; }
        public int SickLeaveDays { get; set; }
        public int AnnualLeaveDays { get; set; }

        public decimal TotalDeductions { get; set; }

        public decimal TasksAndRewards { get; set; }

        public decimal NetSalary { get; set; }

    }
}
