using System.ComponentModel.DataAnnotations;

namespace App.Common.Enums
{
    public enum PropertyStatusEnum
    {
        [Display(Name = "Available", Description = "متاح")]
        Available = 1,

        [Display(Name = "Funded", Description = "مكتمل التمويل")]
        Funded = 2,

        [Display(Name = "Exited", Description = "مُغلَق")]
        Exited = 3,

        [Display(Name = "Not Available", Description = "غير متاح")]
        NotAvailable = 4
    }
}
