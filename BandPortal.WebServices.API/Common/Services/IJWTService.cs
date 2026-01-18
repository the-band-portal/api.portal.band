namespace BandPortal.WebServices.API.Common.Services
{
    public interface IJWTService
    {
        string CreateAccessJWT(Dictionary<string, object> userData);
        string CreateRefreshJWT(Dictionary<string, object> userData);
    }
}
