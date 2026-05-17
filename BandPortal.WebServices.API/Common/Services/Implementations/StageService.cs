using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Stage;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class StageService : IStageService
    {
        private readonly BandPortalDbContext _db;
        private readonly ILogger<StageService> _logger;

        public StageService(
            BandPortalDbContext db,
            ILogger<StageService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<StageEntityModel?> GetByIdAsync(Guid id, Guid bandId)
        {
            try
            {
                return await _db.Stages
                    .FirstOrDefaultAsync(v => v.Id == id && v.BandId == bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get stage {StageId} for band {BandId}", id, bandId);
                return null;
            }
        }

        public async Task<List<StageEntityModel>> GetAllAsync(Guid bandId)
        {
            try
            {
                return await _db.Stages
                    .Where(v => v.BandId == bandId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all stages for band {BandId}", bandId);
                return new List<StageEntityModel>();
            }
        }

        public async Task<StageEntityModel?> CreateAsync(StageEntityModel entity)
        {
            try
            {
                await _db.Stages.AddAsync(entity);
                await _db.SaveChangesAsync();
                return await GetByIdAsync(entity.Id, entity.BandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create stage");
                return null;
            }
        }

        public async Task<StageEntityModel?> UpdateAsync(Guid stageId, Guid bandId, StageUpdateRequestModel updateData)
        {
            try
            {
                var stage = await _db.Stages.FirstOrDefaultAsync(v => v.Id == stageId && v.BandId == bandId);
                if (stage == null)
                {
                    _logger.LogWarning("Stage {StageId} not found for band {BandId}", stageId, bandId);
                    return null;
                }

                bool hasUpdates = false;

                if (updateData.Name != null)                { stage.Name = updateData.Name;                                 hasUpdates = true; }
                if (updateData.CapacityDetails != null)     { stage.CapacityDetails = updateData.CapacityDetails;           hasUpdates = true; }
                if (updateData.StageDetails != null)        { stage.StageDetails = updateData.StageDetails;                 hasUpdates = true; }
                if (updateData.ParkingInstructions != null) { stage.ParkingInstructions = updateData.ParkingInstructions;   hasUpdates = true; }
                if (updateData.LoadInInstructions != null)  { stage.LoadInInstructions = updateData.LoadInInstructions;     hasUpdates = true; }
                if (updateData.LoadOutInstructions != null) { stage.LoadOutInstructions = updateData.LoadOutInstructions;   hasUpdates = true; }
                if (updateData.PrimaryContactId.HasValue)   { stage.PrimaryContactId = updateData.PrimaryContactId;         hasUpdates = true; }

                if (!hasUpdates)
                {
                    return stage;
                }

                await _db.SaveChangesAsync();
                return await GetByIdAsync(stageId, bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update stage {StageId}", stageId);
                return null;
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid id, Guid bandId, Guid deletedBy)
        {
            try
            {
                var stage = await _db.Stages.FirstOrDefaultAsync(v => v.Id == id && v.BandId == bandId && v.DeletedAt == null);
                if (stage == null)
                {
                    return false;
                }

                stage.DeletedBy = deletedBy;
                stage.DeletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete stage {StageId}", id);
                return false;
            }
        }







        public async Task<List<StageEntityModel>> GetAllForVenueAsync(Guid bandId, Guid venueId)
        {
            try
            {
                return await _db.Stages
                    .Where(v => v.BandId == bandId)
                    .Where(v => v.VenueId == venueId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all stages for band {BandId}", bandId);
                return new List<StageEntityModel>();
            }
        }
    }
}
