namespace BandPortal.WebServices.API.Models.Contact
{
    public class ContactUpdateRequestModel
    {
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public bool HasAnyUpdates()
        {
            return Name != null || EmailAddress != null || PhoneNumber != null;
        }
    }
}
