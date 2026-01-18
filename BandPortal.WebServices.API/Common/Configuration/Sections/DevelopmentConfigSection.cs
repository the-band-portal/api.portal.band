namespace BandPortal.WebServices.API.Common.Configuration.Sections
{
    public class DevelopmentConfigSection
    {
        public required bool DisableReCAPTCHA { get; set; }
        public required bool PHPAcceptSelfSignedCertificatesForAPI { get; set; }
    }
}
