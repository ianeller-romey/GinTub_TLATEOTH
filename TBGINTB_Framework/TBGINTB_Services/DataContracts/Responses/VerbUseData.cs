using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts.Responses
{

    [DataContract]
    public class VerbTypeData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class VerbUseData
    {
        [DataMember(Name = "verbTypes")]
        public IEnumerable<VerbTypeData> VerbTypes { get; set; }

        [DataMember(Name = "withVerbTypes")]
        public IEnumerable<VerbTypeData> WithVerbTypes { get; set; }
    }

}