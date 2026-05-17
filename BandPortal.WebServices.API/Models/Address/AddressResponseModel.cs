using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Address
{
    public class AddressResponseModel : BaseResponseModel
    {
        [Required]
        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("addressLine1")]
        public string? AddressLine1 { get; init; }

        [JsonPropertyName("addressLine2")]
        public string? AddressLine2 { get; init; }

        [JsonPropertyName("city")]
        public string? City { get; init; }

        [JsonPropertyName("county")]
        public string? County { get; init; }

        [JsonPropertyName("country")]
        public string? Country { get; init; }

        [JsonPropertyName("postcode")]
        public string? Postcode { get; init; }

        [JsonPropertyName("latitude")]
        public float? Latitude{ get; init; }

        [JsonPropertyName("longitude")]
        public float? Longitude { get; init; }

        [JsonPropertyName("primaryContactId")]
        public Guid? PrimaryContactId { get; init; }
    }
}
