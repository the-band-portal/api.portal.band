using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IBandObjectService
    {
        Task<BandEntityModel?> GetBandAsync(Guid bandId);
        Task<List<BandEntityModel>> GetAllBandsForUserAsync(Guid userId);
        Task<bool> UserHasBandAccessAsync(Guid userId, Guid bandId);
        Task<BandEntityModel?> CreateBandAsync(Guid userId, string name, string? biography);
        Task<BandEntityModel?> UpdateBandAsync(Guid bandId, string? name, string? biography);
    }
}
