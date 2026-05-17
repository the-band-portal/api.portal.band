using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class BandMembershipObjectService : IBandMembershipObjectService
    {
        private readonly BandPortalDbContext _db;
        private readonly ILogger<BandMembershipObjectService> _logger;

        public BandMembershipObjectService(
            BandPortalDbContext db,
            ILogger<BandMembershipObjectService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<BandMembershipEntityModel>> GetAllMembersAsync(Guid bandId)
        {
            try
            {
                return await _db.BandMemberships
                    .Where(bm => bm.BandId == bandId && bm.Role != BandMembershipRole.Client)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all members for band {BandId}", bandId);
                return new List<BandMembershipEntityModel>();
            }
        }

        public async Task<BandMembershipEntityModel?> GetMyMembershipAsync(Guid bandId, Guid userId)
        {
            try
            {
                return await _db.BandMemberships
                    .FirstOrDefaultAsync(bm =>
                        bm.BandId == bandId &&
                        bm.UserId == userId &&
                        bm.Role != BandMembershipRole.Client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get membership for user {UserId} in band {BandId}", userId, bandId);
                return null;
            }
        }

        public async Task<BandMembershipEntityModel?> GetMembershipByIdAsync(Guid bandId, Guid membershipId)
        {
            try
            {
                return await _db.BandMemberships
                    .FirstOrDefaultAsync(bm =>
                        bm.BandId == bandId &&
                        bm.Id == membershipId &&
                        bm.Role != BandMembershipRole.Client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get membership {MembershipId} for band {BandId}", membershipId, bandId);
                return null;
            }
        }
    }
}
