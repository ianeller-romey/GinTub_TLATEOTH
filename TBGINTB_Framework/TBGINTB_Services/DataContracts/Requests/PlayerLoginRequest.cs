using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts.Requests
{

    [DataContract]
    public class PlayerLoginRequest
    {
        [DataMember(Name = "emailAddress")]
        public string EmailAddress { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }
    }

}