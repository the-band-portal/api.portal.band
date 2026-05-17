using BandPortal.WebServices.API.Common.ControllerBases;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Extensions;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandPortal.WebServices.API.Controllers
{
    [ApiController]
    [Route("/api/v2/bands/{bandId:guid}/contacts")]
    [Tags("Contacts")]
    [Authorize]
    public class ContactController : LoggedInControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;
        private readonly IBandObjectService _bandObjectService;

        public ContactController(
            ILogger<ContactController> logger,
            IContactService contactService,
            IBandObjectService bandObjectService,
            BandPortalDbContext db
        )
            : base(db, logger)
        {
            _logger = logger;
            _contactService = contactService;
            _bandObjectService = bandObjectService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<ContactResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ContactResponseModel>>> GetAllContacts(Guid bandId)
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                var hasAccess = await _bandObjectService.UserHasBandAccessAsync(user!.Id, bandId);
                if (!hasAccess)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new GenericFailResponseModel
                    {
                        Detail = "You do not have access to this band"
                    });
                }

                var contacts = await _contactService.GetAllAsync(bandId);
                var response = contacts.Select(c => c).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all contacts for band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpGet("{contactId}")]
        [ProducesResponseType(typeof(ContactResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactResponseModel>> GetContact(Guid bandId, Guid contactId)
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                var hasAccess = await _bandObjectService.UserHasBandAccessAsync(user!.Id, bandId);
                if (!hasAccess)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new GenericFailResponseModel
                    {
                        Detail = "You do not have access to this band"
                    });
                }

                var contact = await _contactService.GetByIdAsync(contactId, bandId);
                if (contact == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Contact not found"
                    });
                }

                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get contact {ContactId}", contactId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(ContactResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactResponseModel>> CreateContact(
            Guid bandId,
            [FromBody] ContactCreateRequestModel newContactData)
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                var hasAccess = await _bandObjectService.UserHasBandAccessAsync(user!.Id, bandId);
                if (!hasAccess)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new GenericFailResponseModel
                    {
                        Detail = "You do not have access to this band"
                    });
                }

                var contact = new ContactEntityModel
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    BandId = bandId,
                    CreatedBy = user.Id,
                    LastUpdatedBy = user.Id,
                    DisplayName = newContactData.DisplayName,
                    EmailAddress = newContactData.EmailAddress,
                    PhoneNumber = newContactData.PhoneNumber,
                };

                var createdContact = await _contactService.CreateAsync(contact);
                if (createdContact == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                    {
                        Detail = "Failed to create contact"
                    });
                }

                return StatusCode(StatusCodes.Status201Created, createdContact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create contact");
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPatch("{contactId}")]
        [ProducesResponseType(typeof(ContactResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactResponseModel>> UpdateContact(
            Guid bandId,
            Guid contactId,
            [FromBody] ContactUpdateRequestModel updateData)
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                var hasAccess = await _bandObjectService.UserHasBandAccessAsync(user!.Id, bandId);
                if (!hasAccess)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new GenericFailResponseModel
                    {
                        Detail = "You do not have access to this band"
                    });
                }

                if (!updateData.HasAnyUpdates())
                {
                    return BadRequest(new GenericFailResponseModel
                    {
                        Detail = "No fields provided for update"
                    });
                }

                var contact = await _contactService.UpdateAsync(contactId, bandId, updateData);
                if (contact == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Contact not found"
                    });
                }

                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update contact {ContactId}", contactId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpDelete("{contactId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteContact(Guid bandId, Guid contactId)
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                var hasAccess = await _bandObjectService.UserHasBandAccessAsync(user!.Id, bandId);
                if (!hasAccess)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new GenericFailResponseModel
                    {
                        Detail = "You do not have access to this band"
                    });
                }

                var deleted = await _contactService.SoftDeleteAsync(contactId, bandId, user.Id);
                if (!deleted)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Contact not found or already deleted"
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete contact {ContactId}", contactId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }
    }
}
