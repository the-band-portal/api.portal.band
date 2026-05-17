namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class VenueEntityModel : BaseEntityModel
    {
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





        public ContactEntityModel? PrimaryContact { get; set; }
    }
}
