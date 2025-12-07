namespace App.Common.Dtos.Identity
{
    public class JWT
    {
        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public double DurationInHours { get; set; }
        public double RefreshTokenValidityInDays { get; set; }
    }
}
