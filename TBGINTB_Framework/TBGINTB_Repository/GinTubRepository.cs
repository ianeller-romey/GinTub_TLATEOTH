using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMapper;

using GinTub.Repository.Entities;
using GinTub.Repository.Entities.Database;

using GameData = System.Tuple<System.TimeSpan, GinTub.Repository.Entities.Area, GinTub.Repository.Entities.Room, System.Collections.Generic.IEnumerable<GinTub.Repository.Entities.RoomState>, System.Collections.Generic.IEnumerable<GinTub.Repository.Entities.ParagraphState>>;
using AreaData = System.Tuple<GinTub.Repository.Entities.Area, GinTub.Repository.Entities.Room, System.Collections.Generic.IEnumerable<GinTub.Repository.Entities.RoomState>, System.Collections.Generic.IEnumerable<GinTub.Repository.Entities.ParagraphState>>;
using RoomData = System.Tuple<GinTub.Repository.Entities.Room, System.Collections.Generic.IEnumerable<GinTub.Repository.Entities.RoomState>, System.Collections.Generic.IEnumerable<GinTub.Repository.Entities.ParagraphState>>;


namespace GinTub.Repository
{
    public class GinTubRepository : Interface.IGinTubRepository
    {
        #region MEMBER FIELDS
        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Guid? PlayerLogin(string userName, string domainName, string domain, string password)
        {
            Guid? playerId = null;
            using (var entities = new GinTubEntities())
            {
                var results = entities.PlayerLogin(userName, domainName, domain, password);
                playerId = results.OfType<Guid?>().SingleOrDefault();
            }
            return playerId;
        }

        public IEnumerable<VerbType> ReadAllVerbTypes()
        {
            IEnumerable<VerbType> verbTypes = null;
            using (var entities = new GinTubEntities())
            {
                var verbTypeResults = entities.ReadAllVerbTypes();
                verbTypes = verbTypeResults.Select(v => TypeAdapter.Adapt<VerbType>(v)).ToList();
            }
            return verbTypes;
        }

        public IEnumerable<ResultType> ReadAllResultTypes()
        {
            IEnumerable<ResultType> resultTypes = null;
            using (var entities = new GinTubEntities())
            {
                var resultTypeResults = entities.ReadAllResultTypes();
                resultTypes = resultTypeResults.Select(v => TypeAdapter.Adapt<ResultType>(v)).ToList();
            }
            return resultTypes;
        }

        public IEnumerable<Audio> ReadAllAudio()
        {
            IEnumerable<Audio> audio = null;
            using (var entities = new GinTubEntities())
            {
                var audioResults = entities.ReadAllAudio();
                audio = audioResults.Select(v => TypeAdapter.Adapt<Audio>(v)).ToList();
            }
            return audio;
        }

        public GameData ReadGame(Guid playerId)
        {
            GameData gameData = null;
            using (var entities = new GinTubEntities())
            {
                var lastTimeResults = entities.ReadGame(playerId);
                gameData = GameAndAreaAndRoomResultsFromDb(lastTimeResults);
            }
            return gameData;
        }

        public Message ReadMessage(int messageId)
        {
            Message message = null;
            using(var entities = new GinTubEntities())
            {
                var messageResult = entities.ReadMessageForPlayer(messageId);
                message = messageResult.Select(m => TypeAdapter.Adapt<Message>(m)).First();

                var messageChoiceResults = messageResult.GetNextResult<ReadMessageChoicesForMessage_Result>();
                if(messageChoiceResults != null)
                    message.MessageChoices = messageChoiceResults.Select(mc => TypeAdapter.Adapt<MessageChoice>(mc)).ToArray();
            }
            return message;
        }

        public IEnumerable<Noun> ReadNounsForParagraphState(int paragraphStateId)
        {
            IEnumerable<Noun> nouns = null;
            using(var entities = new GinTubEntities())
            {
                var nounResults = entities.ReadNounsForParagraphState(paragraphStateId);
                nouns = nounResults.Select(n => TypeAdapter.Adapt<Noun>(n)).ToList();
            }
            return nouns;
        }

