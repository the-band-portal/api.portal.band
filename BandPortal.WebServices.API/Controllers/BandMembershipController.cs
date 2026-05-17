using BandPortal.WebServices.API.Common.ControllerBases;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Extensions;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.BandMembership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandPortal.WebServices.API.Controllers
{
    [ApiController]
    [Route("/api/v2/bands/{bandId}/members")]
    [Tags("Band Members")]
    [Authorize]
    public class BandMembershipController : LoggedInControllerBase
    {
        private readonly ILogger<BandMembershipController> _logger;
        private readonly IBandMembershipObjectService _bandMembershipService;
        private readonly IBandObjectService _bandObjectService;

        public BandMembershipController(
            ILogger<BandMembershipController> logger,
            IBandMembershipObjectService bandMembershipService,
            IBandObjectService bandObjectService,
            BandPortalDbContext db
        )
            : base(db, logger)
        {
            _logger = logger;
            _bandMembershipService = bandMembershipService;
            _bandObjectService = bandObjectService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<BandMembershipResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<BandMembershipResponseModel>>> GetAllMembers(Guid bandId)
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

                var members = await _bandMembershipService.GetAllMembersAsync(bandId);
                var response = members.Select(m => m.ToResponseModel()).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all members for band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(BandMembershipResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BandMembershipResponseModel>> GetMyMembership(Guid bandId)
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

                var membership = await _bandMembershipService.GetMyMembershipAsync(bandId, user.Id);
                if (membership == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Your membership in this band not found"
                    });
                }

                return Ok(membership.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get membership for user in band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpGet("{membershipId}")]
        [ProducesResponseType(typeof(BandMembershipResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BandMembershipResponseModel>> GetMember(Guid bandId, Guid membershipId)
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

                var membership = await _bandMembershipService.GetMembershipByIdAsync(bandId, membershipId);
                if (membership == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Band member not found"
                    });
                }

                return Ok(membership.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get membership {MembershipId} for band {BandId}", membershipId, bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }
    }
}
