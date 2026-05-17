using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Address;
using BandPortal.WebServices.API.Models.Venue;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IAddressService : IBaseEntityService<AddressEntityModel>
    {
        Task<AddressEntityModel?> UpdateAsync(Guid venueId, Guid bandId, AddressUpdateRequestModel updateData);
    }
}
