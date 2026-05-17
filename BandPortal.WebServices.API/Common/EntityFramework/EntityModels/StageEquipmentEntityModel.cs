namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class StageEquipmentEntityModel : BaseEntityModel
    {
        public required Guid VenueId { get; set; }
        public required string Description { get; set; }
        public required string Available { get; set; }





        public StageEntityModel? Venue { get; set; }
    }
}
