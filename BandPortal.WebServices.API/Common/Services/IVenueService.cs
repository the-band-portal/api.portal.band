using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Venue;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IVenueService : IBaseEntityService<VenueEntityModel>
    {
        Task<VenueEntityModel?> UpdateAsync(Guid venueId, Guid bandId, VenueUpdateRequestModel updateData);
    }
}
