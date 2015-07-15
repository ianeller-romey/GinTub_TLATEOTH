using System;
using System.Collections.Generic;
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
using GinTub.Services.DataContracts;


namespace GinTub.Services
{
    public class GinTubService : OperationContracts.IGinTubService
    {
        #region MEMBER FIELDS

        Repository.Interface.IGinTubRepository _repository;

        delegate void ResultHandler(dynamic obj, ref PlayData playData);

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public GinTubService()
        {
            _repository = new Repository.GinTubRepository();
        }

        public PlayerLogin PlayerLogin(string emailAddress, string password)
        {
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
            return new PlayerLogin() { PlayerId = playerId };
        }

        public VerbUseData LoadAllVerbTypes()
        {
            var result = _repository.LoadAllVerbTypes();
            return new VerbUseData()
                {
                    VerbTypes = result.Select(v => TypeAdapter.Adapt<VerbTypeData>(v)).ToList()
                };
        }

        public PlayData LoadGame(Guid playerId)
        {
            var result = _repository.LoadGame(playerId);
            return new PlayData()
                {
                    Area = TypeAdapter.Adapt<AreaData>(result.Item1),
                    Room = TypeAdapter.Adapt<RoomData>(result.Item2),
                    RoomStates = result.Item3.Select(x => TypeAdapter.Adapt<RoomStateData>(x)).ToList(),
                    ParagraphStates = result.Item4.Select(x => TypeAdapter.Adapt<ParagraphStateData>(x)).ToList()
                };
        }

        public PlayData GetNounsForParagraphState(int paragraphStateId)
        {
            var result = _repository.GetNounsForParagraphState(paragraphStateId);
            return new PlayData()
            {
                Message = new MessageData()
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
                }
            };
        }

        public PlayData DoAction(Guid playerId, int? nounId, int verbTypeId)
        {
            if (nounId.HasValue)
            {
                var results = _repository.GetActionResults(playerId, nounId.Value, verbTypeId);
                if(results.Any())
                {
                    PlayData playData = new PlayData();
                    foreach (var result in results)
                    {
                        dynamic data = JsonConvert.DeserializeObject(result.JSONData);
                        ResultSwitch(result.ResultType, data, ref playData);
                    }
                    return playData;
                }
            }

            return new PlayData()
            {
                Message = new MessageData()
                {
                    Id = -1,
                    Text = "Nothing happens."
                }
            }; 
        }

        #endregion


        #region Private Functionality

        public void ResultSwitch(int resultTypeId, dynamic data, ref PlayData playData)
        {
            switch(resultTypeId)
            {
                case 10:
                    Result_MessageActivation(data, ref playData);
                    break;
            }
        }

        public void Result_MessageActivation(dynamic data, ref PlayData playData)
        {
            int messageId = data.messageId;
            var result = _repository.LoadMessage(messageId);
            playData.Message = TypeAdapter.Adapt<MessageData>(result);
        }

        #endregion

        #endregion
    }
}
