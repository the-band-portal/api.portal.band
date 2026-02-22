namespace BandPortal.WebServices.API.Common.EntityFramework.EntityModels
{
    public class NoteEntityModel : BaseEntityModel
    {
        public required string TableName { get; set; }
        public required Guid TableRowId { get; set; }
        public required string NoteContent { get; set; }
    }
}
