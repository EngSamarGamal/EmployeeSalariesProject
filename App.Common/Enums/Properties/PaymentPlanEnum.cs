using System.ComponentModel.DataAnnotations;

namespace App.Common.Enums.Properties
{
    public enum PaymentPlanEnum
    {
        [Display(Name = "Installment", Description = "قسط")]
        Installment = 0,

        [Display(Name = "Cash", Description = "نقدي")]
        Cash = 1
    }
}
