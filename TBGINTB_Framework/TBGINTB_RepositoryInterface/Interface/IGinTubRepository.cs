using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GinTub.Repository.Entities;


namespace GinTub.Repository.Interface
{
    public interface IGinTubRepository
    {
        Guid? PlayerLogin(string userName, string domainName, string domain, string password);

        IEnumerable<VerbType> ReadAllVerbTypes();

        IEnumerable<ResultType> ReadAllResultTypes();

        IEnumerable<Audio> ReadAllAudio();

        Tuple<TimeSpan, Area, Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> ReadGame(Guid playerId);

        Message ReadMessage(int messageId);

        IEnumerable<Noun> ReadNounsForParagraphState(int paragraphStateId);

        Tuple<Area, IEnumerable<MapEntry>> ReadMapForPlayer(Guid playerId, int areaId);

        IEnumerable<InventoriesEntry> ReadInventoryForPlayer(Guid playerId);

        IEnumerable<InventoriesEntry> ReadHistoryForPlayer(Guid playerId);

        IEnumerable<InventoriesEntry> ReadPartyForPlayer(Guid playerId);

        Task UpdateLastTime(Guid playerId, int nounId, int verbTypeId, TimeSpan time);

        IEnumerable<Result> GetActionResults(Guid playerId, int nounId, int verbTypeId);

        IEnumerable<Result> GetMessageChoiceResults(int messageChoiceId);
        
        Tuple<Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> PlayerMoveXYZ(Guid playerId, int xDir, int yDir, int zDir);

        Tuple<Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> PlayerTeleportRoomXYZ(Guid playerId, int xPos, int yPos, int zPos);

        Tuple<Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> PlayerTeleportRoomID(Guid playerId, int roomId);

        Tuple<Area, Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> PlayerTeleportAreaIdRoomXYZ(Guid playerId, int areaId, int xPos, int yPos, int zPos);

        Tuple<Area, Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> PlayerTeleportAreaIdRoomId(Guid playerId, int areaId, int roomId);

        void PlayerItemAdd(Guid playerId, int itemId);

        void PlayerEventAdd(Guid playerId, int eventId);

        void PlayerCharacterAdd(Guid playerId, int characterId);

        Tuple<Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> PlayerParagraphStateChange(Guid playerId, int paragraphId, int state);

        Tuple<Room, IEnumerable<RoomState>, IEnumerable<ParagraphState>> PlayerRoomStateChange(Guid playerId, int roomId, int state);
    }
}
