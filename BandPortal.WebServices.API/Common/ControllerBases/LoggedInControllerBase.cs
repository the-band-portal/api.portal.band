using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BandPortal.WebServices.API.Common.ControllerBases
{
    [Authorize]
    public class LoggedInControllerBase : ControllerBase
    {
        protected readonly BandPortalDbContext Db;
        protected readonly ILogger Logger;

        protected LoggedInControllerBase(BandPortalDbContext db, ILogger logger)
        {
            Db = db;
            Logger = logger;
        }


        protected async Task<(UserEntityModel? user, ActionResult? error)> GetCurrentUserAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                Logger.LogWarning("User ID not found in JWT token");
                return (null, Unauthorized(new GenericFailResponseModel
                {
                    Detail = "User ID not found in token"
                }));
            }

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                Logger.LogWarning("Invalid user ID format in token: {UserId}", userIdClaim);
                return (null, BadRequest(new GenericFailResponseModel
                {
                    Detail = "Invalid user ID format"
                }));
            }

            var user = await Db.Users
                .AsNoTracking()  // read-only query for better performance
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                Logger.LogWarning("User {UserId} not found in database", userId);
                return (null, NotFound(new GenericFailResponseModel
                {
                    Detail = "User not found in database"
                }));
            }

            return (user, null);
        }
    }
}
