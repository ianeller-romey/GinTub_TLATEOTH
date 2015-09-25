using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts.Requests
{

    [DataContract]
    public class CheatRequest
    {
        [DataMember(Name = "playerId")]
        public Guid PlayerId { get; set; }
        
        [DataMember(Name = "cheat")]
        public string Cheat { get; set; }
        
        [DataMember(Name = "jsonString")]
        public string JSONString { get; set; }
    }

}