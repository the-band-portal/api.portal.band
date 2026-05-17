using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Address;
using BandPortal.WebServices.API.Models.Venue;

namespace BandPortal.WebServices.API.Extensions
{
    public static class AddressObjectExtensions
    {
        public static AddressResponseModel ToResponseModel(this AddressEntityModel entity)
        {
            return new AddressResponseModel
            {
                Id = entity.Id,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedAt = entity.LastUpdatedAt,
                DeletedBy = entity.DeletedBy,
                DeletedAt = entity.DeletedAt,
                BandId = entity.BandId,

                Name = entity.Name,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                City = entity.City,
                County = entity.County,
                Country = entity.Country,
                Postcode = entity.Postcode,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                PrimaryContactId = entity.PrimaryContactId
            };
        }
    }
}
