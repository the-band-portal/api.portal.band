using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;

namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class QuoteEntityModel : BaseEntityModel
    {
        public required Guid? GigId { get; set; }
        public required Guid? ClientContactId { get; set; }
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required QuoteStatusEnum Status { get; set; }






        public GigEntityModel? Gig { get; set; }
        public ContactEntityModel? Client { get; set; }
    }
}
