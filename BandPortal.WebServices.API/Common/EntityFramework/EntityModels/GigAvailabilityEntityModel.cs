namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class GigAvailabilityEntityModel : BaseEntityModel
    {
        public required Guid GigId { get; set; }
        public required Guid? BandMembershipId { get; set; }
        public required bool IsAvailable { get; set; }
        public string? PrimaryInstrument { get; set; }





        public GigEntityModel? Gig { get; set; }
        public BandMembershipEntityModel? BandMembership { get; set; }
    }
}
