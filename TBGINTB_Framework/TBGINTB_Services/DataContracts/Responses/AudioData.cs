using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts.Responses
{

    [DataContract]
    public class AudioData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "audioFile")]
        public string AudioFile { get; set; }

        [DataMember(Name = "isLooped")]
        public bool IsLooped { get; set; }
    }

    [DataContract]
    public class AudioUseData
    {
        [DataMember(Name = "audio")]
        public IEnumerable<AudioData> Audio { get; set; }
    }

}