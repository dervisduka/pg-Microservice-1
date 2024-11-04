namespace Application.Common.Interfaces
{
    public interface ICurrentActorService
    {
        string? ClientId { get; }
        string? UserId { get; }
        bool IsClient { get; }
        List<string> Permissions { get; }
        string? UserIP { get; }
    }
}
