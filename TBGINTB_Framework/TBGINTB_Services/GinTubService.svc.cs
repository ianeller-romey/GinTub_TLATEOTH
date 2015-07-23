using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;

using FastMapper;

using Newtonsoft.Json;

using GinTub;
using GinTub.Repository.Interface;
using DC = GinTub.Services.DataContracts;


namespace GinTub.Services
{
    public class GinTubService : OperationContracts.IGinTubService
    {
        #region MEMBER FIELDS

        private delegate void ResultHandler(IGinTubRepository repository, dynamic obj, Guid playerId, ref DC.Responses.PlayData playData);

        private IGinTubRepository _repository;
        private static readonly ConcurrentDictionary<string, ResultHandler> _resultHandlers =
            new ConcurrentDictionary<string, ResultHandler>
            (
                new KeyValuePair<string, ResultHandler>[]
                {
                    new KeyValuePair<string, ResultHandler>("Room XYZ Movement", Result_RoomXYZMovement),
                    new KeyValuePair<string, ResultHandler>("Room XYZ Teleport", Result_RoomXYZTeleport),
                    new KeyValuePair<string, ResultHandler>("Room Id Teleport", Result_RoomIdTeleport),
                    new KeyValuePair<string, ResultHandler>("Area Id Room XYZ Teleport", Result_AreaIdRoomXYZTeleport),
                    new KeyValuePair<string, ResultHandler>("Area Id Room Id Teleport", Result_AreaIdRoomIdTeleport),
                    new KeyValuePair<string, ResultHandler>("Item Acquisition", Result_ItemAcquisition),
                    new KeyValuePair<string, ResultHandler>("Event Acquisition", Result_EventAcquisition),
                    new KeyValuePair<string, ResultHandler>("Character Acquisition", Result_CharacterAcquisition),
                    new KeyValuePair<string, ResultHandler>("Paragraph State Change", Result_ParagraphStateChange),
                    new KeyValuePair<string, ResultHandler>("Room State Change", Result_RoomStateChange),
                    new KeyValuePair<string, ResultHandler>("Message Activation", Result_MessageActivation),
                }
            );


        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public GinTubService()
        {
            _repository = new Repository.GinTubRepository();
        }

        public DC.Responses.PlayerIdentifier PlayerLogin(DC.Requests.PlayerLoginRequest request)
        {
            string
                emailAddress = request.EmailAddress,
                password = request.Password;
            // I know, catching Exceptions is not "best practice" for data validation,
            // but this is an easy way to avoid Regex (for now)
            try
            {
                var m = new MailAddress(emailAddress);
            }
            catch (FormatException)
            {
                return null;
            }

            // If we've made it past the validation, it's safe to brute-force parse the email address
            string userName, domainName, domain;

            int atIndex = emailAddress.IndexOf('@');

            userName = emailAddress.Substring(0, atIndex);

            emailAddress = emailAddress.Remove(0, atIndex + 1);

            int periodIndex = emailAddress.LastIndexOf('.');

            domainName = emailAddress.Substring(0, periodIndex);

            emailAddress = emailAddress.Remove(0, periodIndex + 1);

            domain = emailAddress;

            Guid? playerId = _repository.PlayerLogin(userName, domainName, domain, password);
            return new DC.Responses.PlayerIdentifier() { PlayerId = playerId };
        }

        public DC.Responses.VerbUseData GetAllVerbTypes()
        {
            var result = _repository.ReadAllVerbTypes();
            return new DC.Responses.VerbUseData()
                {
                    VerbTypes = result.Select(v => TypeAdapter.Adapt<DC.Responses.VerbTypeData>(v)).ToList()
                };
        }

        public DC.Responses.PlayData LoadGame(DC.Requests.LoadGameRequest request)
        {
            var result = _repository.ReadGame(request.PlayerId);
            return new DC.Responses.PlayData()
                {
                    Area = TypeAdapter.Adapt<DC.Responses.AreaData>(result.Item1),
                    Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item2),
                    RoomStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList(),
                    ParagraphStates = result.Item4.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList()
                };
        }

        public DC.Responses.PlayData GetNounsForParagraphState(string paragraphStateId)
        {
            DC.Responses.PlayData data = new DC.Responses.PlayData();

            int paragraphStateIntId;
            if (int.TryParse(paragraphStateId, out paragraphStateIntId))
            {
                var result = _repository.ReadNounsForParagraphState(paragraphStateIntId);
                data.Message =
                    new DC.Responses.MessageData()
                    {
                        Id = -1,
                        Text =
                            (result.Any())
                            ?
                                string.Format
                                (
                                    "You take special notice of the {0}.",
                                    result.Select(n => n.Text).Aggregate((x, y) => string.Format("{0}, {1}", x, y))
                                )
                            : "Nothing in particular catches your eye here."
                    };
            }

            return data;
        }

