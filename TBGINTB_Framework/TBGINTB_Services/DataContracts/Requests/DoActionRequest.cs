using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts.Requests
{

    [DataContract]
    public class DoActionRequest
    {
        [DataMember(Name = "playerId")]
        public Guid PlayerId { get; set; }
        
        [DataMember(Name = "nounId")]
        public int? NounId { get; set; }
        
        [DataMember(Name = "verbTypeId")]
        public int VerbTypeId { get; set; }

        [DataMember(Name = "time")]
        public TimeSpan Time { get; set; }
    }

}