﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace GinTub.Services.DataContracts.Responses
{

    [DataContract]
    public class MessageChoiceData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    [DataContract]
    public class MessageData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "messageChoices")]
        public IEnumerable<MessageChoiceData> MessageChoices { get; set; }
    }

    [DataContract]
    public class WordData
    {
        [DataMember(Name = "nounId")]
        public int? NounId { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    [DataContract]
    public class ParagraphStateData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "roomState")]
        public int RoomState { get; set; }

        [DataMember(Name = "words")]
        public IEnumerable<WordData> Words { get; set; }
    }

    [DataContract]
    public class RoomStateData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "state")]
        public int State { get; set; }

        [DataMember(Name = "time")]
        public TimeSpan Time { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }
    }

    [DataContract]
    public class RoomData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "x")]
        public int X { get; set; }

        [DataMember(Name = "y")]
        public int Y { get; set; }

        [DataMember(Name = "z")]
        public int Z { get; set; }
    }

    [DataContract]
    public class AreaData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "displayTime")]
        public bool DisplayTime { get; set; }

        [DataMember(Name = "audio")]
        public int? Audio { get; set; }
    }

    [DataContract]
    public class GameStateData
    {
        [DataMember(Name = "lastTime")]
        public TimeSpan? LastTime { get; set; }

        [DataMember(Name = "stopTime")]
        public bool? StopTime { get; set; }
    }

    [DataContract]
    public class PlayData
    {
        [DataMember(Name = "area")]
        public AreaData Area { get; set; }

        [DataMember(Name = "room")]
        public RoomData Room { get; set; }

        [DataMember(Name = "roomStates")]
        public IEnumerable<RoomStateData> RoomStates { get; set; }

        [DataMember(Name = "paragraphStates")]
        public IEnumerable<ParagraphStateData> ParagraphStates { get; set; }

        [DataMember(Name = "message")]
        public MessageData Message { get; set; }

        [DataMember(Name = "deathMessage")]
        public MessageData DeathMessage { get; set; }

        [DataMember(Name = "gameState")]
        public GameStateData GameState { get; set; }
    }

    [DataContract]
    public class MapEntryData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "x")]
        public int X { get; set; }

        [DataMember(Name = "y")]
        public int Y { get; set; }

        [DataMember(Name = "z")]
        public int Z { get; set; }

        [DataMember(Name = "visited")]
        public bool Visited { get; set; }
    }

    [DataContract]
    public class MapData
    {
        [DataMember(Name = "area")]
        public AreaData Area { get; set; }

        [DataMember(Name = "mapEntries")]
        public IEnumerable<MapEntryData> MapEntries { get; set; }
    }

    [DataContract]
    public class InventoriesEntryData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "acquired")]
        public bool Acquired { get; set; }
    }

    [DataContract]
    public class InventoriesData
    {
        [DataMember(Name = "inventoriesEntries")]
        public IEnumerable<InventoriesEntryData> InventoriesEntries { get; set; }
    }
}