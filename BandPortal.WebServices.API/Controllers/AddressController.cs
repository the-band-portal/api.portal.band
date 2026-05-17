using BandPortal.WebServices.API.Common.ControllerBases;
using BandPortal.WebServices.API.Common.EntityFramework;
using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Common.Services;
using BandPortal.WebServices.API.Extensions;
using BandPortal.WebServices.API.Models;
using BandPortal.WebServices.API.Models.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandPortal.WebServices.API.Controllers
{
    [ApiController]
    [Route("/api/v2/bands/{bandId:guid}/addresses")]
    [Tags("Addresses")]
    [Authorize]
    public class AddressController : LoggedInControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressService _addressService;
        private readonly IBandObjectService _bandObjectService;

        public AddressController(
            ILogger<AddressController> logger,
            IAddressService addressService,
            IBandObjectService bandObjectService,
            BandPortalDbContext db
        )
            : base(db, logger)
        {
            _logger = logger;
            _addressService = addressService;
            _bandObjectService = bandObjectService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<AddressResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AddressResponseModel>>> GetAllAddresses(Guid bandId)
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

                var Addresses = await _addressService.GetAllAsync(bandId);
                var response = Addresses.Select(v => v.ToResponseModel()).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all Addresses for band {BandId}", bandId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpGet("{addressId}")]
        [ProducesResponseType(typeof(AddressResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressResponseModel>> GetAddress(Guid bandId, Guid addressId)
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

                var address = await _addressService.GetByIdAsync(addressId, bandId);
                if (address == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Address not found"
                    });
                }

                return Ok(address.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get address {AddressId}", addressId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(AddressResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressResponseModel>> CreateAddress(
            Guid bandId,
            [FromBody] AddressCreationRequestModel newAddressData)
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

                var address = new AddressEntityModel
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    BandId = bandId,
                    CreatedBy = user.Id,
                    LastUpdatedBy = user.Id,
                    Name = newAddressData.Name,
                    AddressLine1 = newAddressData.AddressLine1,
                    AddressLine2 = newAddressData.AddressLine2,
                    City = newAddressData.City,
                    County = newAddressData.County,
                    Country = newAddressData.Country,
                    Postcode = newAddressData.Postcode,
                    Latitude = newAddressData.Latitude,
                    Longitude = newAddressData.Longitude,
                };

                var createdAddress = await _addressService.CreateAsync(address);
                if (createdAddress == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                    {
                        Detail = "Failed to create address"
                    });
                }

                return StatusCode(StatusCodes.Status201Created, createdAddress.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create address");
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpPatch("{addressId}")]
        [ProducesResponseType(typeof(AddressResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddressResponseModel>> UpdateAddress(
            Guid bandId,
            Guid addressId,
            [FromBody] AddressUpdateRequestModel updateData)
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

                var address = await _addressService.UpdateAsync(addressId, bandId, updateData);
                if (address == null)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Address not found"
                    });
                }

                return Ok(address.ToResponseModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update address {AddressId}", addressId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }

        [HttpDelete("{addressId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericFailResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAddress(Guid bandId, Guid addressId)
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

                var deleted = await _addressService.SoftDeleteAsync(addressId, bandId, user.Id);
                if (!deleted)
                {
                    return NotFound(new GenericFailResponseModel
                    {
                        Detail = "Address not found or already deleted"
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete address {AddressId}", addressId);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericFailResponseModel
                {
                    Detail = $"Database error: {ex.Message}"
                });
            }
        }
    }
}