        public DC.Responses.PlayData DoAction(DC.Requests.DoActionRequest request)
        {
           DC.Responses.PlayData playData = 
                new DC.Responses.PlayData()
                {
                    Message = new DC.Responses. MessageData()
                    {
                        Id = -1,
                        Text = "Nothing happens."
                    }
                };

           if (request.NounId.HasValue)
           {
               var results = _repository.GetActionResults(request.PlayerId, request.NounId.Value, request.VerbTypeId);
               if (results.Any())
               {
                   results = ResultTypeDictionary.SortResultsByResultTypePriority(results);
                   playData.Message = null;
                   foreach (var result in results)
                   {
                       dynamic data = JsonConvert.DeserializeObject(result.JSONData);
                       ResultSwitch(result.ResultType, data, request.PlayerId, ref playData);
                   }
               }
           }

           return playData;
        }

        public DC.Responses.PlayData DoMessageChoice(DC.Requests.DoMessageChoiceRequest request)
        {
            DC.Responses.PlayData playData = new DC.Responses.PlayData();

            var results = _repository.GetMessageChoiceResults(request.MessageChoiceId);
            if (results.Any())
            {
                results = ResultTypeDictionary.SortResultsByResultTypePriority(results);
                foreach (var result in results)
                {
                    dynamic data = JsonConvert.DeserializeObject(result.JSONData);
                    ResultSwitch(result.ResultType, data, request.PlayerId, ref playData);
                }
            }

            return playData;
        }

        #endregion


        #region Private Functionality

        private void ResultSwitch(int resultTypeId, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            _resultHandlers[ResultTypeDictionary.GetResultTypeNameFromId(resultTypeId)]
            (
                _repository,
                data,
                playerId,
                ref playData
            );
        }

        private static void Result_RoomXYZMovement(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int xDir = data.xDir;
            int yDir = data.yDir;
            int zDir = data.zDir;
            var result = repository.PlayerMoveXYZ(playerId, xDir, yDir, zDir);
            playData.Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item1);
            playData.RoomStates = result.Item2.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList();
            playData.ParagraphStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList();
        }

        private static void Result_RoomXYZTeleport(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int xPos = data.xPos;
            int yPos = data.yPos;
            int zPos = data.zPos;
            var result = repository.PlayerTeleportRoomXYZ(playerId, xPos, yPos, zPos);
            playData.Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item1);
            playData.RoomStates = result.Item2.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList();
            playData.ParagraphStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList();
        }

        private static void Result_RoomIdTeleport(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int roomId = data.roomId;
            var result = repository.PlayerTeleportRoomID(playerId, roomId);
            playData.Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item1);
            playData.RoomStates = result.Item2.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList();
            playData.ParagraphStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList();
        }

        private static void Result_AreaIdRoomXYZTeleport(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int areaId = data.areaId;
            int xPos = data.xPos;
            int yPos = data.yPos;
            int zPos = data.zPos;
            var result = repository.PlayerTeleportAreaIdRoomXYZ(playerId, areaId, xPos, yPos, zPos);
            playData.Area = TypeAdapter.Adapt<DC.Responses.AreaData>(result.Item1);
            playData.Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item2);
            playData.RoomStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList();
            playData.ParagraphStates = result.Item4.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList();
        }

        private static void Result_AreaIdRoomIdTeleport(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int areaId = data.areaId;
            int roomId = data.roomId;
            var result = repository.PlayerTeleportAreaIdRoomId(playerId, areaId, roomId);
            playData.Area = TypeAdapter.Adapt<DC.Responses.AreaData>(result.Item1);
            playData.Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item2);
            playData.RoomStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList();
            playData.ParagraphStates = result.Item4.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList();
        }

        private static void Result_ItemAcquisition(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int itemId = data.itemId;
            repository.PlayerItemAdd(playerId, itemId);
        }

        private static void Result_EventAcquisition(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int eventId = data.eventId;
            repository.PlayerEventAdd(playerId, eventId);
        }

        private static void Result_CharacterAcquisition(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int characterId = data.characterId;
            repository.PlayerCharacterAdd(playerId, characterId);
        }

        private static void Result_ParagraphStateChange(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int paragraphId = data.paragraphId;
            int state = data.state;
            var result = repository.PlayerParagraphStateChange(playerId, paragraphId, state);
            playData.Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item1);
            playData.RoomStates = result.Item2.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList();
            playData.ParagraphStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList();
        }

        private static void Result_RoomStateChange(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int roomId = data.roomId;
            int state = data.state;
            var result = repository.PlayerParagraphStateChange(playerId, roomId, state);
            playData.Room = TypeAdapter.Adapt<DC.Responses.RoomData>(result.Item1);
            playData.RoomStates = result.Item2.Select(x => TypeAdapter.Adapt<DC.Responses.RoomStateData>(x)).ToList();
            playData.ParagraphStates = result.Item3.Select(x => TypeAdapter.Adapt<DC.Responses.ParagraphStateData>(x)).ToList();
        }

        private static void Result_MessageActivation(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            int messageId = data.messageId;
            var result = repository.ReadMessage(messageId);
            playData.Message = TypeAdapter.Adapt<DC.Responses.MessageData>(result);
        }

        private static void Result_NotImplemented(IGinTubRepository repository, dynamic data, Guid playerId, ref DC.Responses.PlayData playData)
        {
            // shrug!
        }

        #endregion

        #endregion
    }
}
