using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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

        public AreaData ReadGame(Guid playerId)
        {
            Area area = null;
            RoomData roomData = null;
            using (var entities = new GinTubEntities())
            {
                var areaResults = entities.ReadGame(playerId);
                area = TypeAdapter.Adapt<Area>(areaResults.Single());

                var roomResults = areaResults.GetNextResult<ReadRoom_Result>();
                roomData = RoomResultsFromDB(roomResults);
            }
            return new AreaData(area, roomData.Item1, roomData.Item2, roomData.Item3);
        }

        public IEnumerable<Noun> GetNounsForParagraphState(int paragraphStateId)
        {
            IEnumerable<Noun> nouns = null;
            using(var entities = new GinTubEntities())
            {
                var nounResults = entities.ReadNounsForParagraphState(paragraphStateId);
                nouns = nounResults.Select(n => TypeAdapter.Adapt<Noun>(n)).ToList();
            }
            return nouns;
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

        #endregion


        #region Private Functionality

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
