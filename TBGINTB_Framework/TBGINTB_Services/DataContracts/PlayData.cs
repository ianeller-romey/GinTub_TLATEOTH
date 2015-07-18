using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts
{

    [DataContract]
    public class MessageChoiceData
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember]
        public string Text { get; set; }
    }

    [DataContract]
    public class MessageData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public IEnumerable<MessageChoiceData> MessageChoices { get; set; }
    }

    [DataContract]
    public class WordData
    {
        [DataMember] 
        public int? NounId { get; set; }
        [DataMember] 
        public string Text { get; set; }
    }

    [DataContract]
    public class ParagraphStateData
    {
        [DataMember] 
        public int Id { get; set; }
        [DataMember] 
        public int Order { get; set; }
        [DataMember]
        public int? RoomState { get; set; }
        [DataMember] 
        public IEnumerable<WordData> Words { get; set; }
    }

    [DataContract]
    public class RoomStateData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public string Location { get; set; }
    }

    [DataContract]
    public class RoomData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int X { get; set; }
        [DataMember]
        public int Y { get; set; }
        [DataMember]
        public int Z { get; set; }
    }

    [DataContract]
    public class AreaData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class PlayData
    {
        [DataMember]
        public AreaData Area { get; set; }
        [DataMember]
        public RoomData Room { get; set; }
        [DataMember]
        public IEnumerable<RoomStateData> RoomStates { get; set; }
        [DataMember]
        public IEnumerable<ParagraphStateData> ParagraphStates { get; set; }
        [DataMember]
        public MessageData Message { get; set; }
    }
}