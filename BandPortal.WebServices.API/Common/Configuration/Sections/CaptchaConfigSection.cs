namespace BandPortal.WebServices.API.Common.Configuration.Sections
{
    public class CaptchaConfigSection
    {
        public required string SiteKey { get; set; }
        public required string SecretKey { get; set; }
        public required float ScoreThreshold { get; set; }
    }
}
