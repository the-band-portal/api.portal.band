using BandPortal.WebServices.API.Common.ControllerBases;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Extensions;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.Venue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandPortal.WebServices.API.Controllers
{
    [ApiController]
    [Route("/api/v2/bands/{bandId:guid}/venues")]
    [Tags("Venues")]
    [Authorize]
    public class VenueController : LoggedInControllerBase
    {
        private readonly ILogger<VenueController> _logger;
        private readonly IVenueService _venueService;
        private readonly IBandObjectService _bandObjectService;

        public VenueController(
            ILogger<VenueController> logger,
            IVenueService venueService,
            IBandObjectService bandObjectService,
            BandPortalDbContext db
        )
            : base(db, logger)
        {
            _logger = logger;
            _venueService = venueService;
            _bandObjectService = bandObjectService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<VenueResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<VenueResponseModel>>> GetAllVenues(Guid bandId)
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

                var Venues = await _venueService.GetAllAsync(bandId);
                var response = Venues.Select(v => v.ToResponseModel()).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all Venues for band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpGet("{venueId}")]
        [ProducesResponseType(typeof(VenueResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VenueResponseModel>> GetVenue(Guid bandId, Guid venueId)
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

                var venue = await _venueService.GetByIdAsync(venueId, bandId);
                if (venue == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Venue not found"
                    });
                }

                return Ok(venue.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get venue {VenueId}", venueId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(VenueResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VenueResponseModel>> CreateVenue(
            Guid bandId,
            [FromBody] VenueCreationRequestModel newVenueData)
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

                var venue = new VenueEntityModel
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    BandId = bandId,
                    CreatedBy = user.Id,
                    LastUpdatedBy = user.Id,
                    Name = newVenueData.Name,
                    AddressLine1 = newVenueData.AddressLine1,
                    AddressLine2 = newVenueData.AddressLine2,
                    City = newVenueData.City,
                    County = newVenueData.County,
                    Country = newVenueData.Country,
                    Postcode = newVenueData.Postcode,
                    Latitude = newVenueData.Latitude,
                    Longitude = newVenueData.Longitude,
                };

                var createdVenue = await _venueService.CreateAsync(venue);
                if (createdVenue == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                    {
                        Detail = "Failed to create venue"
                    });
                }

                return StatusCode(StatusCodes.Status201Created, createdVenue.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create venue");
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPatch("{venueId}")]
        [ProducesResponseType(typeof(VenueResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VenueResponseModel>> UpdateVenue(
            Guid bandId,
            Guid venueId,
            [FromBody] VenueUpdateRequestModel updateData)
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

                var venue = await _venueService.UpdateAsync(venueId, bandId, updateData);
                if (venue == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Venue not found"
                    });
                }

                return Ok(venue.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update venue {VenueId}", venueId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpDelete("{venueId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteVenue(Guid bandId, Guid venueId)
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

                var deleted = await _venueService.SoftDeleteAsync(venueId, bandId, user.Id);
                if (!deleted)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Venue not found or already deleted"
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete venue {VenueId}", venueId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }
    }
}
