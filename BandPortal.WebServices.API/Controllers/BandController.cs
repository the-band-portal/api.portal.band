using BandPortal.WebServices.API.Common.ControllerBases;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.Band;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandPortal.WebServices.API.Controllers
{
    [ApiController]
    [Route("/api/v2/bands")]
    [Tags("Bands")]
    [Authorize]
    public class BandController : LoggedInControllerBase
    {
        private readonly ILogger<BandController> _logger;
        private readonly IBandObjectService _bandObjectService;

        public BandController(
            ILogger<BandController> logger,
            IBandObjectService bandObjectService,
            BandPortalDbContext db
        )
            : base(db, logger)
        {
            _logger = logger;
            _bandObjectService = bandObjectService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<BandResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<BandResponseModel>>> GetAllBands()
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                var bands = await _bandObjectService.GetAllBandsForUserAsync(user!.Id);

                var response = bands.Select(band => new BandResponseModel
                {
                    Id = band.Id,
                    BandId = band.Id,
                    CreatedBy = band.CreatedBy,
                    CreatedAt = band.CreatedAt,
                    LastUpdatedAt = band.LastUpdatedAt,
                    LastUpdatedBy = band.LastUpdatedBy,
                    DeletedAt = band.DeletedAt,
                    DeletedBy = band.DeletedBy,

                    Name = band.Name,
                    Biography = band.Biography,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all bands");
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(BandResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BandResponseModel>> CreateBand([FromBody] BandCreationRequestModel newBandData)
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                var band = await _bandObjectService.CreateBandAsync(user!.Id, newBandData.Name, newBandData.Biography);

                if (band == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                    {
                        Detail = "Failed to create band"
                    });
                }

                return StatusCode(StatusCodes.Status201Created, new BandResponseModel
                {
                    Id = band.Id,
                    BandId = band.Id,
                    CreatedBy = band.CreatedBy,
                    CreatedAt = band.CreatedAt,
                    LastUpdatedAt = band.LastUpdatedAt,
                    LastUpdatedBy = band.LastUpdatedBy,
                    DeletedAt = band.DeletedAt,
                    DeletedBy = band.DeletedBy,

                    Name = band.Name,
                    Biography = band.Biography,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create band");
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpGet("{bandId}")]
        [ProducesResponseType(typeof(BandResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BandResponseModel>> GetBand(Guid bandId)
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

                var band = await _bandObjectService.GetBandAsync(bandId);
                if (band == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Band not found"
                    });
                }

                return Ok(new BandResponseModel
                {
                    Id = band.Id,
                    BandId = band.Id,
                    CreatedBy = band.CreatedBy,
                    CreatedAt = band.CreatedAt,
                    LastUpdatedAt = band.LastUpdatedAt,
                    LastUpdatedBy = band.LastUpdatedBy,
                    DeletedAt = band.DeletedAt,
                    DeletedBy = band.DeletedBy,

                    Name = band.Name,
                    Biography = band.Biography,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPatch("{bandId}")]
        [ProducesResponseType(typeof(BandResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BandResponseModel>> UpdateBand(
            Guid bandId,
            [FromBody] BandUpdateRequestModel updateData)
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

                if (updateData.Name == null && updateData.Biography == null)
                {
                    return BadRequest(new GenericFailResponseModel
                    {
                        Detail = "No fields provided for update"
                    });
                }

                var band = await _bandObjectService.UpdateBandAsync(bandId, updateData.Name, updateData.Biography);
                if (band == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Band not found"
                    });
                }

                return Ok(new BandResponseModel
                {
                    Id = band.Id,
                    BandId = band.Id,
                    CreatedBy = band.CreatedBy,
                    CreatedAt = band.CreatedAt,
                    LastUpdatedAt = band.LastUpdatedAt,
                    LastUpdatedBy = band.LastUpdatedBy,
                    DeletedAt = band.DeletedAt,
                    DeletedBy = band.DeletedBy,

                    Name = band.Name,
                    Biography = band.Biography,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }
    }
}
