using BandPortal.WebServices.API.Common.EntityFramework.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandPortal.WebServices.API.Models.BandMembership
{
    public class BandMembershipResponseModel : BaseResponseModel
    {
        [Required]
        [JsonPropertyName("userId")]
        public required Guid UserId { get; init; }


        [Required]
        [JsonPropertyName("role")]
        public required BandMembershipRole Role { get; init; }
    }
}
