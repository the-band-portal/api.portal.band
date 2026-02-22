namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class DividendEntityModel : BaseEntityModel
    {
        public required Guid InvoiceId { get; set; }
        public required Guid? UserId { get; set; }
        public required decimal Amount { get; set; }





        public InvoiceEntityModel? Invoice { get; set; }
        public UserEntityModel? User { get; set; }
    }
}
