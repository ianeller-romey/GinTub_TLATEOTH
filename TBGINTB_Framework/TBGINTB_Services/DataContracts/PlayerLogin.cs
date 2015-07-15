using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts
{

    [DataContract]
    public class PlayerLogin
    {
        [DataMember] 
        public Guid? PlayerId { get; set; }
    }

}