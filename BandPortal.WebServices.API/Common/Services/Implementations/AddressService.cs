using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Address;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly BandPortalDbContext _db;
        private readonly ILogger<AddressService> _logger;

        public AddressService(
            BandPortalDbContext db,
            ILogger<AddressService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddressEntityModel?> GetByIdAsync(Guid id, Guid bandId)
        {
            try
            {
                return await _db.Addresses
                    .FirstOrDefaultAsync(v => v.Id == id && v.BandId == bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get address {AddressId} for band {BandId}", id, bandId);
                return null;
            }
        }

        public async Task<List<AddressEntityModel>> GetAllAsync(Guid bandId)
        {
            try
            {
                return await _db.Addresses
                    .Where(v => v.BandId == bandId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all addresss for band {BandId}", bandId);
                return new List<AddressEntityModel>();
            }
        }

        public async Task<AddressEntityModel?> CreateAsync(AddressEntityModel entity)
        {
            try
            {
                await _db.Addresses.AddAsync(entity);
                await _db.SaveChangesAsync();
                return await GetByIdAsync(entity.Id, entity.BandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create address");
                return null;
            }
        }

        public async Task<AddressEntityModel?> UpdateAsync(Guid addressId, Guid bandId, AddressUpdateRequestModel updateData)
        {
            try
            {
                var address = await _db.Addresses.FirstOrDefaultAsync(v => v.Id == addressId && v.BandId == bandId);
                if (address == null)
                {
                    _logger.LogWarning("Address {AddressId} not found for band {BandId}", addressId, bandId);
                    return null;
                }

                bool hasUpdates = false;

                if (updateData.Name != null)                {   address.Name = updateData.Name;                             hasUpdates = true; }
                if (updateData.AddressLine1 != null)        {   address.AddressLine1 = updateData.AddressLine1;             hasUpdates = true; }
                if (updateData.AddressLine2 != null)        {   address.AddressLine1 = updateData.AddressLine1;             hasUpdates = true; }
                if (updateData.City != null)                {   address.City = updateData.City;                             hasUpdates = true; }
                if (updateData.County != null)              {   address.County = updateData.County;                         hasUpdates = true; }
                if (updateData.Country != null)             {   address.Country = updateData.Country;                       hasUpdates = true; }
                if (updateData.Postcode != null)            {   address.Postcode = updateData.Postcode;                     hasUpdates = true; }
                if (updateData.Latitude != null)            {   address.Latitude = updateData.Latitude;                     hasUpdates = true; }
                if (updateData.Longitude != null)           {   address.Longitude = updateData.Longitude;                   hasUpdates = true; }
                if (updateData.PrimaryContactId.HasValue)   {   address.PrimaryContactId = updateData.PrimaryContactId;     hasUpdates = true; }

                if (!hasUpdates)
                {
                    return address;
                }

                await _db.SaveChangesAsync();
                return await GetByIdAsync(addressId, bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update address {AddressId}", addressId);
                return null;
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid id, Guid bandId, Guid deletedBy)
        {
            try
            {
                var address = await _db.Addresses.FirstOrDefaultAsync(v => v.Id == id && v.BandId == bandId && v.DeletedAt == null);
                if (address == null)
                {
                    return false;
                }

                address.DeletedBy = deletedBy;
                address.DeletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete address {AddressId}", id);
                return false;
            }
        }
    }
}