        public Tuple<Area, IEnumerable<MapEntry>> ReadMapForPlayer(Guid playerId, int areaId)
        {
            Area area = null;
            IEnumerable<MapEntry> results = null;
            using (var entities = new GinTubEntities())
            {
                var areaResults = entities.ReadAreaForPlayer(areaId);
                area = areaResults.Select(a => TypeAdapter.Adapt<Area>(a)).FirstOrDefault();
                var resultResults = entities.ReadMapForPlayer(playerId, areaId);
                results = resultResults.Select(r => TypeAdapter.Adapt<MapEntry>(r)).ToList();
            }
            return new Tuple<Area,IEnumerable<MapEntry>>(area, results);
        }

        public IEnumerable<InventoriesEntry> ReadInventoryForPlayer(Guid playerId)
        {
            IEnumerable<InventoriesEntry> results = null;
            using (var entities = new GinTubEntities())
            {
                var resultResults = entities.ReadInventoryForPlayer(playerId);
                results = resultResults.Select(r => TypeAdapter.Adapt<InventoriesEntry>(r)).ToList();
            }
            return results;
        }

        public IEnumerable<InventoriesEntry> ReadHistoryForPlayer(Guid playerId)
        {
            IEnumerable<InventoriesEntry> results = null;
            using (var entities = new GinTubEntities())
            {
                var resultResults = entities.ReadHistoryForPlayer(playerId);
                results = resultResults.Select(r => TypeAdapter.Adapt<InventoriesEntry>(r)).ToList();
            }
            return results;
        }

        public IEnumerable<InventoriesEntry> ReadPartyForPlayer(Guid playerId)
        {
            IEnumerable<InventoriesEntry> results = null;
            using (var entities = new GinTubEntities())
            {
                var resultResults = entities.ReadPartyForPlayer(playerId);
                results = resultResults.Select(r => TypeAdapter.Adapt<InventoriesEntry>(r)).ToList();
            }
            return results;
        }

        public Task UpdateLastTime(Guid playerId, int nounId, int verbTypeId, TimeSpan time)
        {
            return Task.Run(() =>
                {                    
                    using (var entities = new GinTubEntities())
                    {
                        entities.UpdateLastTimeForPlayer(playerId, nounId, verbTypeId, time);
                    }
                });
        }

        public IEnumerable<Result> GetActionResults(Guid playerId, int nounId, int verbTypeId)
        {
            IEnumerable<Result> results = null;
            using(var entities = new GinTubEntities())
            {
                var resultResults = entities.GetActionResults(playerId, nounId, verbTypeId);
                results = resultResults.Select(r => TypeAdapter.Adapt<Result>(r)).ToList();
            }
            return results;
        }

        public IEnumerable<Result> GetMessageChoiceResults(int messageChoiceId)
        {
            IEnumerable<Result> results = null;
            using (var entities = new GinTubEntities())
            {
                var resultResults = entities.GetMessageChoiceResults(messageChoiceId);
                results = resultResults.Select(r => TypeAdapter.Adapt<Result>(r)).ToList();
            }
            return results;
        }

        public RoomData PlayerMoveXYZ(Guid playerId, int xDir, int yDir, int zDir)
        {
            RoomData roomData = null;
            using(var entities = new GinTubEntities())
            {
                var roomResults = entities.PlayerMoveXYZ(playerId, xDir, yDir, zDir);
                roomData = RoomResultsFromDB(roomResults);
            }
            return roomData;
        }

        public RoomData PlayerTeleportRoomXYZ(Guid playerId, int xPos, int yPos, int zPos)
        {
            RoomData roomData = null;
            using (var entities = new GinTubEntities())
            {
                var roomResults = entities.PlayerTeleportRoomXYZ(playerId, xPos, yPos, zPos);
                roomData = RoomResultsFromDB(roomResults);
            }
            return roomData;
        }

        public RoomData PlayerTeleportRoomID(Guid playerId, int roomId)
        {
            RoomData roomData = null;
            using (var entities = new GinTubEntities())
            {
                var roomResults = entities.PlayerTeleportRoomId(playerId, roomId);
                roomData = RoomResultsFromDB(roomResults);
            }
            return roomData;
        }

        public AreaData PlayerTeleportAreaIdRoomXYZ(Guid playerId, int areaId, int xPos, int yPos, int zPos)
        {
            AreaData areaData = null;
            using (var entities = new GinTubEntities())
            {
                var areaResults = entities.PlayerTeleportAreaIdRoomXYZ(playerId, areaId, xPos, yPos, zPos);
                areaData = AreaAndRoomResultsFromDb(areaResults);
            }
            return areaData;
        }

