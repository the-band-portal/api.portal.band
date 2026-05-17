using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IUserObjectService
    {
        Task<UserEntityModel?> GetUserAsync(Guid userId);
    }
}
