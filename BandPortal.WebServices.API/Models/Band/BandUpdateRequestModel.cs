using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Band
{
    public class BandUpdateRequestModel
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }


        [JsonPropertyName("biography")]
        public string? Biography { get; init; }
    }
}
