using BandPortal.WebServices.API.Common.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
