using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IBandMembershipObjectService
    {
        Task<List<BandMembershipEntityModel>> GetAllMembersAsync(Guid bandId);
        Task<BandMembershipEntityModel?> GetMyMembershipAsync(Guid bandId, Guid userId);
        Task<BandMembershipEntityModel?> GetMembershipByIdAsync(Guid bandId, Guid membershipId);
    }
}
