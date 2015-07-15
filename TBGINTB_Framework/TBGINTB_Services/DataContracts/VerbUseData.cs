using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts
{

    [DataContract]
    public class VerbTypeData
    {
        [DataMember] 
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class VerbUseData
    {
        [DataMember]
        public IEnumerable<VerbTypeData> VerbTypes { get; set; }
    }

}