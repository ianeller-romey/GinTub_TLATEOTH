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
        BodyStyle = WebMessageBodyStyle.Wrapped,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "PlayerLogin")]
        DataContracts.PlayerLogin PlayerLogin(string emailAddress, string password);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "LoadGame")]
        DataContracts.PlayData LoadGame(Guid playerId);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetAllVerbTypes")]
        DataContracts.VerbUseData GetAllVerbTypes();

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetNounsForParagraphState/{paragraphStateId}")]
        DataContracts.PlayData GetNounsForParagraphState(string paragraphStateId);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "DoAction")]
        DataContracts.PlayData DoAction(Guid playerId, int? nounId, int verbTypeId);
    }
}
