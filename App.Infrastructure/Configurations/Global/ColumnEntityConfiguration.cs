namespace App.Infrastructure.Configurations.Global
{
    public static class ColumnEntityConfiguration
    {
        public const int DefaultStringSize = 256;
        public const int DefaultLongStringSize = 512;
        public const int DefaultImageSize = 2048;
        public const int DefaultDescriptionSize = 2048;
        public const int DefaultNameSize = 100;
        public const int DefaultPhoneSize = 50;

        public const bool DefaultIsActive = true;
        public const bool DefaultIsDeleted = false;
        public const bool DefaultIsVerified = false;

        public static readonly DateTime DefaultCreatedOn = DateTime.UtcNow;
    }
}
