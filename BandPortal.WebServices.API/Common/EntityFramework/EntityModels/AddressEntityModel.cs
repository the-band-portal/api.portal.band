namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class AddressEntityModel
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }





        public required string Name { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? Country { get; set; }
        public string? Postcode { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public Guid? PrimaryContactId { get; set; }





        public UserEntityModel? CreatedByUser { get; set; }
        public UserEntityModel? LastUpdatedByUser { get; set; }
        public UserEntityModel? DeletedByUser { get; set; }





        public ContactEntityModel? PrimaryContact { get; set; }
    }
}
