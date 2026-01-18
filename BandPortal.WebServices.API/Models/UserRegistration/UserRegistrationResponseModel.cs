using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.UserRegistration
{
    public class UserRegistrationResponseModel
    {
        [Required]
        [JsonPropertyName("id")]
        public required Guid Id { get; init; }
    }
}
