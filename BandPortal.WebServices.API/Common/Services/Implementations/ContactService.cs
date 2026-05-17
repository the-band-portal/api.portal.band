using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Contact;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly BandPortalDbContext _db;
        private readonly ILogger<ContactService> _logger;

        public ContactService(
            BandPortalDbContext db,
            ILogger<ContactService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ContactEntityModel?> GetByIdAsync(Guid id, Guid bandId)
        {
            try
            {
                return await _db.Contacts
                    .FirstOrDefaultAsync(c => c.Id == id && c.BandId == bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get contact {ContactId} for band {BandId}", id, bandId);
                return null;
            }
        }

        public async Task<List<ContactEntityModel>> GetAllAsync(Guid bandId)
        {
            try
            {
                return await _db.Contacts
                    .Where(c => c.BandId == bandId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all contacts for band {BandId}", bandId);
                return new List<ContactEntityModel>();
            }
        }

        public async Task<ContactEntityModel?> CreateAsync(ContactEntityModel entity)
        {
            try
            {
                await _db.Contacts.AddAsync(entity);
                await _db.SaveChangesAsync();
                return await GetByIdAsync(entity.Id, entity.BandId!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create contact");
                return null;
            }
        }

        public async Task<ContactEntityModel?> UpdateAsync(Guid contactId, Guid bandId, ContactUpdateRequestModel updateData)
        {
            try
            {
                var contact = await _db.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.BandId == bandId);
                if (contact == null)
                {
                    _logger.LogWarning("Contact {ContactId} not found for band {BandId}", contactId, bandId);
                    return null;
                }

                bool hasUpdates = false;

                if (updateData.DisplayName != null)
                {
                    contact.DisplayName = updateData.DisplayName;
                    hasUpdates = true;
                }
                if (updateData.EmailAddress != null)
                {
                    contact.EmailAddress = updateData.EmailAddress;
                    hasUpdates = true;
                }
                if (updateData.PhoneNumber != null)
                {
                    contact.PhoneNumber = updateData.PhoneNumber;
                    hasUpdates = true;
                }

                if (!hasUpdates)
                {
                    return contact;
                }

                await _db.SaveChangesAsync();
                return await GetByIdAsync(contactId, bandId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update contact {ContactId}", contactId);
                return null;
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid id, Guid bandId, Guid deletedBy)
        {
            try
            {
                var contact = await _db.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.BandId == bandId && c.DeletedAt == null);
                if (contact == null)
                {
                    return false;
                }

                contact.DeletedBy = deletedBy;
                contact.DeletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete contact {ContactId}", id);
                return false;
            }
        }
    }
}
