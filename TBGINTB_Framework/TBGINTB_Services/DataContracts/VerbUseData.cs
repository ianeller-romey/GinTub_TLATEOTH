using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts
{

    [DataContract(Name = "verbTypeData")]
    public class VerbTypeData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract(Name = "verbUseData")]
    public class VerbUseData
    {
        [DataMember(Name = "verbTypes")]
        public IEnumerable<VerbTypeData> VerbTypes { get; set; }
    }

}