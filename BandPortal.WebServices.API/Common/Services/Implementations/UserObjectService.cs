using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class UserObjectService : IUserObjectService
    {
        private readonly BandPortalDbContext _db;
        private readonly ILogger<UserObjectService> _logger;

        public UserObjectService(
            BandPortalDbContext db,
            ILogger<UserObjectService> logger)
        {
            _db = db;
            _logger = logger;
        }


        public async Task<UserEntityModel?> GetDocumentAsync(Guid userId)
        {
            try
            {
                return await _db.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user {UserId}", userId);
                return null;
            }
        }
    }
}
