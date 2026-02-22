using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Band
{
    public class BandResponseModel : BaseResponseModel
    {
        [Required]
        [JsonPropertyName("name")]
        [EmailAddress]
        public required string Name { get; init; }

        [Required]
        [JsonPropertyName("biography")]
        public required string Biography { get; init; }
    }
}
