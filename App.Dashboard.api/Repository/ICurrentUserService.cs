namespace App.Dashboard.api.Repository
{
    public interface ICurrentUserService
    {
        string? AccessToken { get; }
        string? UserId { get; }
    }
}
