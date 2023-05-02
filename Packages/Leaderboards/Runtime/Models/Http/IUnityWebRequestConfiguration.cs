namespace Leaderboards
{
    public interface IUnityWebRequestConfiguration
    {
        string ApiKey { get; }
        string BaseUrl { get; }        
        string Subdomain { get; }
        int Version { get; }
    }
}
