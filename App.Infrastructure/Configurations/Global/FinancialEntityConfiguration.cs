namespace App.Infrastructure.Configurations.Global
{
    public static class FinancialEntityConfiguration
    {
        // Money columns (UnitPrice, DownPayments, etc.)
        public const int MoneyPrecision = 18;
        public const int MoneyScale = 2;

        // Percentage columns (ROI, DownPaymentPercent, etc.)
        public const int PercentagePrecision = 5;   // e.g. 100.00
        public const int PercentageScale = 2;

        // ROI range constraints (backend validation can also use this)
        public const decimal MinPercentage = 0m;
        public const decimal MaxPercentage = 100m;
    }
}
