using BandPortal.WebServices.API.Common.Enumerators;
using System.Security.Cryptography;
using System.Text;

namespace BandPortal.WebServices.API.Common.Utilities
{
    public class UUIDHandlingUtilities
    {
        private static readonly Dictionary<DatabaseObjectType, string> NamespacePaths = new()
        {
            [DatabaseObjectType.Artist]                 = "/band_portal/database_object/artist",
            [DatabaseObjectType.BandMembership]         = "/band_portal/database_object/band_membership",
            [DatabaseObjectType.Band]                   = "/band_portal/database_object/band",
            [DatabaseObjectType.Contact]                = "/band_portal/database_object/contact",
            [DatabaseObjectType.Dividend]               = "/band_portal/database_object/dividend",
            [DatabaseObjectType.GigAvailability]        = "/band_portal/database_object/gig_availability",
            [DatabaseObjectType.Gig]                    = "/band_portal/database_object/gig",
            [DatabaseObjectType.Invoice]                = "/band_portal/database_object/invoice",
            [DatabaseObjectType.Note]                   = "/band_portal/database_object/note",
            [DatabaseObjectType.PasswordResetToken]     = "/band_portal/database_object/password_reset_token",
            [DatabaseObjectType.Quote]                  = "/band_portal/database_object/quote",
            [DatabaseObjectType.RefreshToken]           = "/band_portal/database_object/refresh_token",
            [DatabaseObjectType.SetListItem]            = "/band_portal/database_object/set_list_item",
            [DatabaseObjectType.SetList]                = "/band_portal/database_object/set_list",
            [DatabaseObjectType.Track]                  = "/band_portal/database_object/track",
            [DatabaseObjectType.User]                   = "/band_portal/database_object/user",
            [DatabaseObjectType.VenueEquipment]         = "/band_portal/database_object/venue_equipment",
            [DatabaseObjectType.Venue]                  = "/band_portal/database_object/venue",
        };


        private static readonly Dictionary<DatabaseObjectType, Guid> NamespaceUuids =
            NamespacePaths.ToDictionary(
                kvp => kvp.Key,
                kvp => PathToUuid(kvp.Value)
            );

        public static Guid GenerateV5(DatabaseObjectType objectType)
        {
            var namespaceId = NamespaceUuids[objectType];
            var name = $"{objectType}_{DateTime.UtcNow.Ticks}_{Guid.NewGuid()}";

            return GenerateV5(namespaceId, name);
        }

        public static Guid GenerateV5(Guid namespaceId, string name)
        {
            var namespaceBytes = namespaceId.ToByteArray();
            var nameBytes = Encoding.UTF8.GetBytes(name);

            SwapByteOrder(namespaceBytes);

            var hash = SHA1.HashData(namespaceBytes.Concat(nameBytes).ToArray());

            var newGuid = new byte[16];
            Array.Copy(hash, 0, newGuid, 0, 16);

            newGuid[6] = (byte)(newGuid[6] & 0x0F | 0x50);
            newGuid[8] = (byte)(newGuid[8] & 0x3F | 0x80);

            SwapByteOrder(newGuid);

            return new Guid(newGuid);
        }

        private static Guid PathToUuid(string path)
        {
            var hash = SHA1.HashData(Encoding.UTF8.GetBytes(path));

            var uuid = new byte[16];
            Array.Copy(hash, 0, uuid, 0, 16);

            uuid[6] = (byte)(uuid[6] & 0x0F | 0x50);
            uuid[8] = (byte)(uuid[8] & 0x3F | 0x80);

            return new Guid(uuid);
        }

        private static void SwapByteOrder(byte[] guid)
        {
            SwapBytes(guid, 0, 3);
            SwapBytes(guid, 1, 2);
            SwapBytes(guid, 4, 5);
            SwapBytes(guid, 6, 7);
        }

        private static void SwapBytes(byte[] guid, int left, int right)
        {
            (guid[left], guid[right]) = (guid[right], guid[left]);
        }
    }
}