        public AreaData PlayerTeleportAreaIdRoomId(Guid playerId, int areaId, int roomId)
        {
            AreaData areaData = null;
            using (var entities = new GinTubEntities())
            {
                var areaResults = entities.PlayerTeleportAreaIdRoomId(playerId, areaId, roomId);
                areaData = AreaAndRoomResultsFromDb(areaResults);
            }
            return areaData;
        }

        public void PlayerItemAdd(Guid playerId, int itemId)
        {
            using(var entities = new GinTubEntities())
            {
                entities.PlayerItemAdd(playerId, itemId);
            }
        }

        public void PlayerEventAdd(Guid playerId, int eventId)
        {
            using (var entities = new GinTubEntities())
            {
                entities.PlayerEventAdd(playerId, eventId);
            }
        }

        public void PlayerCharacterAdd(Guid playerId, int characterId)
        {
            using (var entities = new GinTubEntities())
            {
                entities.PlayerCharacterAdd(playerId, characterId);
            }
        }

        public RoomData PlayerParagraphStateChange(Guid playerId, int paragraphId, int state)
        {
            RoomData roomData = null;
            using(var entities = new GinTubEntities())
            {
                var roomResults = entities.PlayerParagraphStateChange(playerId, paragraphId, state);
                roomData = RoomResultsFromDB(roomResults);
            }
            return roomData;
        }

        public RoomData PlayerRoomStateChange(Guid playerId, int roomId, int state)
        {
            RoomData roomData = null;
            using (var entities = new GinTubEntities())
            {
                var roomResults = entities.PlayerRoomStateChange(playerId, roomId, state);
                roomData = RoomResultsFromDB(roomResults);
            }
            return roomData;
        }

        #endregion


        #region Private Functionality

        public GameData GameAndAreaAndRoomResultsFromDb(ObjectResult<ReadLastTimeForPlayer_Result> lastTimeResults)
        {
            TimeSpan time = TimeSpan.MinValue;
            AreaData areaData = null;

            time = TypeAdapter.Adapt<TimeSpan>(lastTimeResults.Single().LastTime);

            var areaResults = lastTimeResults.GetNextResult<ReadArea_Result>();
            areaData = AreaAndRoomResultsFromDb(areaResults);

            return new GameData(time, areaData.Item1, areaData.Item2, areaData.Item3, areaData.Item4);
        }

        public AreaData AreaAndRoomResultsFromDb(ObjectResult<ReadArea_Result> areaResults)
        {
            Area area = null;
            RoomData roomData = null;

            area = TypeAdapter.Adapt<Area>(areaResults.Single());

            var roomResults = areaResults.GetNextResult<ReadRoom_Result>();
            roomData = RoomResultsFromDB(roomResults);

            return new AreaData(area, roomData.Item1, roomData.Item2, roomData.Item3);
        }

        public RoomData RoomResultsFromDB(ObjectResult<ReadRoom_Result> roomResults)
        {
            Room room = null;
            IEnumerable<RoomState> roomStates = null;
            IEnumerable<ParagraphState> paragraphStates = null;

            room = roomResults.Select(x => TypeAdapter.Adapt<Room>(x)).FirstOrDefault();

            var roomStateResults = roomResults.GetNextResult<ReadRoomStatesForPlayerRoom_Result>();
            roomStates = roomStateResults.Select(x => TypeAdapter.Adapt<RoomState>(x)).ToList();

            var paragraphStateResults = roomStateResults.GetNextResult<ReadParagraphStatesForPlayerRoom_Result>();
            paragraphStates = paragraphStateResults.Select(x => TypeAdapter.Adapt<ParagraphState>(x)).ToList();

            var nounResults = paragraphStateResults.GetNextResult<ReadNounsForPlayerRoom_Result>();
            IEnumerable<Noun> nouns = nounResults.Select(x => TypeAdapter.Adapt<Noun>(x)).ToList();

            foreach (var paragraphState in paragraphStates)
                paragraphState.Nouns = nouns.Where(x => x.ParagraphState == paragraphState.Id).ToArray();

            return new RoomData(room, roomStates, paragraphStates);
        }

        #endregion

        #endregion
    }
}
