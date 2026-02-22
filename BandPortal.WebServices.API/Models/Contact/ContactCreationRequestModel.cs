namespace BandPortal.WebServices.API.Models.Contact
{
    public class ContactCreateRequestModel
    {
        public required string Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
