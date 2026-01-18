namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class GigAvailabilityEntityModel
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }





        public required Guid GigId { get; set; }
        public required Guid? BandMembershipId { get; set; }
        public required bool IsAvailable { get; set; }
        public string? PrimaryInstrument { get; set; }





        public UserEntityModel? CreatedByUser { get; set; }
        public UserEntityModel? LastUpdatedByUser { get; set; }
        public UserEntityModel? DeletedByUser { get; set; }





        public GigEntityModel? Gig { get; set; }
        public BandMembershipEntityModel? BandMembership { get; set; }
    }
}
