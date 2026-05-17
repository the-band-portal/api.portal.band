using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;

namespace BandPortal.WebServices.API.Common.Services
{
    public interface IBaseEntityService<TEntity> where TEntity : BaseEntityModel
    {
        Task<TEntity?> GetByIdAsync(Guid id, Guid bandId);
        Task<List<TEntity>> GetAllAsync(Guid bandId);
        Task<TEntity?> CreateAsync(TEntity entity);
        Task<bool> SoftDeleteAsync(Guid id, Guid bandId, Guid deletedBy);
    }
}
