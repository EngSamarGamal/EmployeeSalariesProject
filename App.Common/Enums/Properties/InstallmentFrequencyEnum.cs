using System.ComponentModel.DataAnnotations;

namespace App.Common.Enums.Properties
{
    /// <summary>
    /// Frequency of installment payments.
    /// </summary>
    public enum InstallmentFrequencyEnum
    {
        [Display(Name = "Monthly", Description = "شهري")]
        Monthly = 0,

        [Display(Name = "Quarterly", Description = "ربع سنوي")]
        Quarterly = 1,

        [Display(Name = "Yearly", Description = "سنوي")]
        Yearly = 2
    }
}
