using BandPortal.WebServices.API.Common.ControllerBases;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Extensions;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.Stage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandPortal.WebServices.API.Controllers
{
    [ApiController]
    [Route("/api/v2/bands/{bandId:guid}/venues/{venueId:guid}/stages")]
    [Tags("Stages")]
    [Authorize]
    public class StageController : LoggedInControllerBase
    {
        private readonly ILogger<StageController> _logger;
        private readonly IVenueService _venueService;
        private readonly IStageService _stageService;
        private readonly IBandObjectService _bandObjectService;

        public StageController(
            ILogger<StageController> logger,
            IStageService venueService,
            IBandObjectService bandObjectService,
            BandPortalDbContext db
        )
            : base(db, logger)
        {
            _logger = logger;
            _stageService = venueService;
            _bandObjectService = bandObjectService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<StageResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StageResponseModel>>> GetAllStages(Guid bandId, Guid venueId)
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

                var stages = await _stageService.GetAllForVenueAsync(bandId, venueId);
                var response = stages.Select(s => s.ToResponseModel()).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all venues for band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpGet("{stageId:guid}")]
        [ProducesResponseType(typeof(StageResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StageResponseModel>> GetStage(Guid bandId, Guid venueId, Guid stageId)
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

                var venue = await _stageService.GetByIdAsync(venueId, bandId);
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
        [ProducesResponseType(typeof(StageResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StageResponseModel>> CreateStage(
            Guid bandId,
            Guid venueId,
            [FromBody] StageCreationRequestModel newVenueData)
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

                var venue = new StageEntityModel
                {
                    Id = Guid.NewGuid(),
                    BandId = bandId,
                    CreatedBy = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedBy = user.Id,
                    Name = newVenueData.Name,
                    VenueId = venueId,
                    PrimaryContactId = newVenueData.PrimaryContactId
                };

                var createdVenue = await _stageService.CreateAsync(venue);
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

        [HttpPatch("{stageId:guid}")]
        [ProducesResponseType(typeof(StageResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StageResponseModel>> UpdateStage(
            Guid bandId,
            Guid venueId,
            Guid stageId,
            [FromBody] StageUpdateRequestModel updateData)
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

                var venue = await _stageService.UpdateAsync(venueId, bandId, updateData);
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
    }
}
