using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;

namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class GigEntityModel : BaseEntityModel
    {
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






        public ContactEntityModel? Client { get; set; }
        public VenueEntityModel? Venue { get; set; }
    }
}
