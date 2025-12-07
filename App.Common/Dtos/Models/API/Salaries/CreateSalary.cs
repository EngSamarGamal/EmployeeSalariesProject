using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Dtos.Models.API.Salaries
{
    public class CreateSalary
    {
        public Guid ?SalaryId { get; set; }

        public Guid EmployeeId { get; set; }
        public string ?job { get; set; }



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

        public decimal FinalSalary { get; set; }


        public string ?Notes { get; set; }
    }

    public class EmpList
    {
        public Guid EmployeeId { get; set; }
        public string FullName { get; set; }
        public string job { get; set; }

        public decimal BaseSalary { get; set; }


    }
}
