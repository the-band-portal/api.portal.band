using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;

namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class InvoiceEntityModel : BaseEntityModel
    {
        public required Guid? GigId { get; set; }






        public GigEntityModel? Gig { get; set; }
    }
}
