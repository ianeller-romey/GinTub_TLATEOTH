using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts.Requests
{

    [DataContract]
    public class LoadGameRequest
    {
        [DataMember(Name = "playerId")]
        public Guid PlayerId { get; set; }
    }

}