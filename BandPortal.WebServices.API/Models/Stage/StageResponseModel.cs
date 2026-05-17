using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Stage
{
    public class StageResponseModel : BaseResponseModel
    {
        [Required]
        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [Required]
        [JsonPropertyName("addressId")]
        public required Guid? AddressId { get; init; }

        [Required]
        [JsonPropertyName("capacityDetails")]
        public required string? CapacityDetails { get; init; }

        [Required]
        [JsonPropertyName("stageDetails")]
        public required string? StageDetails { get; init; }

        [Required]
        [JsonPropertyName("parkingInstructions")]
        public required string? ParkingInstructions { get; init; }

        [Required]
        [JsonPropertyName("loadInInstructions")]
        public required string? LoadInInstructions { get; init; }

        [Required]
        [JsonPropertyName("loadOutInstructions")]
        public required string? LoadOutInstructions { get; init; }

        [Required]
        [JsonPropertyName("primaryContactId")]
        public required Guid? PrimaryContactId { get; init; }
    }
}
