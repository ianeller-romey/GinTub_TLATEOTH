﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GinTub.Repository.Entities
{
    public class MessageChoice
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Message { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public MessageChoice[] MessageChoices { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public string JSONData { get; set; }
        public int ResultType { get; set; }
    }

    public class VerbType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Noun
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ParagraphState { get; set; }
    }

    public class ParagraphState
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public int? RoomState { get; set; }
        public Noun[] Nouns { get; set; }
    }

    public class RoomState
    {
        public int Id { get; set; }
        public int State { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
        public int Room { get; set; }
    }

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Area { get; set; }
    }

    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
