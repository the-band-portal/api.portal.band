using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Band
{
    public class BandCreationRequestModel
    {
        [Required]
        [JsonPropertyName("name")]
        public required string Name { get; init; }


        [JsonPropertyName("biography")]
        public string? Biography { get; init; }
    }
}
