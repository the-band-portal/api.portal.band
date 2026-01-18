using BandPortal.WebServices.API.Common.Configuration.Sections;

namespace BandPortal.WebServices.API.Common.Configuration
{
    public class ConfigStructure
    {
        public required DevelopmentConfigSection Development { get; set; }
        public required CaptchaConfigSection Captcha { get; set; }
        public required JWTConfigSection JWT { get; set; }
    }
}
