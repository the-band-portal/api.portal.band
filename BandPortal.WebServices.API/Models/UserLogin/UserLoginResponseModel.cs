using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.UserLogin
{
    public class UserLoginResponseModel
    {
        [Required]
        [JsonPropertyName("accessToken")]
        public required string AccessToken { get; init; }

        [Required]
        [JsonPropertyName("refreshToken")]
        public required string RefreshToken { get; init; }

        [Required]
        [JsonPropertyName("expiresInMinutes")]
        public required int ExpiresInMinutes { get; init; }
    }
}
