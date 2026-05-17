using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.User
{
    public class UserResponseModel
    {
        [Required]
        [JsonPropertyName("id")]
        public required Guid Id { get; init; }

        [Required]
        [JsonPropertyName("emailAddress")]
        [EmailAddress]
        public required string EmailAddress { get; init; }

        [Required]
        [JsonPropertyName("displayName")]
        public required string DisplayName { get; init; }
    }
}
