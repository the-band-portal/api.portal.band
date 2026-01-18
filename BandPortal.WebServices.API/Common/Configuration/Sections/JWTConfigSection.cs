namespace BandPortal.WebServices.API.Common.Configuration.Sections
{
    public class JWTConfigSection
    {
        public required string SecretKey { get; set; }
        public required string Algorithm { get; set; }
        public required int AccessTokenExpirationInMinutes { get; set; }
        public required int RefreshTokenExpirationInDays { get; set; }
        public required string ValidIssuer { get; set; }
        public required string ValidAudience { get; set; }
    }
}
