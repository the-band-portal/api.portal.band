namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class ContactsContactMethodEntityModel : BaseEntityModel
    {
        public Guid ContactId { get; set; }
        public required string Description{ get; set; }
        public required string Data { get; set; }





        public ContactEntityModel? Contact { get; set; }
    }
}
