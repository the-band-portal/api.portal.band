namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class RefreshTokenEntityModel
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }





        public required string TokenHash { get; set; }
        public required DateTime ExpiresAt { get; set; }






        public UserEntityModel? CreatedByUser { get; set; }
    }
}
