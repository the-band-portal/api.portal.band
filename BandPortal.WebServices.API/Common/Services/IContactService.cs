using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Contact;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IContactService : IBaseEntityService<ContactEntityModel>
    {
        Task<ContactEntityModel?> UpdateAsync(Guid contactId, Guid bandId, ContactUpdateRequestModel updateData);
    }
}
