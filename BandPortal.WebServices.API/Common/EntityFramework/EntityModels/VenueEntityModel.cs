namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class VenueEntityModel
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }





        public required string Name { get; set; }
        public Guid? AddressId { get; set; }
        public string? Capacity { get; set; }
        public string? StageDimensions { get; set; }
        public string? ParkingInstructions { get; set; }
        public string? LoadInInstructions { get; set; }
        public Guid? PrimaryContactId { get; set; }





        public UserEntityModel? CreatedByUser { get; set; }
        public UserEntityModel? LastUpdatedByUser { get; set; }
        public UserEntityModel? DeletedByUser { get; set; }





        public AddressEntityModel? Address { get; set; }
        public ContactEntityModel? PrimaryContact { get; set; }
    }
}
