using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.Venue
{
    public class VenueUpdateRequestModel
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }


        [JsonPropertyName("addressId")]
        public Guid? AddressId { get; set; }


        [JsonPropertyName("capacityDetails")]
        public string? CapacityDetails { get; set; }


        [JsonPropertyName("stageDetails")]
        public string? StageDetails { get; set; }


        [JsonPropertyName("parkingInstructions")]
        public string? ParkingInstructions { get; set; }


        [JsonPropertyName("loadInInstructions")]
        public string? LoadInInstructions { get; set; }


        [JsonPropertyName("loadOutInstructions")]
        public string? LoadOutInstructions { get; set; }


        [JsonPropertyName("primaryContactId")]
        public Guid? PrimaryContactId { get; set; }

        public bool HasAnyUpdates()
        {
            return Name != null
                || AddressId.HasValue
                || CapacityDetails != null
                || StageDetails != null
                || ParkingInstructions != null
                || LoadInInstructions != null
                || LoadOutInstructions != null
                || PrimaryContactId.HasValue;
        }
    }
}
