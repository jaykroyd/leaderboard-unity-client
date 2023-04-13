namespace Leaderboards
{
    public interface IRequest
    {
        string Endpoint { get; }
        string Method { get; }
        string ContentType { get; }
        byte[] GetBody();
    }
}
