using System.ComponentModel.DataAnnotations;

namespace BandPortal.WebServices.API.Models
{
    public abstract class BaseResponseModel
    {
        [Key]
        [Required]
        public required Guid Id { get; set; }

        [Required]
        public required Guid? BandId { get; set; }

        [Required]
        public required Guid? CreatedBy { get; set; }

        [Required]
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public required Guid? LastUpdatedBy { get; set; }

        [Required]
        public required DateTime? LastUpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public required Guid? DeletedBy { get; set; }

        [Required]
        public required DateTime? DeletedAt { get; set; }
    }
}
