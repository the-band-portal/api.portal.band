using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;

namespace BandPortal.WebServices.API.Common.EntityFramework
{
    public class BaseEntityModel
    {
        public required Guid Id { get; set; }


        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }


        public DateTime? LastUpdatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }


        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

        public required Guid BandId { get; set; }





        public UserEntityModel? CreatedByUser { get; set; }
        public UserEntityModel? LastUpdatedByUser { get; set; }
        public UserEntityModel? DeletedByUser { get; set; }
        public BandEntityModel? Band { get; set; }
    }
}
