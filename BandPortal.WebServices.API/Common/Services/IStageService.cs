using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Stage;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IStageService : IBaseEntityService<StageEntityModel>
    {
        Task<StageEntityModel?> UpdateAsync(Guid venueId, Guid bandId, StageUpdateRequestModel updateData);
    }
}
