using BandPortal.WebServices.API.Common.ControllerBases;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandPortal.WebServices.API.Controllers
{

    [ApiController]
    [Route("/api/v2/users")]
    [Tags("Users")]
    public class UserController : LoggedInControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserObjectService _userDocService;

        public UserController(
            ILogger<UserController> logger,
            IUserObjectService userDocService,
            BandPortalDbContext db
            )
            : base(db, logger)
        {
            _logger = logger;

            _userDocService = userDocService;
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(GenericSuccessResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GenericSuccessResponseModel>> GetMe()
        {
            try
            {
                var (user, error) = await GetCurrentUserAsync();
                if (error != null) return error;

                return StatusCode(StatusCodes.Status200OK, new UserResponseModel
                {
                    Id = user!.Id,
                    EmailAddress = user!.EmailAddress,
                    DisplayName = user!.DisplayName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = "unexpected error occured"
                });
            }
        }
    }
}
