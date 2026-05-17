using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;
using BandPortal.WebServices.API.Common.Enumerators;
using BandPortal.WebServices.API.Common.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class BandObjectService : IBandObjectService
    {
        private readonly BandPortalDbContext _db;
        private readonly ILogger<BandObjectService> _logger;

        public BandObjectService(
            BandPortalDbContext db,
            ILogger<BandObjectService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<BandEntityModel?> GetBandAsync(Guid bandId)
        {
            try
            {
                return await _db.Bands
                    .FirstOrDefaultAsync(b => b.Id == bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Band {BandId}", bandId);
                return null;
            }
        }

        public async Task<List<BandEntityModel>> GetAllBandsForUserAsync(Guid userId)
        {
            try
            {
                return await _db.Bands
                    .Join(_db.BandMemberships,
                        band => band.Id,
                        membership => membership.BandId,
                        (band, membership) => new { Band = band, Membership = membership })
                    .Where(x => x.Membership.UserId == userId)
                    .Select(x => x.Band)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all bands for user {UserId}", userId);
                return new List<BandEntityModel>();
            }
        }

        public async Task<bool> UserHasBandAccessAsync(Guid userId, Guid bandId)
        {
            try
            {
                return await _db.BandMemberships
                    .AnyAsync(bm => bm.BandId == bandId && bm.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check band access for user {UserId} and band {BandId}", userId, bandId);
                return false;
            }
        }

        public async Task<BandEntityModel?> CreateBandAsync(Guid userId, string name, string? biography)
        {
            try
            {
                Guid bandId = UUIDHandlingUtilities.GenerateV5(DatabaseObjectType.Band);
                Guid bandMembershupId = UUIDHandlingUtilities.GenerateV5(DatabaseObjectType.BandMembership);

                var band = new BandEntityModel
                {
                    Id = bandId,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow,

                    Name = name,
                    Biography = biography
                };

                await _db.Bands.AddAsync(band);

                var membership = new BandMembershipEntityModel
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId,
                    BandId = bandId,
                    Role = BandMembershipRole.Owner
                };

                await _db.BandMemberships.AddAsync(membership);
                await _db.SaveChangesAsync();

                return await GetBandAsync(bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create band for user {UserId}", userId);
                return null;
            }
        }

        public async Task<BandEntityModel?> UpdateBandAsync(Guid bandId, string? name, string? biography)
        {
            try
            {
                var band = await _db.Bands.FirstOrDefaultAsync(b => b.Id == bandId);
                if (band == null)
                {
                    _logger.LogWarning("Band {BandId} not found for update", bandId);
                    return null;
                }

                bool hasUpdates = false;

                if (name != null)
                {
                    band.Name = name;
                    hasUpdates = true;
                }

                if (biography != null)
                {
                    band.Biography = biography;
                    hasUpdates = true;
                }

                if (!hasUpdates)
                {
                    return band;
                }

                await _db.SaveChangesAsync();
                return await GetBandAsync(bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update band {BandId}", bandId);
                return null;
            }
        }
    }
}
