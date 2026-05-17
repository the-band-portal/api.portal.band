using BandPortal.WebServices.API.Common.Configuration;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Common.Enumerators;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Common.Utilities;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.UserLogin;
using BandPortal.WebServices.API.Models.UserRefresh;
using BandPortal.WebServices.API.Models.UserRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandPortal.WebServices.API.Controllers
{
    [ApiController]
    [Route("/api/v2/authentication")]
    [Tags("Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ConfigStructure _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly BandPortalDbContext _db;
        private readonly ICaptchaService _captchaService;
        private readonly IPasswordService _passwordService;
        private readonly IJWTService _tokenService;

        public AuthenticationController(
            IConfiguration configuration,
            ILogger<AuthenticationController> logger,
            BandPortalDbContext db,
            ICaptchaService captchaService,
            IPasswordService passwordService,
            IJWTService tokenService
            )
        {
            _configuration = configuration.Get<ConfigStructure>()!;
            _logger = logger;
            _db = db;
            _captchaService = captchaService;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }



        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegistrationResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserRegistrationResponseModel>> Register(
            [FromBody] UserRegistrationRequestModel request
            )
        {
            try
            {
                // handle captcha
                if (string.IsNullOrEmpty(request.RecaptchaToken))
                    return StatusCode(StatusCodes.Status400BadRequest, new GenericFailResponseModel
                    {
                        Detail = "You must provide a captcha token."
                    });
                string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                await _captchaService.VerifyRecaptchaAsync(request.RecaptchaToken, clientIp);

                // check if email already exists
                var existingUser = await _db.Users
                    .FirstOrDefaultAsync(u => u.EmailAddress == request.EmailAddress);
                if (existingUser != null)
                    return StatusCode(StatusCodes.Status409Conflict, new GenericFailResponseModel
                    {
                        Detail = "The provided email address is already associated with a user account."
                    });

                // create user
                var userId = UUIDHandlingUtilities.GenerateV5(DatabaseObjectType.User);
                var passwordHash = _passwordService.HashPassword(request.RawPassword);
                var user = new UserEntityModel
                {
                    Id = userId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    EmailAddress = request.EmailAddress,
                    PasswordHash = passwordHash,
                    DisplayName = request.DisplayName,
                    AllowLogin = true,
                    HasEmailAddressBeenVerified = false,
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                // return
                return StatusCode(StatusCodes.Status201Created, new GenericObjectCreatedResponseModel
                {
                    ObjectId = userId.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = "An unknown error occured."
                });
            }
        }



        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserLoginResponseModel>> Login(
            [FromBody] UserLoginRequestModel request
            )
        {
            try
            {
                // handle captcha
                if (string.IsNullOrEmpty(request.RecaptchaToken))
                    return StatusCode(StatusCodes.Status400BadRequest, new GenericFailResponseModel
                    {
                        Detail = "You must provide a captcha token."
                    });
                string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                await _captchaService.VerifyRecaptchaAsync(request.RecaptchaToken, clientIp);

                // get the user from the database
                var user = await _db.Users
                    .FirstOrDefaultAsync(u => u.EmailAddress == request.EmailAddress);
                if (user == null)
                    return StatusCode(StatusCodes.Status409Conflict, new GenericFailResponseModel
                    {
                        Detail = "Incorrect credentials"
                    });
                if (user.AllowLogin == false)
                    return StatusCode(StatusCodes.Status409Conflict, new GenericFailResponseModel
                    {
                        Detail = "User is not allowed to login."
                    });
                if (!_passwordService.VerifyPassword(request.RawPassword, user.PasswordHash))
                    return StatusCode(StatusCodes.Status401Unauthorized, new GenericFailResponseModel
                    {
                        Detail = "Incorrect credentials."
                    });

                // create the jwts
                var userData = new Dictionary<string, object>
                {
                    ["id"] = user.Id
                };
                string accessToken = _tokenService.CreateAccessJWT(userData);
                string refreshToken = _tokenService.CreateRefreshJWT(userData);

                // delete expired tokens
                var expiredTokens = _db.RefreshTokens
                    .Where(rt => rt.CreatedBy == user.Id && rt.ExpiresAt < DateTime.UtcNow);
                _db.RefreshTokens.RemoveRange(expiredTokens);

                // store the new refresh token
                var refreshTokenId = UUIDHandlingUtilities.GenerateV5(DatabaseObjectType.RefreshToken);
                var tokenHash = HashingUtilities.SHA256Hash(refreshToken);
                var expiresAtTime = DateTime.UtcNow.AddDays(_configuration.JWT.RefreshTokenExpirationInDays);
                var refreshTokenEntity = new RefreshTokenEntityModel
                {
                    Id = refreshTokenId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Id,
                    TokenHash = tokenHash,
                    ExpiresAt = expiresAtTime
                };
                _db.RefreshTokens.Add(refreshTokenEntity);
                await _db.SaveChangesAsync();

                // return
                return StatusCode(StatusCodes.Status200OK, new UserLoginResponseModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresInMinutes = _configuration.JWT.AccessTokenExpirationInMinutes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = "An unknown error occured."
                });
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(UserLoginResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserLoginResponseModel>> Refresh(
            [FromBody] UserRefreshRequestModel request
            )
        {
            try
            {
                // hash the provided refresh token
                var tokenHash = HashingUtilities.SHA256Hash(request.RefreshToken);

                // verify the refresh token and get user
                var refreshToken = await _db.RefreshTokens
                    .Include(rt => rt.CreatedByUser)
                    .FirstOrDefaultAsync(rt =>
                        rt.TokenHash == tokenHash &&
                        rt.ExpiresAt > DateTime.UtcNow);

                if (refreshToken == null || refreshToken.CreatedByUser == null)
                    return StatusCode(StatusCodes.Status401Unauthorized, new GenericFailResponseModel
                    {
                        Detail = "Refresh token is either invalid or expired."
                    });

                var user = refreshToken.CreatedByUser;

                // create new access and refresh tokens
                var userData = new Dictionary<string, object>
                {
                    ["id"] = user.Id
                };
                var accessToken = _tokenService.CreateAccessJWT(userData);
                var newRefreshToken = _tokenService.CreateRefreshJWT(userData);

                // update the refresh token
                var newTokenHash = HashingUtilities.SHA256Hash(newRefreshToken);
                var newExpiresAt = DateTime.UtcNow.AddDays(_configuration.JWT.RefreshTokenExpirationInDays);

                refreshToken.TokenHash = newTokenHash;
                refreshToken.ExpiresAt = newExpiresAt;

                await _db.SaveChangesAsync();

                // return
                return StatusCode(StatusCodes.Status200OK, new UserLoginResponseModel
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken!,
                    ExpiresInMinutes = _configuration.JWT.AccessTokenExpirationInMinutes
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new GenericFailResponseModel
                {
                    Detail = "Unauthorised exception"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = "An error occurred during token refresh"
                });
            }
        }


        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(GenericSuccessResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GenericSuccessResponseModel>> Logout()
        {
            try
            {
                // grab the current user id from token
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return StatusCode(StatusCodes.Status401Unauthorized, new GenericFailResponseModel
                    {
                        Detail = "user id not found in token"
                    });

                if (!Guid.TryParse(userId, out var userGuid))
                    return StatusCode(StatusCodes.Status400BadRequest, new GenericFailResponseModel
                    {
                        Detail = "invalid user ID format"
                    });

                // delete all the refresh tokens for this user
                var tokens = _db.RefreshTokens.Where(rt => rt.CreatedBy == userGuid);
                _db.RefreshTokens.RemoveRange(tokens);

                await _db.SaveChangesAsync();

                _logger.LogInformation("User {UserId} logged out successfully", userGuid);

                return Ok(new GenericSuccessResponseModel());
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
