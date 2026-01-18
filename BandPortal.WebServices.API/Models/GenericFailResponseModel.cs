using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models
{
    public class GenericFailResponseModel
    {
        [Required]
        [JsonPropertyName("detail")]
        public required string Detail { get; init; }
    }
}
