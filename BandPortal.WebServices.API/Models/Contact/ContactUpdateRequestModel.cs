using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Contact
{
    public class ContactUpdateRequestModel
    {
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }


        [JsonPropertyName("emailAddress")]
        public string? EmailAddress { get; set; }


        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }


        public bool HasAnyUpdates()
        {
            return DisplayName != null || EmailAddress != null || PhoneNumber != null;
        }
    }
}
