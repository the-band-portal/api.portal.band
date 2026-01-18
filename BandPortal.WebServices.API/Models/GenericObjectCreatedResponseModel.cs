using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models
{
    public class GenericObjectCreatedResponseModel
    {
        [Required]
        [JsonPropertyName("objectId")]
        public required string ObjectId { get; init; }
    }
}
