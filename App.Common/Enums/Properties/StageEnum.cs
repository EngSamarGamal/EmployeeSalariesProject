using System.ComponentModel.DataAnnotations;

namespace App.Common.Enums.Properties
{
    public enum StageEnum
    {
        [Display(Name = "Delivered", Description = "تم الإنشاء")]
        Delivered = 0,

        [Display(Name = "Off-Plan", Description = "قيد الإنشاء")]
        OffPlan = 1
    }
}
