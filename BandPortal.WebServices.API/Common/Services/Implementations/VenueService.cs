using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Venue;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class VenueService : IVenueService
    {
        private readonly BandPortalDbContext _db;
        private readonly ILogger<VenueService> _logger;

        public VenueService(
            BandPortalDbContext db,
            ILogger<VenueService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<VenueEntityModel?> GetByIdAsync(Guid id, Guid bandId)
        {
            try
            {
                return await _db.Venues
                    .FirstOrDefaultAsync(v => v.Id == id && v.BandId == bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get venue {VenueId} for band {BandId}", id, bandId);
                return null;
            }
        }

        public async Task<List<VenueEntityModel>> GetAllAsync(Guid bandId)
        {
            try
            {
                return await _db.Venues
                    .Where(v => v.BandId == bandId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all venues for band {BandId}", bandId);
                return new List<VenueEntityModel>();
            }
        }

        public async Task<VenueEntityModel?> CreateAsync(VenueEntityModel entity)
        {
            try
            {
                await _db.Venues.AddAsync(entity);
                await _db.SaveChangesAsync();
                return await GetByIdAsync(entity.Id, entity.BandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create venue");
                return null;
            }
        }

        public async Task<VenueEntityModel?> UpdateAsync(Guid venueId, Guid bandId, VenueUpdateRequestModel updateData)
        {
            try
            {
                var venue = await _db.Venues.FirstOrDefaultAsync(v => v.Id == venueId && v.BandId == bandId);
                if (venue == null)
                {
                    _logger.LogWarning("Venue {VenueId} not found for band {BandId}", venueId, bandId);
                    return null;
                }

                bool hasUpdates = false;

                if (updateData.Name != null)
                {
                    venue.Name = updateData.Name;
                    hasUpdates = true;
                }
                if (updateData.AddressId.HasValue)
                {
                    venue.AddressId = updateData.AddressId;
                    hasUpdates = true;
                }
                if (updateData.CapacityDetails != null)
                {
                    venue.CapacityDetails = updateData.CapacityDetails;
                    hasUpdates = true;
                }
                if (updateData.StageDetails != null)
                {
                    venue.StageDetails = updateData.StageDetails;
                    hasUpdates = true;
                }
                if (updateData.ParkingInstructions != null)
                {
                    venue.ParkingInstructions = updateData.ParkingInstructions;
                    hasUpdates = true;
                }
                if (updateData.LoadInInstructions != null)
                {
                    venue.LoadInInstructions = updateData.LoadInInstructions;
                    hasUpdates = true;
                }
                if (updateData.LoadOutInstructions != null)
                {
                    venue.LoadOutInstructions = updateData.LoadOutInstructions;
                    hasUpdates = true;
                }
                if (updateData.PrimaryContactId.HasValue)
                {
                    venue.PrimaryContactId = updateData.PrimaryContactId;
                    hasUpdates = true;
                }

                if (!hasUpdates)
                {
                    return venue;
                }

                await _db.SaveChangesAsync();
                return await GetByIdAsync(venueId, bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update venue {VenueId}", venueId);
                return null;
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid id, Guid bandId, Guid deletedBy)
        {
            try
            {
                var venue = await _db.Venues.FirstOrDefaultAsync(v => v.Id == id && v.BandId == bandId && v.DeletedAt == null);
                if (venue == null)
                {
                    return false;
                }

                venue.DeletedBy = deletedBy;
                venue.DeletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete venue {VenueId}", id);
                return false;
            }
        }
    }
}
