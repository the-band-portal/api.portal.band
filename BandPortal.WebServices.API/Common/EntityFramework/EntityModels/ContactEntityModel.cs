namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class ContactEntityModel : BaseEntityModel
    {
        public required string Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? UserId { get; set; }





        public UserEntityModel? User { get; set; }
    }
}
