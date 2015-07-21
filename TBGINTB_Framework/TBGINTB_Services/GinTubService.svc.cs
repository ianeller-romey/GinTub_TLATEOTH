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
using DC = GinTub.Services.DataContracts;


namespace GinTub.Services
{
    public class GinTubService : OperationContracts.IGinTubService
    {
        #region MEMBER FIELDS

        Repository.Interface.IGinTubRepository _repository;

        delegate void ResultHandler(dynamic obj, ref DC.Responses.PlayData playData);

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
                var result = _repository.GetNounsForParagraphState(paragraphStateIntId);
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
                   playData.Message = null;
                   foreach (var result in results)
                   {
                       dynamic data = JsonConvert.DeserializeObject(result.JSONData);
                       ResultSwitch(result.ResultType, data, ref playData);
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
                foreach (var result in results)
                {
                    dynamic data = JsonConvert.DeserializeObject(result.JSONData);
                    ResultSwitch(result.ResultType, data, ref playData);
                }
            }

            return playData;
        }

        #endregion


        #region Private Functionality

        public void ResultSwitch(int resultTypeId, dynamic data, ref DC.Responses.PlayData playData)
        {
            switch(resultTypeId)
            {
                case 10:
                    Result_MessageActivation(data, ref playData);
                    break;
            }
        }

        public void Result_MessageActivation(dynamic data, ref DC.Responses.PlayData playData)
        {
            int messageId = data.messageId;
            var result = _repository.ReadMessage(messageId);
            playData.Message = TypeAdapter.Adapt<DC.Responses.MessageData>(result);
        }

        #endregion

        #endregion
    }
}
