namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class TrackEntityModel : BaseEntityModel
    {
        public required string Title { get; set; }
        public Guid? ArtistContactId { get; set; }
        public long? DurationInSeconds { get; set; }
        public string? TimeSignature { get; set; }
        public string? KeySignature { get; set; }
        public string? TempoInBpm { get; set; }
        public string? Genre { get; set; }






        public ContactEntityModel? ArtistContact { get; set; }
    }
}
