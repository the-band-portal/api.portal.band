using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models
{
    public abstract class BaseResponseModel
    {
        [Key]
        [Required]
        [JsonPropertyName("id")]
        public required Guid Id { get; set; }

        [Required]
        [JsonPropertyName("bandId")]
        public required Guid? BandId { get; set; }

        [Required]
        [JsonPropertyName("createdBy")]
        public required Guid? CreatedBy { get; set; }

        [Required]
        [JsonPropertyName("createdAt")]
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [JsonPropertyName("lastUpdatedBy")]
        public required Guid? LastUpdatedBy { get; set; }

        [Required]
        [JsonPropertyName("lastUpdatedAt")]
        public required DateTime? LastUpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [JsonPropertyName("deletedBy")]
        public required Guid? DeletedBy { get; set; }

        [Required]
        [JsonPropertyName("deletedAt")]
        public required DateTime? DeletedAt { get; set; }
    }
}
