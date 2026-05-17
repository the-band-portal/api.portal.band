using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Contact
{
    public class ContactCreateRequestModel
    {
        [Required]
        [JsonPropertyName("displayName")]
        public required string DisplayName { get; set; }


        [JsonPropertyName("emailAddress")]
        public string? EmailAddress { get; set; }


        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }
}
