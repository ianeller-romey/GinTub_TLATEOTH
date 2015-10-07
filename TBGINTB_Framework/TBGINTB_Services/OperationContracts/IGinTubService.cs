using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using GinTub.Services;


namespace GinTub.Services.OperationContracts
{
    [ServiceContract]
    public interface IGinTubService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "PlayerLogin")]
        DataContracts.Responses.PlayerIdentifier PlayerLogin(DataContracts.Requests.PlayerLoginRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "LoadGame")]
        DataContracts.Responses.PlayData LoadGame(DataContracts.Requests.LoadGameRequest request);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetAllVerbTypes")]
        DataContracts.Responses.VerbUseData GetAllVerbTypes();

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetAllAudio/{extension}")]
        DataContracts.Responses.AudioUseData GetAllAudio(string extension);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetNounsForParagraphState/{paragraphStateId}")]
        DataContracts.Responses.PlayData GetNounsForParagraphState(string paragraphStateId);

        [OperationContract]
        [WebInvoke(Method = "GET",
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetMapForPlayer/{areaId}/{playerId}")]
        DataContracts.Responses.MapData GetMapForPlayer(string areaId, string playerId);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetInventoryForPlayer/{playerId}")]
        DataContracts.Responses.InventoriesData GetInventoryForPlayer(string playerId);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetHistoryForPlayer/{playerId}")]
        DataContracts.Responses.InventoriesData GetHistoryForPlayer(string playerId);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetPartyForPlayer/{playerId}")]
        DataContracts.Responses.InventoriesData GetPartyForPlayer(string playerId);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "DoAction")]
        DataContracts.Responses.PlayData DoAction(DataContracts.Requests.DoActionRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "DoMessageChoice")]
        DataContracts.Responses.PlayData DoMessageChoice(DataContracts.Requests.DoMessageChoiceRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "DoCheat")]
        DataContracts.Responses.PlayData DoCheat(DataContracts.Requests.CheatRequest request);
    }
}
