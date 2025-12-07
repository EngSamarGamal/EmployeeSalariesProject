using System.ComponentModel.DataAnnotations;

namespace App.Common.Enums.Properties
{
    /// <summary>
    /// Investment model of the property.
    /// </summary>
    public enum InvestmentModelEnum
    {
        [Display(Name = "Quick Gain", Description = "ربح سريع")]
        QuickGain = 0,

        [Display(Name = "Steady Growth", Description = "نمو ثابت")]
        SteadyGrowth = 1
    }
}
