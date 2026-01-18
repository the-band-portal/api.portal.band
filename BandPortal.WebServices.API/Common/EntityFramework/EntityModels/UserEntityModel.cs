namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class UserEntityModel
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }





        public required string EmailAddress { get; set; }
        public required string DisplayName { get; set; }
        public required string PasswordHash { get; set; }
        public required DateTime? LastLogin { get; set; }
        public required bool HasEmailAddressBeenVerified { get; set; }
        public required bool AllowLogin { get; set; }





        public UserEntityModel? CreatedByUser { get; set; }
        public UserEntityModel? LastUpdatedByUser { get; set; }
        public UserEntityModel? DeletedByUser { get; set; }
    }
}
