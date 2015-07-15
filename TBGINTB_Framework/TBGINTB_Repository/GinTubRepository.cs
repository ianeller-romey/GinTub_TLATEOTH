using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMapper;

using GinTub.Repository.Entities;
using GinTub.Repository.Entities.Database;

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

        public IEnumerable<VerbType> LoadAllVerbTypes()
        {
            IEnumerable<VerbType> verbTypes;
            using (var entities = new GinTubEntities())
            {
                var verbTypeResults = entities.LoadAllVerbTypes();
                verbTypes = verbTypeResults.Select(v => TypeAdapter.Adapt<VerbType>(v)).ToList();
            }
            return verbTypes;
        }

        public AreaData LoadGame(Guid playerId)
        {
            Area area;
            RoomData roomData;
            using (var entities = new GinTubEntities())
            {
                var areaResults = entities.LoadGame(playerId);
                area = TypeAdapter.Adapt<Area>(areaResults.Single());

                var roomResults = areaResults.GetNextResult<LoadRoom_Result>();
                roomData = RoomResultsFromDB(roomResults);
            }
            return new AreaData(area, roomData.Item1, roomData.Item2, roomData.Item3);
        }

        public IEnumerable<Noun> GetNounsForParagraphState(int paragraphStateId)
        {
            IEnumerable<Noun> nouns;
            using(var entities = new GinTubEntities())
            {
                var nounResults = entities.LoadNounsForParagraphState(paragraphStateId);
                nouns = nounResults.Select(n => TypeAdapter.Adapt<Noun>(n)).ToList();
            }
            return nouns;
        }

        public IEnumerable<Result> GetActionResults(Guid playerId, int nounId, int verbTypeId)
        {
            IEnumerable<Result> results;
            using(var entities = new GinTubEntities())
            {
                var resultResults = entities.GetActionResults(playerId, nounId, verbTypeId);
                results = resultResults.Select(r => TypeAdapter.Adapt<Result>(r)).ToList();
            }
            return results;
        }

        public Message LoadMessage(int messageId)
        {
            Message message;
            using(var entities = new GinTubEntities())
            {
                var messageResult = entities.LoadMessageId(messageId);
                message = messageResult.Select(m => TypeAdapter.Adapt<Message>(m)).First();

                var messageChoiceResults = messageResult.GetNextResult<LoadMessageChoicesForMessage_Result>();
                if(messageChoiceResults != null)
                    message.MessageChoices = messageChoiceResults.Select(mc => TypeAdapter.Adapt<MessageChoice>(mc)).ToArray();
            }
            return message;
        }

        #endregion


        #region Private Functionality

        public RoomData RoomResultsFromDB(ObjectResult<LoadRoom_Result> roomResults)
        {
            Room room;
            IEnumerable<RoomState> roomStates;
            IEnumerable<ParagraphState> paragraphStates;

            room = roomResults.Select(x => TypeAdapter.Adapt<Room>(x)).FirstOrDefault();

            var roomStateResults = roomResults.GetNextResult<LoadRoomStatesForRoom_Result>();
            roomStates = roomStateResults.Select(x => TypeAdapter.Adapt<RoomState>(x)).ToList();

            var paragraphStateResults = roomStateResults.GetNextResult<LoadParagraphStatesForRoom_Result>();
            paragraphStates = paragraphStateResults.Select(x => TypeAdapter.Adapt<ParagraphState>(x)).ToList();

            var nounResults = paragraphStateResults.GetNextResult<LoadNounsForRoom_Result>();
            IEnumerable<Noun> nouns = nounResults.Select(x => TypeAdapter.Adapt<Noun>(x)).ToList();

            foreach (var paragraphState in paragraphStates)
                paragraphState.Nouns = nouns.Where(x => x.ParagraphState == paragraphState.Id).ToArray();

            return new RoomData(room, roomStates, paragraphStates);
        }

        #endregion

        #endregion
    }
}
