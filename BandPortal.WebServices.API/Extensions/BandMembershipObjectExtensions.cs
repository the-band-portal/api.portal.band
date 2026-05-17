using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.BandMembership;

namespace BandPortal.WebServices.API.Extensions
{
    public static class BandMembershipEntityExtensions
    {
        public static BandMembershipResponseModel ToResponseModel(this BandMembershipEntityModel entity)
        {
            return new BandMembershipResponseModel
            {
                Id = entity.Id,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedAt = entity.LastUpdatedAt,
                DeletedBy = entity.DeletedBy,
                DeletedAt = entity.DeletedAt,
                BandId = entity.BandId,

                UserId = entity.UserId,
                Role = entity.Role
            };
        }
    }
}
