namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class VenueEntityModel : BaseEntityModel
    {
        public required string Name { get; set; }
        public Guid? AddressId { get; set; }
        public string? CapacityDetails { get; set; }
        public string? StageDetails { get; set; }
        public string? ParkingInstructions { get; set; }
        public string? LoadInInstructions { get; set; }
        public string? LoadOutInstructions { get; set; }
        public Guid? PrimaryContactId { get; set; }





        public AddressEntityModel? Address { get; set; }
        public ContactEntityModel? PrimaryContact { get; set; }
    }
}
