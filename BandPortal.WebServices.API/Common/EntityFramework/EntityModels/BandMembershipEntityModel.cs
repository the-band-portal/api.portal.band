using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;

namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class BandMembershipEntityModel : BaseEntityModel
    {
        public required Guid UserId { get; set; }
        public required BandMembershipRole Role { get; set; }





        public UserEntityModel? User { get; set; }
    }
}
