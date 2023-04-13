namespace Leaderboards
{
    public interface IResponse
    {
        bool IsError { get; internal set; }
        string Error { get; }
    }
}
