using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;

namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class GigEntityModel
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }





        public required string Name { get; set; }
        public required string? Description { get; set; }
        public required Guid? ClientContactId { get; set; }
        public Guid? VenueId { get; set; }
        public DateOnly? PerformanceStartDate { get; set; }
        public TimeOnly? PerformanceStartTime { get; set; }
        public DateOnly? PerformanceEndDate { get; set; }
        public TimeOnly? PerformanceEndTime { get; set; }
        public DateTime? LoadInDateTime { get; set; }
        public DateTime? LoadOutDateTime { get; set; }
        public DateTime? SoundCheckDateTime { get; set; }
        public GigStatusEnum? Status { get; set; }






        public UserEntityModel? CreatedByUser { get; set; }
        public UserEntityModel? LastUpdatedByUser { get; set; }
        public UserEntityModel? DeletedByUser { get; set; }






        public ContactEntityModel? Client { get; set; }
        public VenueEntityModel? Venue { get; set; }
    }
}
