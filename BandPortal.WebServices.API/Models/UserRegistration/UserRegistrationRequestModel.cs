using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.UserRegistration
{
    public class UserRegistrationRequestModel
    {
        [Required]
        [JsonPropertyName("recaptchaToken")]
        public required string RecaptchaToken { get; init; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("emailAddress")]
        public required string EmailAddress { get; init; }

        [Required]
        [JsonPropertyName("displayName")]
        public required string DisplayName { get; init; }

        [Required]
        [JsonPropertyName("rawPassword")]
        public required string RawPassword { get; init; }
    }
}
