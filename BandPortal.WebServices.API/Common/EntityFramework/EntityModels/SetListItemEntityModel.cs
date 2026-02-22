namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class SetListItemEntityModel : BaseEntityModel
    {
        public required Guid SetListId { get; set; }
        public required Guid? TrackId { get; set; }






        public SetListEntityModel? SetList { get; set; }
        public TrackEntityModel? Track { get; set; }
    }
}
