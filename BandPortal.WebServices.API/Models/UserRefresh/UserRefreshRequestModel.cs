using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.UserRefresh
{
    public class UserRefreshRequestModel
    {
        [Required]
        [JsonPropertyName("refreshToken")]
        public required string RefreshToken { get; init; }
    }
}
