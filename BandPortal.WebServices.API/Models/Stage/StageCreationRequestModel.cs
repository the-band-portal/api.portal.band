using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Stage
{
    public class StageCreationRequestModel
    {
        [Required]
        [JsonPropertyName("name")]
        public required string Name { get; set; }


        [JsonPropertyName("capacityDetails")]
        public string CapacityDetails { get; set; } = string.Empty;


        [JsonPropertyName("stageDetails")]
        public string StageDetails { get; set; } = string.Empty;


        [JsonPropertyName("parkingInstructions")]
        public string ParkingInstructions { get; set; } = string.Empty;


        [JsonPropertyName("loadInInstructions")]
        public string LoadInInstructions { get; set; } = string.Empty;


        [JsonPropertyName("loadOutInstructions")]
        public string LoadOutInstructions { get; set; } = string.Empty;


        [JsonPropertyName("primaryContactId")]
        public Guid? PrimaryContactId { get; set; }
    }
}
