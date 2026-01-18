namespace BandPortal.WebServices.API.Common.Services
{
    public interface ICaptchaService
    {
        Task<bool> VerifyRecaptchaAsync(string token, string? clientIp);
    }
}
