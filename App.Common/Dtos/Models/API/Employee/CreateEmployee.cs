using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Dtos.Models.API.Developers
{
    public class CreateEmployee
    {
        public string? FullName { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Nationality { get; set; }
        public string? JobTitle { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? PassportExpiry { get; set; }
        public string? NationalIdNumber { get; set; }
        public DateTime? NationalIdExpiry { get; set; }
        public string? IdPlaceOfIssue { get; set; }
        public decimal? Salary { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? Notes { get; set; }
        public string? PhotoPath { get; set; }
    }
}
