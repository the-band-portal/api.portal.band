using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Contact
{
    public class ContactResponseModel
    {
        [Required]
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [Required]
        [JsonPropertyName("emailAddress")]
        public required string EmailAddress { get; set; }

        [Required]
        [JsonPropertyName("phoneNumber")]
        public required string PhoneNumber { get; set; }

        [Required]
        [JsonPropertyName("userId")]
        public required Guid UserId { get; set; }
    }
}
