using System.ComponentModel.DataAnnotations;

namespace App.Common.Enums.Properties
{
    /// <summary>
    /// Exit strategy of the property investment.
    /// </summary>
    public enum ExitStrategyEnum
    {
        [Display(Name = "Fixed Term", Description = "مدة ثابتة")]
        FixedTerm = 0,

        [Display(Name = "Capital Appreciation", Description = "نمو رأس المال")]
        CapitalAppreciation = 1
    }
}
