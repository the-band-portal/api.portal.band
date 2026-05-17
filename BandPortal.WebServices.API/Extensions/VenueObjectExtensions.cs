using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using BandPortal.WebServices.API.Models.Stage;

namespace BandPortal.WebServices.API.Extensions
{
    public static class VenueObjectExtensions
    {
        public static StageResponseModel ToResponseModel(this StageEntityModel entity)
        {
            return new StageResponseModel
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
                AddressId = entity.AddressId,
                CapacityDetails = entity.CapacityDetails,
                StageDetails = entity.StageDetails,
                ParkingInstructions = entity.ParkingInstructions,
                LoadInInstructions = entity.LoadInInstructions,
                LoadOutInstructions = entity.LoadOutInstructions,
                PrimaryContactId = entity.PrimaryContactId
            };
        }
    }
}
