namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class TrackEntityModel
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }





        public required string Title { get; set; }
        public Guid? ArtistContactId { get; set; }
        public long? DurationInSeconds { get; set; }
        public string? TimeSignature { get; set; }
        public string? KeySignature { get; set; }
        public string? TempoInBpm { get; set; }
        public string? Genre { get; set; }






        public UserEntityModel? CreatedByUser { get; set; }
        public UserEntityModel? LastUpdatedByUser { get; set; }
        public UserEntityModel? DeletedByUser { get; set; }






        public ContactEntityModel? ArtistContact { get; set; }
    }
}
