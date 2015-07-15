using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using TBGINTB_Builder.Lib.Exceptions;
using TBGINTB_Builder.Lib.Model.DbModel;
using TBGINTB_Builder.Lib.Repository;

using Db = TBGINTB_Builder.Lib.Model.DbModel;


namespace TBGINTB_Builder.Lib
{
    public static partial class GinTubBuilderManager
    {
        #region MEMBER EVENTS

        #region Areas

        public class AreaEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public AreaEventArgs(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }


        public class AreaAddedEventArgs : AreaEventArgs
        {
            public AreaAddedEventArgs(int id, string name) :  base(id, name) {}
        }
        public delegate void AreaAddedEventHandler(object sender, AreaAddedEventArgs args);
        public static event AreaAddedEventHandler AreaAdded;
        private static void OnAreaAdded(Area area)
        {
            if (AreaAdded != null)
                AreaAdded(typeof(GinTubBuilderManager), new AreaAddedEventArgs(area.Id, area.Name));
        }


        public class AreaModifiedEventArgs : AreaEventArgs
        {
            public AreaModifiedEventArgs(int id, string name) : base(id, name) {}
        }
        public delegate void AreaModifiedEventHandler(object sender, AreaModifiedEventArgs args);
        public static event AreaModifiedEventHandler AreaModified;
        private static void OnAreaModified(Area area)
        {
            if (AreaModified != null)
                AreaModified(typeof(GinTubBuilderManager), new AreaModifiedEventArgs(area.Id, area.Name));
        }


        public class AreaGetEventArgs : AreaEventArgs
        {
            public int MaxX { get; set; }
            public int MinX { get; set; }
            public int MaxY { get; set; }
            public int MinY { get; set; }
            public int MaxZ { get; set; }
            public int MinZ { get; set; }
            public int NumRooms { get; set; }
            public AreaGetEventArgs(int id, string name, int maxX, int minX, int maxY, int minY, int maxZ, int minZ, int numRooms) :
                base(id, name)
            {
                MaxX = maxX;
                MinX = minX;
                MaxY = maxY;
                MinY = minY;
                MaxZ = maxZ;
                MinZ = minZ;
                NumRooms = numRooms;
            }
        }
        public delegate void AreaGetEventHandler(object sender, AreaGetEventArgs args);
        public static event AreaGetEventHandler AreaGet;
        private static void OnAreaGet(Area area)
        {
            if (AreaGet != null)
                AreaGet(typeof(GinTubBuilderManager), new AreaGetEventArgs(area.Id, area.Name, area.MaxX, area.MinX, area.MaxY, area.MinY, area.MaxZ, area.MinZ, area.NumRooms));
        }

        #endregion


        #region Locations

        public class LocationEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LocationFile { get; set; }
            public LocationEventArgs(int id, string name, string locationFile)
            {
                Id = id;
                Name = name;
                LocationFile = locationFile;
            }
        }


        public class LocationAddedEventArgs : LocationEventArgs
        {
            public LocationAddedEventArgs(int id, string name, string locationFile) : base(id, name, locationFile) { }
        }
        public delegate void LocationAddedEventHandler(object sender, LocationAddedEventArgs args);
        public static event LocationAddedEventHandler LocationAdded;
        private static void OnLocationAdded(Location location)
        {
            if (LocationAdded != null)
                LocationAdded(typeof(GinTubBuilderManager), new LocationAddedEventArgs(location.Id, location.Name, location.LocationFile));
        }


        public class LocationModifiedEventArgs : LocationEventArgs
        {
            public LocationModifiedEventArgs(int id, string name, string locationFile) : base(id, name, locationFile) { }
        }
        public delegate void LocationModifiedEventHandler(object sender, LocationModifiedEventArgs args);
        public static event LocationModifiedEventHandler LocationModified;
        private static void OnLocationModified(Location location)
        {
            if (LocationModified != null)
                LocationModified(typeof(GinTubBuilderManager), new LocationModifiedEventArgs(location.Id, location.Name, location.LocationFile));
        }


        public class LocationGetEventArgs : LocationEventArgs
        {
            public LocationGetEventArgs(int id, string name, string locationFile) : base(id, name, locationFile) { }
        }
        public delegate void LocationGetEventHandler(object sender, LocationGetEventArgs args);
        public static event LocationGetEventHandler LocationGet;
        private static void OnLocationGet(Location location)
        {
            if (LocationGet != null)
                LocationGet(typeof(GinTubBuilderManager), new LocationGetEventArgs(location.Id, location.Name, location.LocationFile));
        }

        #endregion


        #region Rooms

        public class RoomEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int Area { get; set; }
            public RoomEventArgs(int id, string name, int x, int y, int z, int area)
            {
                Id = id;
                Name = name;
                X = x;
                Y = y;
                Z = z;
                Area = area;
            }
        }


        public class RoomAddedEventArgs : RoomEventArgs
        {
            public RoomAddedEventArgs(int id, string name, int x, int y, int z, int area) : base(id, name, x, y, z, area) {}
        }
        public delegate void RoomAddedEventHandler(object sender, RoomAddedEventArgs args);
        public static event RoomAddedEventHandler RoomAdded;
        private static void OnRoomAdded(Room room)
        {
            if (RoomAdded != null)
                RoomAdded(typeof(GinTubBuilderManager), new RoomAddedEventArgs(room.Id, room.Name, room.X, room.Y, room.Z, room.Area));
        }


        public class RoomModifiedEventArgs : RoomEventArgs
        {
            public RoomModifiedEventArgs(int id, string name, int x, int y, int z, int area) : base(id, name, x, y, z, area) {}
        }
        public delegate void RoomModifiedEventHandler(object sender, RoomModifiedEventArgs args);
        public static event RoomModifiedEventHandler RoomModified;
        private static void OnRoomModified(Room room)
        {
            if (RoomModified != null)
                RoomModified(typeof(GinTubBuilderManager), new RoomModifiedEventArgs(room.Id, room.Name, room.X, room.Y, room.Z, room.Area));
        }


        public class RoomGetEventArgs : RoomEventArgs
        {
            public RoomGetEventArgs(int id, string name, int x, int y, int z, int area) : base(id, name, x, y, z, area) {}
        }
        public delegate void RoomGetEventHandler(object sender, RoomGetEventArgs args);
        public static event RoomGetEventHandler RoomGet;
        private static void OnRoomGet(Room room)
        {
            if (RoomGet != null)
                RoomGet(typeof(GinTubBuilderManager), new RoomGetEventArgs(room.Id, room.Name, room.X, room.Y, room.Z, room.Area));
        }

        #endregion


        #region RoomStates

        public class RoomStateEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int State { get; set; }
            public DateTime? Time { get; set; }
            public int Location { get; set; }
            public int Room { get; set; }
            public RoomStateEventArgs(int id, int state, DateTime time, int location, int room)
            {
                Id = id;
                State = state;
                Time = time;
                Location = location;
                Room = room;
            }
        }


        public class RoomStateAddedEventArgs : RoomStateEventArgs
        {
            public RoomStateAddedEventArgs(int id, int state, DateTime time, int location, int room) : 
                base(id, state, time, location, room) { }
        }
        public delegate void RoomStateAddedEventHandler(object sender, RoomStateAddedEventArgs args);
        public static event RoomStateAddedEventHandler RoomStateAdded;
        private static void OnRoomStateAdded(RoomState roomState)
        {
            if (RoomStateAdded != null)
                RoomStateAdded(typeof(GinTubBuilderManager),
                    new RoomStateAddedEventArgs(roomState.Id, roomState.State, roomState.Time, roomState.Location, roomState.Room));
        }


        public class RoomStateModifiedEventArgs : RoomStateEventArgs
        {
            public RoomStateModifiedEventArgs(int id, int state, DateTime time, int location, int room) :
                base(id, state, time, location, room) { }
        }
        public delegate void RoomStateModifiedEventHandler(object sender, RoomStateModifiedEventArgs args);
        public static event RoomStateModifiedEventHandler RoomStateModified;
        private static void OnRoomStateModified(RoomState roomState)
        {
            if (RoomStateModified != null)
                RoomStateModified(typeof(GinTubBuilderManager),
                    new RoomStateModifiedEventArgs(roomState.Id, roomState.State, roomState.Time, roomState.Location, roomState.Room));
        }


        public class RoomStateGetEventArgs : RoomStateEventArgs
        {
            public RoomStateGetEventArgs(int id, int state, DateTime time, int location, int room) :
                base(id, state, time, location, room) { }
        }
        public delegate void RoomStateGetEventHandler(object sender, RoomStateGetEventArgs args);
        public static event RoomStateGetEventHandler RoomStateGet;
        private static void OnRoomStateGet(RoomState roomState)
        {
            if (RoomStateGet != null)
                RoomStateGet(typeof(GinTubBuilderManager),
                    new RoomStateGetEventArgs(roomState.Id, roomState.State, roomState.Time, roomState.Location, roomState.Room));
        }

        #endregion


        #region Paragraphs

        public class ParagraphEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int Order { get; set; }
            public int Room { get; set; }
            public int? RoomState { get; set; }
            public ParagraphEventArgs(int id, int order, int room, int? roomState)
            {
                Id = id;
                Order = order;
                Room = room;
                RoomState = roomState;
            }
        }


        public class ParagraphAddedEventArgs : ParagraphEventArgs
        {
            public ParagraphAddedEventArgs(int id, int order, int room, int? roomState) :
                base(id, order, room, roomState) { }
        }
        public delegate void ParagraphAddedEventHandler(object sender, ParagraphAddedEventArgs args);
        public static event ParagraphAddedEventHandler ParagraphAdded;
        private static void OnParagraphAdded(Paragraph paragraph)
        {
            if (ParagraphAdded != null)
                ParagraphAdded(typeof(GinTubBuilderManager),
                    new ParagraphAddedEventArgs(paragraph.Id, paragraph.Order, paragraph.Room, paragraph.RoomState));
        }


        public class ParagraphModifiedEventArgs : ParagraphEventArgs
        {
            public ParagraphModifiedEventArgs(int id, int order, int room, int? roomState) :
                base(id, order, room, roomState) { }
        }
        public delegate void ParagraphModifiedEventHandler(object sender, ParagraphModifiedEventArgs args);
        public static event ParagraphModifiedEventHandler ParagraphModified;
        private static void OnParagraphModified(Paragraph paragraph)
        {
            if (ParagraphModified != null)
                ParagraphModified(typeof(GinTubBuilderManager),
                    new ParagraphModifiedEventArgs(paragraph.Id, paragraph.Order, paragraph.Room, paragraph.RoomState));
        }


        public class ParagraphGetEventArgs : ParagraphEventArgs
        {
            public ParagraphGetEventArgs(int id, int order, int room, int? roomState) :
                base(id, order, room, roomState) { }
        }
        public delegate void ParagraphGetEventHandler(object sender, ParagraphGetEventArgs args);
        public static event ParagraphGetEventHandler ParagraphGet;
        private static void OnParagraphGet(Paragraph paragraph)
        {
            if (ParagraphGet != null)
                ParagraphGet(typeof(GinTubBuilderManager),
                    new ParagraphGetEventArgs(paragraph.Id, paragraph.Order, paragraph.Room, paragraph.RoomState));
        }

        #endregion


        #region ParagraphStates

        public class ParagraphStateEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public int State { get; set; }
            public int Paragraph { get; set; }
            public ParagraphStateEventArgs(int id, string text, int state, int paragraphState)
            {
                Id = id;
                Text = text;
                State = state;
                Paragraph = paragraphState;
            }
        }


        public class ParagraphStateAddedEventArgs : ParagraphStateEventArgs
        {
            public ParagraphStateAddedEventArgs(int id, string text, int state, int paragraphState) :
                base(id, text, state, paragraphState) { }
        }
        public delegate void ParagraphStateAddedEventHandler(object sender, ParagraphStateAddedEventArgs args);
        public static event ParagraphStateAddedEventHandler ParagraphStateAdded;
        private static void OnParagraphStateAdded(ParagraphState paragraphState)
        {
            if (ParagraphStateAdded != null)
                ParagraphStateAdded(typeof(GinTubBuilderManager),
                    new ParagraphStateAddedEventArgs(paragraphState.Id, paragraphState.Text, paragraphState.State, paragraphState.Paragraph));
        }


        public class ParagraphStateModifiedEventArgs : ParagraphStateEventArgs
        {
            public ParagraphStateModifiedEventArgs(int id, string text, int state, int paragraphState) :
                base(id, text, state, paragraphState) { }
        }
        public delegate void ParagraphStateModifiedEventHandler(object sender, ParagraphStateModifiedEventArgs args);
        public static event ParagraphStateModifiedEventHandler ParagraphStateModified;
        private static void OnParagraphStateModified(ParagraphState paragraphState)
        {
            if (ParagraphStateModified != null)
                ParagraphStateModified(typeof(GinTubBuilderManager),
                    new ParagraphStateModifiedEventArgs(paragraphState.Id, paragraphState.Text, paragraphState.State, paragraphState.Paragraph));
        }


        public class ParagraphStateGetEventArgs : ParagraphStateEventArgs
        {
            public ParagraphStateGetEventArgs(int id, string text, int state, int paragraphState) :
                base(id, text, state, paragraphState) { }
        }
        public delegate void ParagraphStateGetEventHandler(object sender, ParagraphStateGetEventArgs args);
        public static event ParagraphStateGetEventHandler ParagraphStateGet;
        private static void OnParagraphStateGet(ParagraphState paragraphState)
        {
            if (ParagraphStateGet != null)
                ParagraphStateGet(typeof(GinTubBuilderManager),
                    new ParagraphStateGetEventArgs(paragraphState.Id, paragraphState.Text, paragraphState.State, paragraphState.Paragraph));
        }

        #endregion


        #region Nouns

        public class NounEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public int ParagraphState { get; set; }
            public NounEventArgs(int id, string text, int paragraphState)
            {
                Id = id;
                Text = text;
                ParagraphState = paragraphState;
            }
        }


        public class NounAddedEventArgs : NounEventArgs
        {
            public NounAddedEventArgs(int id, string text, int paragraphState) :
                base(id, text, paragraphState) { }
        }
        public delegate void NounAddedEventHandler(object sender, NounAddedEventArgs args);
        public static event NounAddedEventHandler NounAdded;
        private static void OnNounAdded(Noun noun)
        {
            if (NounAdded != null)
                NounAdded(typeof(GinTubBuilderManager),
                    new NounAddedEventArgs(noun.Id, noun.Text, noun.ParagraphState));
        }


        public class NounModifiedEventArgs : NounEventArgs
        {
            public NounModifiedEventArgs(int id, string text, int paragraphState) :
                base(id, text, paragraphState) { }
        }
        public delegate void NounModifiedEventHandler(object sender, NounModifiedEventArgs args);
        public static event NounModifiedEventHandler NounModified;
        private static void OnNounModified(Noun noun)
        {
            if (NounModified != null)
                NounModified(typeof(GinTubBuilderManager),
                    new NounModifiedEventArgs(noun.Id, noun.Text, noun.ParagraphState));
        }


        public class NounGetEventArgs : NounEventArgs
        {
            public NounGetEventArgs(int id, string text, int paragraphState) :
                base(id, text, paragraphState) { }
        }
        public delegate void NounGetEventHandler(object sender, NounGetEventArgs args);
        public static event NounGetEventHandler NounGet;
        private static void OnNounGet(Noun noun)
        {
            if (NounGet != null)
                NounGet(typeof(GinTubBuilderManager),
                    new NounGetEventArgs(noun.Id, noun.Text, noun.ParagraphState));
        }

        #endregion


        #region VerbTypes

        public class VerbTypeEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public VerbTypeEventArgs(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }


        public class VerbTypeAddedEventArgs : VerbTypeEventArgs
        {
            public VerbTypeAddedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void VerbTypeAddedEventHandler(object sender, VerbTypeAddedEventArgs args);
        public static event VerbTypeAddedEventHandler VerbTypeAdded;
        private static void OnVerbTypeAdded(VerbType verbType)
        {
            if (VerbTypeAdded != null)
                VerbTypeAdded(typeof(GinTubBuilderManager), new VerbTypeAddedEventArgs(verbType.Id, verbType.Name));
        }


        public class VerbTypeModifiedEventArgs : VerbTypeEventArgs
        {
            public VerbTypeModifiedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void VerbTypeModifiedEventHandler(object sender, VerbTypeModifiedEventArgs args);
        public static event VerbTypeModifiedEventHandler VerbTypeModified;
        private static void OnVerbTypeModified(VerbType verbType)
        {
            if (VerbTypeModified != null)
                VerbTypeModified(typeof(GinTubBuilderManager), new VerbTypeModifiedEventArgs(verbType.Id, verbType.Name));
        }


        public class VerbTypeGetEventArgs : VerbTypeEventArgs
        {
            public VerbTypeGetEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void VerbTypeGetEventHandler(object sender, VerbTypeGetEventArgs args);
        public static event VerbTypeGetEventHandler VerbTypeGet;
        private static void OnVerbTypeGet(VerbType verbType)
        {
            if (VerbTypeGet != null)
                VerbTypeGet(typeof(GinTubBuilderManager), new VerbTypeGetEventArgs(verbType.Id, verbType.Name));
        }

        #endregion


        #region Verbs

        public class VerbEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int VerbType { get; set; }
            public VerbEventArgs(int id, string name, int verbType)
            {
                Id = id;
                Name = name;
                VerbType = verbType;
            }
        }


        public class VerbAddedEventArgs : VerbEventArgs
        {
            public VerbAddedEventArgs(int id, string name, int verbType) : base(id, name, verbType) { }
        }
        public delegate void VerbAddedEventHandler(object sender, VerbAddedEventArgs args);
        public static event VerbAddedEventHandler VerbAdded;
        private static void OnVerbAdded(Verb verb)
        {
            if (VerbAdded != null)
                VerbAdded(typeof(GinTubBuilderManager), new VerbAddedEventArgs(verb.Id, verb.Name, verb.VerbType));
        }


        public class VerbModifiedEventArgs : VerbEventArgs
        {
            public VerbModifiedEventArgs(int id, string name, int verbType) : base(id, name, verbType) { }
        }
        public delegate void VerbModifiedEventHandler(object sender, VerbModifiedEventArgs args);
        public static event VerbModifiedEventHandler VerbModified;
        private static void OnVerbModified(Verb verb)
        {
            if (VerbModified != null)
                VerbModified(typeof(GinTubBuilderManager), new VerbModifiedEventArgs(verb.Id, verb.Name, verb.VerbType));
        }


        public class VerbGetEventArgs : VerbEventArgs
        {
            public VerbGetEventArgs(int id, string name, int verbType) : base(id, name, verbType) { }
        }
        public delegate void VerbGetEventHandler(object sender, VerbGetEventArgs args);
        public static event VerbGetEventHandler VerbGet;
        private static void OnVerbGet(Verb verb)
        {
            if (VerbGet != null)
                VerbGet(typeof(GinTubBuilderManager), new VerbGetEventArgs(verb.Id, verb.Name, verb.VerbType));
        }

        #endregion


        #region Actions

        public class ActionEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int VerbType { get; set; }
            public int Noun { get; set; }
            public string Name { get; set; }
            public ActionEventArgs(int id, int verbType, int noun, string name)
            {
                Id = id;
                VerbType = verbType;
                Noun = noun;
                Name = name;
            }
        }


        public class ActionAddedEventArgs : ActionEventArgs
        {
            public ActionAddedEventArgs(int id, int verbType, int noun, string name) : base(id, verbType, noun, name) { }
        }
        public delegate void ActionAddedEventHandler(object sender, ActionAddedEventArgs args);
        public static event ActionAddedEventHandler ActionAdded;
        private static void OnActionAdded(Db.Action action)
        {
            if (ActionAdded != null)
                ActionAdded(typeof(GinTubBuilderManager), new ActionAddedEventArgs(action.Id, action.VerbType, action.Noun, action.Name));
        }


        public class ActionModifiedEventArgs : ActionEventArgs
        {
            public ActionModifiedEventArgs(int id, int verbType, int noun, string name) : base(id, verbType, noun, name) { }
        }
        public delegate void ActionModifiedEventHandler(object sender, ActionModifiedEventArgs args);
        public static event ActionModifiedEventHandler ActionModified;
        private static void OnActionModified(Db.Action action)
        {
            if (ActionModified != null)
                ActionModified(typeof(GinTubBuilderManager), new ActionModifiedEventArgs(action.Id, action.VerbType, action.Noun, action.Name));
        }


        public class ActionGetEventArgs : ActionEventArgs
        {
            public ActionGetEventArgs(int id, int verbType, int noun, string name) : base(id, verbType, noun, name) { }
        }
        public delegate void ActionGetEventHandler(object sender, ActionGetEventArgs args);
        public static event ActionGetEventHandler ActionGet;
        private static void OnActionGet(Db.Action action)
        {
            if (ActionGet != null)
                ActionGet(typeof(GinTubBuilderManager), new ActionGetEventArgs(action.Id, action.VerbType, action.Noun, action.Name));
        }

        #endregion


        #region ResultTypes

        public class ResultTypeEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ResultTypeEventArgs(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }


        public class ResultTypeAddedEventArgs : ResultTypeEventArgs
        {
            public ResultTypeAddedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void ResultTypeAddedEventHandler(object sender, ResultTypeAddedEventArgs args);
        public static event ResultTypeAddedEventHandler ResultTypeAdded;
        private static void OnResultTypeAdded(ResultType resultType)
        {
            if (ResultTypeAdded != null)
                ResultTypeAdded(typeof(GinTubBuilderManager), new ResultTypeAddedEventArgs(resultType.Id, resultType.Name));
        }


        public class ResultTypeModifiedEventArgs : ResultTypeEventArgs
        {
            public ResultTypeModifiedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void ResultTypeModifiedEventHandler(object sender, ResultTypeModifiedEventArgs args);
        public static event ResultTypeModifiedEventHandler ResultTypeModified;
        private static void OnResultTypeModified(ResultType resultType)
        {
            if (ResultTypeModified != null)
                ResultTypeModified(typeof(GinTubBuilderManager), new ResultTypeModifiedEventArgs(resultType.Id, resultType.Name));
        }


        public class ResultTypeGetEventArgs : ResultTypeEventArgs
        {
            public ResultTypeGetEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void ResultTypeGetEventHandler(object sender, ResultTypeGetEventArgs args);
        public static event ResultTypeGetEventHandler ResultTypeGet;
        private static void OnResultTypeGet(ResultType resultType)
        {
            if (ResultTypeGet != null)
                ResultTypeGet(typeof(GinTubBuilderManager), new ResultTypeGetEventArgs(resultType.Id, resultType.Name));
        }

        #endregion


        #region JSONPropertyDataTypes

        public class JSONPropertyDataTypeEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string DataType { get; set; }
            public JSONPropertyDataTypeEventArgs(int id, string dataType)
            {
                Id = id;
                DataType = dataType;
            }
        }


        public class JSONPropertyDataTypeAddedEventArgs : JSONPropertyDataTypeEventArgs
        {
            public JSONPropertyDataTypeAddedEventArgs(int id, string dataType) : base(id, dataType) { }
        }
        public delegate void JSONPropertyDataTypeAddedEventHandler(object sender, JSONPropertyDataTypeAddedEventArgs args);
        public static event JSONPropertyDataTypeAddedEventHandler JSONPropertyDataTypeAdded;
        private static void OnJSONPropertyDataTypeAdded(JSONPropertyDataType jsonPropertyDataType)
        {
            if (JSONPropertyDataTypeAdded != null)
                JSONPropertyDataTypeAdded(typeof(GinTubBuilderManager), new JSONPropertyDataTypeAddedEventArgs(jsonPropertyDataType.Id, jsonPropertyDataType.DataType));
        }


        public class JSONPropertyDataTypeModifiedEventArgs : JSONPropertyDataTypeEventArgs
        {
            public JSONPropertyDataTypeModifiedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void JSONPropertyDataTypeModifiedEventHandler(object sender, JSONPropertyDataTypeModifiedEventArgs args);
        public static event JSONPropertyDataTypeModifiedEventHandler JSONPropertyDataTypeModified;
        private static void OnJSONPropertyDataTypeModified(JSONPropertyDataType jsonPropertyDataType)
        {
            if (JSONPropertyDataTypeModified != null)
                JSONPropertyDataTypeModified(typeof(GinTubBuilderManager), new JSONPropertyDataTypeModifiedEventArgs(jsonPropertyDataType.Id, jsonPropertyDataType.DataType));
        }


        public class JSONPropertyDataTypeGetEventArgs : JSONPropertyDataTypeEventArgs
        {
            public JSONPropertyDataTypeGetEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void JSONPropertyDataTypeGetEventHandler(object sender, JSONPropertyDataTypeGetEventArgs args);
        public static event JSONPropertyDataTypeGetEventHandler JSONPropertyDataTypeGet;
        private static void OnJSONPropertyDataTypeGet(JSONPropertyDataType jsonPropertyDataType)
        {
            if (JSONPropertyDataTypeGet != null)
                JSONPropertyDataTypeGet(typeof(GinTubBuilderManager), new JSONPropertyDataTypeGetEventArgs(jsonPropertyDataType.Id, jsonPropertyDataType.DataType));
        }

        #endregion


        #region ResultTypeJSONProperties

        public class ResultTypeJSONPropertyEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string JSONProperty { get; set; }
            public int DataType { get; set; }
            public int ResultType { get; set; }
            public ResultTypeJSONPropertyEventArgs(int id, string jsonProperty, int dataType, int resultType)
            {
                Id = id;
                JSONProperty = jsonProperty;
                DataType = dataType;
                ResultType = resultType;
            }
        }


        public class ResultTypeJSONPropertyAddedEventArgs : ResultTypeJSONPropertyEventArgs
        {
            public ResultTypeJSONPropertyAddedEventArgs(int id, string jsonProperty, int dataType, int resultType) :
                base(id, jsonProperty, dataType, resultType) { }
        }
        public delegate void ResultTypeJSONPropertyAddedEventHandler(object sender, ResultTypeJSONPropertyAddedEventArgs args);
        public static event ResultTypeJSONPropertyAddedEventHandler ResultTypeJSONPropertyAdded;
        private static void OnResultTypeJSONPropertyAdded(ResultTypeJSONProperty resultTypeJSONProperty)
        {
            if (ResultTypeJSONPropertyAdded != null)
                ResultTypeJSONPropertyAdded
                (
                    typeof(GinTubBuilderManager), 
                    new ResultTypeJSONPropertyAddedEventArgs
                    (
                        resultTypeJSONProperty.Id, 
                        resultTypeJSONProperty.JSONProperty, 
                        resultTypeJSONProperty.DataType,
                        resultTypeJSONProperty.ResultType
                    )
                );
        }


        public class ResultTypeJSONPropertyModifiedEventArgs : ResultTypeJSONPropertyEventArgs
        {
            public ResultTypeJSONPropertyModifiedEventArgs(int id, string name, int dataType, int resultType) : base(id, name, dataType, resultType) { }
        }
        public delegate void ResultTypeJSONPropertyModifiedEventHandler(object sender, ResultTypeJSONPropertyModifiedEventArgs args);
        public static event ResultTypeJSONPropertyModifiedEventHandler ResultTypeJSONPropertyModified;
        private static void OnResultTypeJSONPropertyModified(ResultTypeJSONProperty resultTypeJSONProperty)
        {
            if (ResultTypeJSONPropertyModified != null)
                ResultTypeJSONPropertyModified
                (
                    typeof(GinTubBuilderManager),
                    new ResultTypeJSONPropertyModifiedEventArgs
                    (
                        resultTypeJSONProperty.Id,
                        resultTypeJSONProperty.JSONProperty,
                        resultTypeJSONProperty.DataType,
                        resultTypeJSONProperty.ResultType
                    )
                );
        }


        public class ResultTypeJSONPropertyGetEventArgs : ResultTypeJSONPropertyEventArgs
        {
            public ResultTypeJSONPropertyGetEventArgs(int id, string name, int dataType, int resultType) : base(id, name, dataType, resultType) { }
        }
        public delegate void ResultTypeJSONPropertyGetEventHandler(object sender, ResultTypeJSONPropertyGetEventArgs args);
        public static event ResultTypeJSONPropertyGetEventHandler ResultTypeJSONPropertyGet;
        private static void OnResultTypeJSONPropertyGet(ResultTypeJSONProperty resultTypeJSONProperty)
        {
            if (ResultTypeJSONPropertyGet != null)
                ResultTypeJSONPropertyGet
                (
                    typeof(GinTubBuilderManager),
                    new ResultTypeJSONPropertyGetEventArgs
                    (
                        resultTypeJSONProperty.Id,
                        resultTypeJSONProperty.JSONProperty,
                        resultTypeJSONProperty.DataType,
                        resultTypeJSONProperty.ResultType
                    )
                );
        }

        #endregion


        #region Results

        public class ResultEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string JSONData { get; set; }
            public int ResultType { get; set; }
            public ResultEventArgs(int id, string name, string jsonData, int resultType)
            {
                Id = id;
                Name = name;
                JSONData = jsonData;
                ResultType = resultType;
            }
        }


        public class ResultAddedEventArgs : ResultEventArgs
        {
            public ResultAddedEventArgs(int id, string name, string jsonData, int resultType) : base(id, name, jsonData, resultType) { }
        }
        public delegate void ResultAddedEventHandler(object sender, ResultAddedEventArgs args);
        public static event ResultAddedEventHandler ResultAdded;
        private static void OnResultAdded(Result result)
        {
            if (ResultAdded != null)
                ResultAdded(typeof(GinTubBuilderManager), new ResultAddedEventArgs(result.Id, result.Name, result.JSONData, result.ResultType));
        }


        public class ResultModifiedEventArgs : ResultEventArgs
        {
            public ResultModifiedEventArgs(int id, string name, string jsonData, int resultType) : base(id, name, jsonData, resultType) { }
        }
        public delegate void ResultModifiedEventHandler(object sender, ResultModifiedEventArgs args);
        public static event ResultModifiedEventHandler ResultModified;
        private static void OnResultModified(Result result)
        {
            if (ResultModified != null)
                ResultModified(typeof(GinTubBuilderManager), new ResultModifiedEventArgs(result.Id, result.Name, result.JSONData, result.ResultType));
        }


        public class ResultGetEventArgs : ResultEventArgs
        {
            public ResultGetEventArgs(int id, string name, string jsonData, int resultType) : base(id, name, jsonData, resultType) { }
        }
        public delegate void ResultGetEventHandler(object sender, ResultGetEventArgs args);
        public static event ResultGetEventHandler ResultGet;
        private static void OnResultGet(Result result)
        {
            if (ResultGet != null)
                ResultGet(typeof(GinTubBuilderManager), new ResultGetEventArgs(result.Id, result.Name, result.JSONData, result.ResultType));
        }

        #endregion


        #region ActionResults

        public class ActionResultEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int Result { get; set; }
            public int Action { get; set; }
            public ActionResultEventArgs(int id, int result, int action)
            {
                Id = id;
                Result = result;
                Action = action;
            }
        }


        public class ActionResultAddedEventArgs : ActionResultEventArgs
        {
            public ActionResultAddedEventArgs(int id, int result, int action) : base(id, result, action) { }
        }
        public delegate void ActionResultAddedEventHandler(object sender, ActionResultAddedEventArgs args);
        public static event ActionResultAddedEventHandler ActionResultAdded;
        private static void OnActionResultAdded(ActionResult actionResult)
        {
            if (ActionResultAdded != null)
                ActionResultAdded(typeof(GinTubBuilderManager),
                    new ActionResultAddedEventArgs(actionResult.Id, actionResult.Result, actionResult.Action));
        }


        public class ActionResultModifiedEventArgs : ActionResultEventArgs
        {
            public ActionResultModifiedEventArgs(int id, int result, int action) : base(id, result, action) { }
        }
        public delegate void ActionResultModifiedEventHandler(object sender, ActionResultModifiedEventArgs args);
        public static event ActionResultModifiedEventHandler ActionResultModified;
        private static void OnActionResultModified(ActionResult actionResult)
        {
            if (ActionResultModified != null)
                ActionResultModified(typeof(GinTubBuilderManager),
                    new ActionResultModifiedEventArgs(actionResult.Id, actionResult.Result, actionResult.Action));
        }


        public class ActionResultGetEventArgs : ActionResultEventArgs
        {
            public ActionResultGetEventArgs(int id, int result, int action) : base(id, result, action) { }
        }
        public delegate void ActionResultGetEventHandler(object sender, ActionResultGetEventArgs args);
        public static event ActionResultGetEventHandler ActionResultGet;
        private static void OnActionResultGet(ActionResult actionResult)
        {
            if (ActionResultGet != null)
                ActionResultGet(typeof(GinTubBuilderManager),
                    new ActionResultGetEventArgs(actionResult.Id, actionResult.Result, actionResult.Action));
        }

        #endregion


        #region Items

        public class ItemEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public ItemEventArgs(int id, string name, string description)
            {
                Id = id;
                Name = name;
                Description = description;
            }
        }


        public class ItemAddedEventArgs : ItemEventArgs
        {
            public ItemAddedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void ItemAddedEventHandler(object sender, ItemAddedEventArgs args);
        public static event ItemAddedEventHandler ItemAdded;
        private static void OnItemAdded(Item item)
        {
            if (ItemAdded != null)
                ItemAdded(typeof(GinTubBuilderManager),
                    new ItemAddedEventArgs(item.Id, item.Name, item.Description));
        }


        public class ItemModifiedEventArgs : ItemEventArgs
        {
            public ItemModifiedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void ItemModifiedEventHandler(object sender, ItemModifiedEventArgs args);
        public static event ItemModifiedEventHandler ItemModified;
        private static void OnItemModified(Item item)
        {
            if (ItemModified != null)
                ItemModified(typeof(GinTubBuilderManager),
                    new ItemModifiedEventArgs(item.Id, item.Name, item.Description));
        }


        public class ItemGetEventArgs : ItemEventArgs
        {
            public ItemGetEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void ItemGetEventHandler(object sender, ItemGetEventArgs args);
        public static event ItemGetEventHandler ItemGet;
        private static void OnItemGet(Item item)
        {
            if (ItemGet != null)
                ItemGet(typeof(GinTubBuilderManager),
                    new ItemGetEventArgs(item.Id, item.Name, item.Description));
        }

        #endregion


        #region Events

        public class EventEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public EventEventArgs(int id, string name, string description)
            {
                Id = id;
                Name = name;
                Description = description;
            }
        }


        public class EventAddedEventArgs : EventEventArgs
        {
            public EventAddedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void EventAddedEventHandler(object sender, EventAddedEventArgs args);
        public static event EventAddedEventHandler EventAdded;
        private static void OnEventAdded(Event evnt)
        {
            if (EventAdded != null)
                EventAdded(typeof(GinTubBuilderManager),
                    new EventAddedEventArgs(evnt.Id, evnt.Name, evnt.Description));
        }


        public class EventModifiedEventArgs : EventEventArgs
        {
            public EventModifiedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void EventModifiedEventHandler(object sender, EventModifiedEventArgs args);
        public static event EventModifiedEventHandler EventModified;
        private static void OnEventModified(Event evnt)
        {
            if (EventModified != null)
                EventModified(typeof(GinTubBuilderManager),
                    new EventModifiedEventArgs(evnt.Id, evnt.Name, evnt.Description));
        }


        public class EventGetEventArgs : EventEventArgs
        {
            public EventGetEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void EventGetEventHandler(object sender, EventGetEventArgs args);
        public static event EventGetEventHandler EventGet;
        private static void OnEventGet(Event evnt)
        {
            if (EventGet != null)
                EventGet(typeof(GinTubBuilderManager),
                    new EventGetEventArgs(evnt.Id, evnt.Name, evnt.Description));
        }

        #endregion


        #region Characters

        public class CharacterEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public CharacterEventArgs(int id, string name, string description)
            {
                Id = id;
                Name = name;
                Description = description;
            }
        }


        public class CharacterAddedEventArgs : CharacterEventArgs
        {
            public CharacterAddedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void CharacterAddedCharacterHandler(object sender, CharacterAddedEventArgs args);
        public static event CharacterAddedCharacterHandler CharacterAdded;
        private static void OnCharacterAdded(Character character)
        {
            if (CharacterAdded != null)
                CharacterAdded(typeof(GinTubBuilderManager),
                    new CharacterAddedEventArgs(character.Id, character.Name, character.Description));
        }


        public class CharacterModifiedEventArgs : CharacterEventArgs
        {
            public CharacterModifiedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void CharacterModifiedCharacterHandler(object sender, CharacterModifiedEventArgs args);
        public static event CharacterModifiedCharacterHandler CharacterModified;
        private static void OnCharacterModified(Character character)
        {
            if (CharacterModified != null)
                CharacterModified(typeof(GinTubBuilderManager),
                    new CharacterModifiedEventArgs(character.Id, character.Name, character.Description));
        }


        public class CharacterGetEventArgs : CharacterEventArgs
        {
            public CharacterGetEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void CharacterGetCharacterHandler(object sender, CharacterGetEventArgs args);
        public static event CharacterGetCharacterHandler CharacterGet;
        private static void OnCharacterGet(Character character)
        {
            if (CharacterGet != null)
                CharacterGet(typeof(GinTubBuilderManager),
                    new CharacterGetEventArgs(character.Id, character.Name, character.Description));
        }

        #endregion


        #region ItemActionRequirements

        public class ItemActionRequirementEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int Item { get; set; }
            public int Action { get; set; }
            public ItemActionRequirementEventArgs(int id, int item, int action)
            {
                Id = id;
                Item = item;
                Action = action;
            }
        }


        public class ItemActionRequirementAddedEventArgs : ItemActionRequirementEventArgs
        {
            public ItemActionRequirementAddedEventArgs(int id, int item, int action) :
                base(id, item, action) { }
        }
        public delegate void ItemActionRequirementAddedEventHandler(object sender, ItemActionRequirementAddedEventArgs args);
        public static event ItemActionRequirementAddedEventHandler ItemActionRequirementAdded;
        private static void OnItemActionRequirementAdded(ItemActionRequirement itemActionRequirement)
        {
            if (ItemActionRequirementAdded != null)
                ItemActionRequirementAdded(typeof(GinTubBuilderManager),
                    new ItemActionRequirementAddedEventArgs(itemActionRequirement.Id, itemActionRequirement.Item, itemActionRequirement.Action));
        }


        public class ItemActionRequirementModifiedEventArgs : ItemActionRequirementEventArgs
        {
            public ItemActionRequirementModifiedEventArgs(int id, int item, int action) :
                base(id, item, action) { }
        }
        public delegate void ItemActionRequirementModifiedEventHandler(object sender, ItemActionRequirementModifiedEventArgs args);
        public static event ItemActionRequirementModifiedEventHandler ItemActionRequirementModified;
        private static void OnItemActionRequirementModified(ItemActionRequirement itemActionRequirement)
        {
            if (ItemActionRequirementModified != null)
                ItemActionRequirementModified(typeof(GinTubBuilderManager),
                    new ItemActionRequirementModifiedEventArgs(itemActionRequirement.Id, itemActionRequirement.Item, itemActionRequirement.Action));
        }


        public class ItemActionRequirementGetEventArgs : ItemActionRequirementEventArgs
        {
            public ItemActionRequirementGetEventArgs(int id, int item, int action) :
                base(id, item, action) { }
        }
        public delegate void ItemActionRequirementGetEventHandler(object sender, ItemActionRequirementGetEventArgs args);
        public static event ItemActionRequirementGetEventHandler ItemActionRequirementGet;
        private static void OnItemActionRequirementGet(ItemActionRequirement itemActionRequirement)
        {
            if (ItemActionRequirementGet != null)
                ItemActionRequirementGet(typeof(GinTubBuilderManager),
                    new ItemActionRequirementGetEventArgs(itemActionRequirement.Id, itemActionRequirement.Item, itemActionRequirement.Action));
        }

        #endregion


        #region EventActionRequirements

        public class EventActionRequirementEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int Event { get; set; }
            public int Action { get; set; }
            public EventActionRequirementEventArgs(int id, int evnt, int action)
            {
                Id = id;
                Event = evnt;
                Action = action;
            }
        }


        public class EventActionRequirementAddedEventArgs : EventActionRequirementEventArgs
        {
            public EventActionRequirementAddedEventArgs(int id, int evnt, int action) :
                base(id, evnt, action) { }
        }
        public delegate void EventActionRequirementAddedEventHandler(object sender, EventActionRequirementAddedEventArgs args);
        public static event EventActionRequirementAddedEventHandler EventActionRequirementAdded;
        private static void OnEventActionRequirementAdded(EventActionRequirement evntActionRequirement)
        {
            if (EventActionRequirementAdded != null)
                EventActionRequirementAdded(typeof(GinTubBuilderManager),
                    new EventActionRequirementAddedEventArgs(evntActionRequirement.Id, evntActionRequirement.Event, evntActionRequirement.Action));
        }


        public class EventActionRequirementModifiedEventArgs : EventActionRequirementEventArgs
        {
            public EventActionRequirementModifiedEventArgs(int id, int evnt, int action) :
                base(id, evnt, action) { }
        }
        public delegate void EventActionRequirementModifiedEventHandler(object sender, EventActionRequirementModifiedEventArgs args);
        public static event EventActionRequirementModifiedEventHandler EventActionRequirementModified;
        private static void OnEventActionRequirementModified(EventActionRequirement evntActionRequirement)
        {
            if (EventActionRequirementModified != null)
                EventActionRequirementModified(typeof(GinTubBuilderManager),
                    new EventActionRequirementModifiedEventArgs(evntActionRequirement.Id, evntActionRequirement.Event, evntActionRequirement.Action));
        }


        public class EventActionRequirementGetEventArgs : EventActionRequirementEventArgs
        {
            public EventActionRequirementGetEventArgs(int id, int evnt, int action) :
                base(id, evnt, action) { }
        }
        public delegate void EventActionRequirementGetEventHandler(object sender, EventActionRequirementGetEventArgs args);
        public static event EventActionRequirementGetEventHandler EventActionRequirementGet;
        private static void OnEventActionRequirementGet(EventActionRequirement evntActionRequirement)
        {
            if (EventActionRequirementGet != null)
                EventActionRequirementGet(typeof(GinTubBuilderManager),
                    new EventActionRequirementGetEventArgs(evntActionRequirement.Id, evntActionRequirement.Event, evntActionRequirement.Action));
        }

        #endregion


        #region CharacterActionRequirements

        public class CharacterActionRequirementEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int Character { get; set; }
            public int Action { get; set; }
            public CharacterActionRequirementEventArgs(int id, int character, int action)
            {
                Id = id;
                Character = character;
                Action = action;
            }
        }


        public class CharacterActionRequirementAddedEventArgs : CharacterActionRequirementEventArgs
        {
            public CharacterActionRequirementAddedEventArgs(int id, int character, int action) :
                base(id, character, action) { }
        }
        public delegate void CharacterActionRequirementAddedEventHandler(object sender, CharacterActionRequirementAddedEventArgs args);
        public static event CharacterActionRequirementAddedEventHandler CharacterActionRequirementAdded;
        private static void OnCharacterActionRequirementAdded(CharacterActionRequirement characterActionRequirement)
        {
            if (CharacterActionRequirementAdded != null)
                CharacterActionRequirementAdded(typeof(GinTubBuilderManager),
                    new CharacterActionRequirementAddedEventArgs(characterActionRequirement.Id, characterActionRequirement.Character, characterActionRequirement.Action));
        }


        public class CharacterActionRequirementModifiedEventArgs : CharacterActionRequirementEventArgs
        {
            public CharacterActionRequirementModifiedEventArgs(int id, int character, int action) :
                base(id, character, action) { }
        }
        public delegate void CharacterActionRequirementModifiedEventHandler(object sender, CharacterActionRequirementModifiedEventArgs args);
        public static event CharacterActionRequirementModifiedEventHandler CharacterActionRequirementModified;
        private static void OnCharacterActionRequirementModified(CharacterActionRequirement characterActionRequirement)
        {
            if (CharacterActionRequirementModified != null)
                CharacterActionRequirementModified(typeof(GinTubBuilderManager),
                    new CharacterActionRequirementModifiedEventArgs(characterActionRequirement.Id, characterActionRequirement.Character, characterActionRequirement.Action));
        }


        public class CharacterActionRequirementGetEventArgs : CharacterActionRequirementEventArgs
        {
            public CharacterActionRequirementGetEventArgs(int id, int character, int action) :
                base(id, character, action) { }
        }
        public delegate void CharacterActionRequirementGetEventHandler(object sender, CharacterActionRequirementGetEventArgs args);
        public static event CharacterActionRequirementGetEventHandler CharacterActionRequirementGet;
        private static void OnCharacterActionRequirementGet(CharacterActionRequirement characterActionRequirement)
        {
            if (CharacterActionRequirementGet != null)
                CharacterActionRequirementGet(typeof(GinTubBuilderManager),
                    new CharacterActionRequirementGetEventArgs(characterActionRequirement.Id, characterActionRequirement.Character, characterActionRequirement.Action));
        }

        #endregion


        #region Messages

        public class MessageEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
            public MessageEventArgs(int id, string name, string text)
            {
                Id = id;
                Name = name;
                Text = text;
            }
        }


        public class MessageAddedEventArgs : MessageEventArgs
        {
            public MessageAddedEventArgs(int id, string name, string text) :
                base(id, name, text) { }
        }
        public delegate void MessageAddedMessageHandler(object sender, MessageAddedEventArgs args);
        public static event MessageAddedMessageHandler MessageAdded;
        private static void OnMessageAdded(Message message)
        {
            if (MessageAdded != null)
                MessageAdded(typeof(GinTubBuilderManager),
                    new MessageAddedEventArgs(message.Id, message.Name, message.Text));
        }


        public class MessageModifiedEventArgs : MessageEventArgs
        {
            public MessageModifiedEventArgs(int id, string name, string text) :
                base(id, name, text) { }
        }
        public delegate void MessageModifiedMessageHandler(object sender, MessageModifiedEventArgs args);
        public static event MessageModifiedMessageHandler MessageModified;
        private static void OnMessageModified(Message message)
        {
            if (MessageModified != null)
                MessageModified(typeof(GinTubBuilderManager),
                    new MessageModifiedEventArgs(message.Id, message.Name, message.Text));
        }


        public class MessageGetEventArgs : MessageEventArgs
        {
            public MessageGetEventArgs(int id, string name, string text) :
                base(id, name, text) { }
        }
        public delegate void MessageGetMessageHandler(object sender, MessageGetEventArgs args);
        public static event MessageGetMessageHandler MessageGet;
        private static void OnMessageGet(Message message)
        {
            if (MessageGet != null)
                MessageGet(typeof(GinTubBuilderManager),
                    new MessageGetEventArgs(message.Id, message.Name, message.Text));
        }

        #endregion


        #region MessageChoices

        public class MessageChoiceEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
            public int Message { get; set; }
            public MessageChoiceEventArgs(int id, string name, string text, int message)
            {
                Id = id;
                Name = name;
                Text = text;
                Message = message;
            }
        }


        public class MessageChoiceAddedEventArgs : MessageChoiceEventArgs
        {
            public MessageChoiceAddedEventArgs(int id, string name, string text, int message) :
                base(id, name, text, message) { }
        }
        public delegate void MessageChoiceAddedMessageChoiceHandler(object sender, MessageChoiceAddedEventArgs args);
        public static event MessageChoiceAddedMessageChoiceHandler MessageChoiceAdded;
        private static void OnMessageChoiceAdded(MessageChoice messageChoice)
        {
            if (MessageChoiceAdded != null)
                MessageChoiceAdded(typeof(GinTubBuilderManager),
                    new MessageChoiceAddedEventArgs(messageChoice.Id, messageChoice.Name, messageChoice.Text, messageChoice.Message));
        }


        public class MessageChoiceModifiedEventArgs : MessageChoiceEventArgs
        {
            public MessageChoiceModifiedEventArgs(int id, string name, string text, int message) :
                base(id, name, text, message) { }
        }
        public delegate void MessageChoiceModifiedMessageChoiceHandler(object sender, MessageChoiceModifiedEventArgs args);
        public static event MessageChoiceModifiedMessageChoiceHandler MessageChoiceModified;
        private static void OnMessageChoiceModified(MessageChoice messageChoice)
        {
            if (MessageChoiceModified != null)
                MessageChoiceModified(typeof(GinTubBuilderManager),
                    new MessageChoiceModifiedEventArgs(messageChoice.Id, messageChoice.Name, messageChoice.Text, messageChoice.Message));
        }


        public class MessageChoiceGetEventArgs : MessageChoiceEventArgs
        {
            public MessageChoiceGetEventArgs(int id, string name, string text, int message) :
                base(id, name, text, message) { }
        }
        public delegate void MessageChoiceGetMessageChoiceHandler(object sender, MessageChoiceGetEventArgs args);
        public static event MessageChoiceGetMessageChoiceHandler MessageChoiceGet;
        private static void OnMessageChoiceGet(MessageChoice messageChoice)
        {
            if (MessageChoiceGet != null)
                MessageChoiceGet(typeof(GinTubBuilderManager),
                    new MessageChoiceGetEventArgs(messageChoice.Id, messageChoice.Name, messageChoice.Text, messageChoice.Message));
        }

        #endregion


        #region MessageChoiceResults

        public class MessageChoiceResultEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int Result { get; set; }
            public int MessageChoice { get; set; }
            public MessageChoiceResultEventArgs(int id, int result, int messageChoice)
            {
                Id = id;
                Result = result;
                MessageChoice = messageChoice;
            }
        }


        public class MessageChoiceResultAddedEventArgs : MessageChoiceResultEventArgs
        {
            public MessageChoiceResultAddedEventArgs(int id, int result, int messageChoice) : base(id, result, messageChoice) { }
        }
        public delegate void MessageChoiceResultAddedEventHandler(object sender, MessageChoiceResultAddedEventArgs args);
        public static event MessageChoiceResultAddedEventHandler MessageChoiceResultAdded;
        private static void OnMessageChoiceResultAdded(MessageChoiceResult messageChoiceResult)
        {
            if (MessageChoiceResultAdded != null)
                MessageChoiceResultAdded(typeof(GinTubBuilderManager),
                    new MessageChoiceResultAddedEventArgs(messageChoiceResult.Id, messageChoiceResult.Result, messageChoiceResult.MessageChoice));
        }


        public class MessageChoiceResultModifiedEventArgs : MessageChoiceResultEventArgs
        {
            public MessageChoiceResultModifiedEventArgs(int id, int result, int messageChoice) : base(id, result, messageChoice) { }
        }
        public delegate void MessageChoiceResultModifiedEventHandler(object sender, MessageChoiceResultModifiedEventArgs args);
        public static event MessageChoiceResultModifiedEventHandler MessageChoiceResultModified;
        private static void OnMessageChoiceResultModified(MessageChoiceResult messageChoiceResult)
        {
            if (MessageChoiceResultModified != null)
                MessageChoiceResultModified(typeof(GinTubBuilderManager),
                    new MessageChoiceResultModifiedEventArgs(messageChoiceResult.Id, messageChoiceResult.Result, messageChoiceResult.MessageChoice));
        }


        public class MessageChoiceResultGetEventArgs : MessageChoiceResultEventArgs
        {
            public MessageChoiceResultGetEventArgs(int id, int result, int messageChoice) : base(id, result, messageChoice) { }
        }
        public delegate void MessageChoiceResultGetEventHandler(object sender, MessageChoiceResultGetEventArgs args);
        public static event MessageChoiceResultGetEventHandler MessageChoiceResultGet;
        private static void OnMessageChoiceResultGet(MessageChoiceResult messageChoiceResult)
        {
            if (MessageChoiceResultGet != null)
                MessageChoiceResultGet(typeof(GinTubBuilderManager),
                    new MessageChoiceResultGetEventArgs(messageChoiceResult.Id, messageChoiceResult.Result, messageChoiceResult.MessageChoice));
        }

        #endregion


        #region RoomPreviewParagraphStates

        public class RoomPreviewParagraphStateEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public int State { get; set; }
            public int Paragraph { get; set; }
            public int Room { get; set; }
            public RoomPreviewNounEventArgs[] Nouns { get; set; }
            public RoomPreviewParagraphStateEventArgs(int id, string text, int state, int paragraphState, int room, RoomPreviewNounEventArgs[] nouns)
            {
                Id = id;
                Text = text;
                State = state;
                Paragraph = paragraphState;
                Room = room;
                Nouns = nouns;
            }
        }


        public class RoomPreviewParagraphStateGetEventArgs : RoomPreviewParagraphStateEventArgs
        {
            public RoomPreviewParagraphStateGetEventArgs(int id, string text, int state, int paragraphState, int room, RoomPreviewNounEventArgs[] nouns) :
                base(id, text, state, paragraphState, room, nouns) { }
        }
        public delegate void RoomPreviewParagraphStateGetEventHandler(object sender, RoomPreviewParagraphStateGetEventArgs args);
        public static event RoomPreviewParagraphStateGetEventHandler RoomPreviewParagraphStateGet;
        private static void OnRoomPreviewParagraphStateGet(RoomPreviewParagraphState roomPreviewParagraphState)
        {
            if (RoomPreviewParagraphStateGet != null)
                RoomPreviewParagraphStateGet(typeof(GinTubBuilderManager),
                    new RoomPreviewParagraphStateGetEventArgs
                    (
                        roomPreviewParagraphState.Id, 
                        roomPreviewParagraphState.Text, 
                        roomPreviewParagraphState.State, 
                        roomPreviewParagraphState.Paragraph,
                        roomPreviewParagraphState.Room,
                        roomPreviewParagraphState.Nouns.Select(n => new RoomPreviewNounEventArgs(n.Id, n.Text, n.ParagraphState, n.Room)).ToArray()
                    ));
        }

        #endregion


        #region RoomPreviewNouns

        public class RoomPreviewNounEventArgs : EventArgs
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public int ParagraphState { get; set; }
            public int Room { get; set; }
            public RoomPreviewNounEventArgs(int id, string text, int paragraphState, int room)
            {
                Id = id;
                Text = text;
                ParagraphState = paragraphState;
                Room = room;
            }
        }


        public class RoomPreviewNounGetEventArgs : RoomPreviewNounEventArgs
        {
            public RoomPreviewNounGetEventArgs(int id, string text, int paragraphState, int room) :
                base(id, text, paragraphState, room) { }
        }
        public delegate void RoomPreviewNounGetEventHandler(object sender, RoomPreviewNounGetEventArgs args);
        public static event RoomPreviewNounGetEventHandler RoomPreviewNounGet;
        private static void OnRoomPreviewNounGet(RoomPreviewNoun roomPreviewNoun)
        {
            if (RoomPreviewNounGet != null)
                RoomPreviewNounGet(typeof(GinTubBuilderManager),
                    new RoomPreviewNounGetEventArgs
                    (
                        roomPreviewNoun.Id,
                        roomPreviewNoun.Text,
                        roomPreviewNoun.ParagraphState,
                        roomPreviewNoun.Room
                    ));
        }

        #endregion


        #region AreaRoomOnInitialLoads

        public class AreaRoomOnInitialLoadEventArgs : EventArgs
        {
            public int? Area { get; set; }
            public int? Room { get; set; }
            public AreaRoomOnInitialLoadEventArgs(int? area, int? room)
            {
                Area = area;
                Room = room;
            }
        }


        public class AreaRoomOnInitialLoadAddedEventArgs : AreaRoomOnInitialLoadEventArgs
        {
            public AreaRoomOnInitialLoadAddedEventArgs(int? area, int? room) : base(area, room) { }
        }
        public delegate void AreaRoomOnInitialLoadAddedEventHandler(object sender, AreaRoomOnInitialLoadAddedEventArgs args);
        public static event AreaRoomOnInitialLoadAddedEventHandler AreaRoomOnInitialLoadAdded;
        private static void OnAreaRoomOnInitialLoadAdded(AreaRoomOnInitialLoad areaRoomOnInitialLoad)
        {
            if (AreaRoomOnInitialLoadAdded != null)
                AreaRoomOnInitialLoadAdded(typeof(GinTubBuilderManager), new AreaRoomOnInitialLoadAddedEventArgs(areaRoomOnInitialLoad.Area, areaRoomOnInitialLoad.Room));
        }


        public class AreaRoomOnInitialLoadModifiedEventArgs : AreaRoomOnInitialLoadEventArgs
        {
            public AreaRoomOnInitialLoadModifiedEventArgs(int? area, int? room) : base(area, room) { }
        }
        public delegate void AreaRoomOnInitialLoadModifiedEventHandler(object sender, AreaRoomOnInitialLoadModifiedEventArgs args);
        public static event AreaRoomOnInitialLoadModifiedEventHandler AreaRoomOnInitialLoadModified;
        private static void OnAreaRoomOnInitialLoadModified(AreaRoomOnInitialLoad areaRoomOnInitialLoad)
        {
            if (AreaRoomOnInitialLoadModified != null)
                AreaRoomOnInitialLoadModified(typeof(GinTubBuilderManager), new AreaRoomOnInitialLoadModifiedEventArgs(areaRoomOnInitialLoad.Area, areaRoomOnInitialLoad.Room));
        }


        public class AreaRoomOnInitialLoadGetEventArgs : AreaRoomOnInitialLoadEventArgs
        {
            public AreaRoomOnInitialLoadGetEventArgs(int? area, int? room) : base(area, room) { }
        }
        public delegate void AreaRoomOnInitialLoadGetEventHandler(object sender, AreaRoomOnInitialLoadGetEventArgs args);
        public static event AreaRoomOnInitialLoadGetEventHandler AreaRoomOnInitialLoadGet;
        private static void OnAreaRoomOnInitialLoadGet(AreaRoomOnInitialLoad areaRoomOnInitialLoad)
        {
            if (AreaRoomOnInitialLoadGet != null)
                AreaRoomOnInitialLoadGet(typeof(GinTubBuilderManager), new AreaRoomOnInitialLoadGetEventArgs(areaRoomOnInitialLoad.Area, areaRoomOnInitialLoad.Room));
        }

        #endregion

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        #region Areas

        public static void AddArea(string areaName)
        {
            int id = InsertArea(areaName);
            Area area = SelectArea(id);
            OnAreaAdded(area);
        }

        public static void ModifyArea(int areaId, string areaName)
        {
            UpdateArea(areaId, areaName);
            Area area = SelectArea(areaId);
            OnAreaModified(area);
        }

        public static void GetArea(int areaId)
        {
            Area area = SelectArea(areaId);
            OnAreaGet(area);
        }

        public static void LoadAllAreas()
        {
            List<Area> areas = SelectAllAreas();
            foreach (var area in areas)
                OnAreaAdded(area);
        }

        #endregion


        #region Locations

        public static void AddLocation(string locationName, string locationFile)
        {
            int id = InsertLocation(locationName, locationFile);
            Location location = SelectLocation(id);
            OnLocationAdded(location);
        }

        public static void ModifyLocation(int locationId, string locationName, string locationFile)
        {
            UpdateLocation(locationId, locationName, locationFile);
            Location location = SelectLocation(locationId);
            OnLocationModified(location);
        }

        public static void GetLocation(int locationId)
        {
            Location location = SelectLocation(locationId);
            OnLocationGet(location);
        }

        public static void LoadAllLocations()
        {
            List<Location> locations = SelectAllLocations();
            foreach (var location in locations)
                OnLocationAdded(location);
        }

        #endregion


        #region Rooms

        public static void AddRoom(string roomName, int roomX, int roomY, int roomZ, int areaId)
        {
            int id = InsertRoom(roomName, roomX, roomY, roomZ, areaId);
            Room room = SelectRoom(id);
            OnRoomAdded(room);
        }

        public static void ModifyRoom(int roomId, string roomName, int roomX, int roomY, int roomZ, int areaId)
        {
            UpdateRoom(roomId, roomName, roomX, roomY, roomZ, areaId);
            Room room = SelectRoom(roomId);
            OnRoomModified(room);
        }

        public static void GetRoom(int roomId)
        {
            Room room = SelectRoom(roomId);
            OnRoomGet(room);
        }

        public static void LoadAllRoomsInArea(int areaId)
        {
            List<Room> rooms = SelectAllRoomsInArea(areaId);
            foreach (var room in rooms)
                OnRoomAdded(room);
        }

        public static void LoadAllRoomsInAreaOnFloor(int areaId, int z)
        {
            List<Room> rooms = SelectAllRoomsInAreaOnFloor(areaId, z);
            foreach (var room in rooms)
                OnRoomAdded(room);
        }

        #endregion


        #region RoomStates

        public static void AddRoomState(DateTime? roomStateTime, int locationId, int roomId)
        {
            int id = InsertRoomState(roomStateTime, locationId, roomId);
            RoomState roomState = SelectRoomState(id);
            OnRoomStateAdded(roomState);
        }

        public static void ModifyRoomState(int roomStateId, int roomStateState, DateTime? roomStateTime, int locationId, int roomId)
        {
            UpdateRoomState(roomStateId, roomStateState, roomStateTime, locationId, roomId);
            RoomState roomState = SelectRoomState(roomStateId);
            OnRoomStateModified(roomState);
        }

        public static void GetRoomState(int roomStateId)
        {
            RoomState roomState = SelectRoomState(roomStateId);
            OnRoomStateGet(roomState);
        }

        public static void LoadAllRoomStatesForRoom(int roomId)
        {
            List<RoomState> roomStates = SelectAllRoomStatesForRoom(roomId);
            foreach (var roomState in roomStates)
                OnRoomStateAdded(roomState);
        }

        #endregion


        #region Paragraphs

        public static void AddParagraph(int paragraphOrder, int roomId, int? roomStateId)
        {
            int id = InsertParagraph(paragraphOrder, roomId, roomStateId);
            Paragraph paragraph = SelectParagraph(id);
            OnParagraphAdded(paragraph);
        }

        public static void ModifyParagraph(int paragraphId, int paragraphOrder, int roomId, int? roomStateId)
        {
            UpdateParagraph(paragraphId, paragraphOrder, roomId, roomStateId);
            Paragraph paragraph = SelectParagraph(paragraphId);
            OnParagraphModified(paragraph);
        }

        public static void GetParagraph(int paragraphId)
        {
            Paragraph paragraph = SelectParagraph(paragraphId);
            OnParagraphGet(paragraph);
        }

        public static void LoadAllParagraphForRoom(int roomId)
        {
            List<Paragraph> paragraphs = SelectAllParagraphsForRoom(roomId);
            foreach (var paragraph in paragraphs)
                OnParagraphAdded(paragraph);
        }

        public static void LoadAllParagraphsForRoomAndRoomState(int roomId, int? roomStateId)
        {
            List<Paragraph> paragraphs = SelectAllParagraphsForRoomAndRoomState(roomId, roomStateId);
            foreach (var paragraph in paragraphs)
                OnParagraphAdded(paragraph);
        }

        #endregion


        #region ParagraphStates

        public static void AddParagraphState(string paragraphStateText, int paragraphId)
        {
            int id = InsertParagraphState(paragraphStateText, paragraphId);
            ParagraphState paragraphState = SelectParagraphState(id);
            OnParagraphStateAdded(paragraphState);
        }

        public static void ModifyParagraphState(int paragraphStateId, string paragraphStateText, int paragraphStateState, int paragraphId)
        {
            UpdateParagraphState(paragraphStateId, paragraphStateText, paragraphStateState, paragraphId);
            ParagraphState paragraphState = SelectParagraphState(paragraphStateId);
            OnParagraphStateModified(paragraphState);
        }

        public static void GetParagraphState(int paragraphStateId)
        {
            ParagraphState paragraphState = SelectParagraphState(paragraphStateId);
            OnParagraphStateGet(paragraphState);
        }

        public static void LoadAllParagraphStatesForParagraph(int paragraphId)
        {
            List<ParagraphState> paragraphStates = SelectAllParagraphStatesForParagraph(paragraphId);
            foreach (var paragraphState in paragraphStates)
                OnParagraphStateAdded(paragraphState);
        }

        public static void LoadParagraphStateNounPossibilities(int paragraphStateId)
        {
            ParagraphState paragraphState = SelectParagraphState(paragraphStateId);
            OnParagraphStateAdded(paragraphState);
        }

        public static void LoadParagraphStateForParagraphPreview(int paragraphStateState, int paragraphId)
        {
            ParagraphState paragraphState = SelectParagraphStateForParagraphPreview(paragraphStateState, paragraphId);
            if(paragraphState != null)
                OnParagraphStateAdded(paragraphState);
        }

        public static void GetParagraphStateForParagraphPreview(int paragraphStateState, int paragraphId)
        {
            ParagraphState paragraphState = SelectParagraphStateForParagraphPreview(paragraphStateState, paragraphId);
            if (paragraphState != null)
                OnParagraphStateGet(paragraphState);
        }

        #endregion


        #region Nouns

        public static void AddNoun(string nounText, int paragraphStateId)
        {
            int id = InsertNoun(nounText, paragraphStateId);
            Noun noun = SelectNoun(id);
            OnNounAdded(noun);
        }

        public static void ModifyNoun(int nounId, string nounText, int paragraphStateId)
        {
            UpdateNoun(nounId, nounText, paragraphStateId);
            Noun noun = SelectNoun(nounId);
            OnNounModified(noun);
        }

        public static void GetNoun(int nounId)
        {
            Noun noun = SelectNoun(nounId);
            OnNounGet(noun);
        }

        public static void LoadAllNounsForParagraphState(int paragraphStateId)
        {
            List<Noun> nouns = SelectAllNounsForParagraphState(paragraphStateId);
            foreach (var noun in nouns)
                OnNounAdded(noun);
        }

        #endregion


        #region VerbTypes

        public static void AddVerbType(string verbTypeName)
        {
            int id = InsertVerbType(verbTypeName);
            VerbType verbType = SelectVerbType(id);
            OnVerbTypeAdded(verbType);
        }

        public static void ModifyVerbType(int verbTypeId, string verbTypeName)
        {
            UpdateVerbType(verbTypeId, verbTypeName);
            VerbType verbType = SelectVerbType(verbTypeId);
            OnVerbTypeModified(verbType);
        }

        public static void GetVerbType(int verbTypeId)
        {
            VerbType verbType = SelectVerbType(verbTypeId);
            OnVerbTypeGet(verbType);
        }

        public static void LoadAllVerbTypes()
        {
            List<VerbType> verbTypes = SelectAllVerbTypes();
            foreach (var verbType in verbTypes)
                OnVerbTypeAdded(verbType);
        }

        #endregion


        #region Verbs

        public static void AddVerb(string verbName, int verbTypeId)
        {
            int id = InsertVerb(verbName, verbTypeId);
            Verb verb = SelectVerb(id);
            OnVerbAdded(verb);
        }

        public static void ModifyVerb(int verbId, string verbName, int verbTypeId)
        {
            UpdateVerb(verbId, verbName, verbTypeId);
            Verb verb = SelectVerb(verbId);
            OnVerbModified(verb);
        }

        public static void GetVerb(int verbId)
        {
            Verb verb = SelectVerb(verbId);
            OnVerbGet(verb);
        }

        public static void LoadAllVerbsForVerbType(int verbTypeId)
        {
            List<Verb> verbs = SelectAllVerbsForVerbType(verbTypeId);
            foreach (var verb in verbs)
                OnVerbAdded(verb);
        }

        #endregion


        #region Actions

        public static void AddAction(int actionVerbType, int actionNoun)
        {
            int id = InsertAction(actionVerbType, actionNoun);
            Db.Action action = SelectAction(id);
            OnActionAdded(action);
        }

        public static void ModifyAction(int actionId, int actionVerbType, int actionNoun)
        {
            UpdateAction(actionId, actionVerbType, actionNoun);
            Db.Action action = SelectAction(actionId);
            OnActionModified(action);
        }

        public static void GetAction(int actionId)
        {
            Db.Action action = SelectAction(actionId);
            OnActionGet(action);
        }

        public static void LoadAllActionsForNoun(int nounId)
        {
            List<Db.Action> actions = SelectAllActionsForNoun(nounId);
            foreach (var action in actions)
                OnActionAdded(action);
        }

        #endregion


        #region ResultTypes

        public static void AddResultType(string resultTypeName)
        {
            int id = InsertResultType(resultTypeName);
            ResultType resultType = SelectResultType(id);
            OnResultTypeAdded(resultType);
        }

        public static void ModifyResultType(int resultTypeId, string resultTypeName)
        {
            UpdateResultType(resultTypeId, resultTypeName);
            ResultType resultType = SelectResultType(resultTypeId);
            OnResultTypeModified(resultType);
        }

        public static void GetResultType(int resultTypeId)
        {
            ResultType resultType = SelectResultType(resultTypeId);
            OnResultTypeGet(resultType);
        }

        public static void LoadAllResultTypes()
        {
            List<ResultType> resultTypes = SelectAllResultTypes();
            foreach (var resultType in resultTypes)
                OnResultTypeAdded(resultType);
        }

        #endregion


        #region JSONPropertyDataTypes

        public static void AddJSONPropertyDataType(string jsonPropertyDataTypeDataType)
        {
            int id = InsertJSONPropertyDataType(jsonPropertyDataTypeDataType);
            JSONPropertyDataType jsonPropertyDataType = SelectJSONPropertyDataType(id);
            OnJSONPropertyDataTypeAdded(jsonPropertyDataType);
        }

        public static void ModifyJSONPropertyDataType(int jsonPropertyDataTypeId, string jsonPropertyDataTypeDataType)
        {
            UpdateJSONPropertyDataType(jsonPropertyDataTypeId, jsonPropertyDataTypeDataType);
            JSONPropertyDataType jsonPropertyDataType = SelectJSONPropertyDataType(jsonPropertyDataTypeId);
            OnJSONPropertyDataTypeModified(jsonPropertyDataType);
        }

        public static void GetJSONPropertyDataType(int jsonPropertyDataTypeId)
        {
            JSONPropertyDataType jsonPropertyDataType = SelectJSONPropertyDataType(jsonPropertyDataTypeId);
            OnJSONPropertyDataTypeGet(jsonPropertyDataType);
        }

        public static void LoadAllJSONPropertyDataTypes()
        {
            List<JSONPropertyDataType> jsonPropertyDataTypes = SelectAllJSONPropertyDataTypes();
            foreach (var jsonPropertyDataType in jsonPropertyDataTypes)
                OnJSONPropertyDataTypeAdded(jsonPropertyDataType);
        }

        #endregion


        #region ResultTypeJSONProperties

        public static void AddResultTypeJSONProperty(string resultTypeJSONPropertyJSONProperty, int resultTypeJSONPropertyDataType, int resultTypeJSONPropertyResultTypeId)
        {
            int id = InsertResultTypeJSONProperty(resultTypeJSONPropertyJSONProperty, resultTypeJSONPropertyDataType, resultTypeJSONPropertyResultTypeId);
            ResultTypeJSONProperty resultTypeJSONProperty = SelectResultTypeJSONProperty(id);
            OnResultTypeJSONPropertyAdded(resultTypeJSONProperty);
        }

        public static void ModifyResultTypeJSONProperty(int resultTypeJSONPropertyId, string resultTypeJSONPropertyJSONProperty, int resultTypeJSONPropertyDataType, int resultTypeJSONPropertyResultTypeId)
        {
            UpdateResultTypeJSONProperty(resultTypeJSONPropertyId, resultTypeJSONPropertyJSONProperty, resultTypeJSONPropertyDataType, resultTypeJSONPropertyResultTypeId);
            ResultTypeJSONProperty resultTypeJSONProperty = SelectResultTypeJSONProperty(resultTypeJSONPropertyId);
            OnResultTypeJSONPropertyModified(resultTypeJSONProperty);
        }

        public static void GetResultTypeJSONProperty(int resultTypeJSONPropertyId)
        {
            ResultTypeJSONProperty resultTypeJSONProperty = SelectResultTypeJSONProperty(resultTypeJSONPropertyId);
            OnResultTypeJSONPropertyGet(resultTypeJSONProperty);
        }

        public static void LoadAllResultTypeJSONPropertiesForResultType(int resultTypeId)
        {
            List<ResultTypeJSONProperty> resultTypeJSONProperties = SelectAllResultTypeJSONPropertiesForResultType(resultTypeId);
            foreach (var resultTypeJSONProperty in resultTypeJSONProperties)
                OnResultTypeJSONPropertyAdded(resultTypeJSONProperty);
        }

        #endregion


        #region Results

        public static void AddResult(string resultName, string resultJSONData, int resultTypeId)
        {
            int id = InsertResult(resultName, resultJSONData, resultTypeId);
            Result result = SelectResult(id);
            OnResultAdded(result);
        }

        public static void ModifyResult(int resultId, string resultName, string resultJSONData, int resultTypeId)
        {
            UpdateResult(resultId, resultName, resultJSONData, resultTypeId);
            Result result = SelectResult(resultId);
            OnResultModified(result);
        }

        public static void GetResult(int resultId)
        {
            Result result = SelectResult(resultId);
            OnResultGet(result);
        }

        public static void LoadAllResultsForResultType(int resultTypeId)
        {
            List<Result> results = SelectAllResultsForResultType(resultTypeId);
            foreach (var result in results)
                OnResultAdded(result);
        }

        public static void LoadAllResultsForActionResultType(int actionId)
        {
            List<Result> results = SelectAllResultsForActionResultType(actionId);
            foreach (var result in results)
                OnResultAdded(result);
        }

        public static void LoadAllResultsForMessageChoiceResultType(int messageChoiceId)
        {
            List<Result> results = SelectAllResultsForMessageChoiceResultType(messageChoiceId);
            foreach (var result in results)
                OnResultAdded(result);
        }

        #endregion


        #region ActionResults

        public static void AddActionResult(int actionResultResult, int actionResultAction)
        {
            int id = InsertActionResult(actionResultResult, actionResultAction);
            ActionResult actionResult = SelectActionResult(id);
            OnActionResultAdded(actionResult);
        }

        public static void ModifyActionResult(int actionResultId, int actionResultResult, int actionResultAction)
        {
            UpdateActionResult(actionResultId, actionResultResult, actionResultAction);
            ActionResult actionResult = SelectActionResult(actionResultId);
            OnActionResultModified(actionResult);
        }

        public static void GetActionResult(int actionResultId)
        {
            ActionResult actionResult = SelectActionResult(actionResultId);
            OnActionResultGet(actionResult);
        }

        public static void LoadAllActionResultsForAction(int actionId)
        {
            List<ActionResult> actionResults = SelectAllActionResultsForAction(actionId);
            foreach (var actionResult in actionResults)
                OnActionResultAdded(actionResult);
        }

        #endregion


        #region Items

        public static void AddItem(string itemName, string itemDescription)
        {
            int id = InsertItem(itemName, itemDescription);
            Item item = SelectItem(id);
            OnItemAdded(item);
        }

        public static void ModifyItem(int itemId, string itemName, string itemDescription)
        {
            UpdateItem(itemId, itemName, itemDescription);
            Item item = SelectItem(itemId);
            OnItemModified(item);
        }

        public static void GetItem(int itemId)
        {
            Item item = SelectItem(itemId);
            OnItemGet(item);
        }

        public static void LoadAllItems()
        {
            List<Item> items = SelectAllItems();
            foreach (var item in items)
                OnItemAdded(item);
        }

        #endregion


        #region Events

        public static void AddEvent(string evntName, string evntDescription)
        {
            int id = InsertEvent(evntName, evntDescription);
            Event evnt = SelectEvent(id);
            OnEventAdded(evnt);
        }

        public static void ModifyEvent(int evntId, string evntName, string evntDescription)
        {
            UpdateEvent(evntId, evntName, evntDescription);
            Event evnt = SelectEvent(evntId);
            OnEventModified(evnt);
        }

        public static void GetEvent(int evntId)
        {
            Event evnt = SelectEvent(evntId);
            OnEventGet(evnt);
        }

        public static void LoadAllEvents()
        {
            List<Event> evnts = SelectAllEvents();
            foreach (var evnt in evnts)
                OnEventAdded(evnt);
        }

        #endregion


        #region Characters

        public static void AddCharacter(string characterName, string characterDescription)
        {
            int id = InsertCharacter(characterName, characterDescription);
            Character character = SelectCharacter(id);
            OnCharacterAdded(character);
        }

        public static void ModifyCharacter(int characterId, string characterName, string characterDescription)
        {
            UpdateCharacter(characterId, characterName, characterDescription);
            Character character = SelectCharacter(characterId);
            OnCharacterModified(character);
        }

        public static void GetCharacter(int characterId)
        {
            Character character = SelectCharacter(characterId);
            OnCharacterGet(character);
        }

        public static void LoadAllCharacters()
        {
            List<Character> characters = SelectAllCharacters();
            foreach (var character in characters)
                OnCharacterAdded(character);
        }

        #endregion


        #region ItemActionRequirements

        public static void AddItemActionRequirement(int itemActionRequirementItem, int itemActionRequirementAction)
        {
            int id = InsertItemActionRequirement(itemActionRequirementItem, itemActionRequirementAction);
            ItemActionRequirement itemActionRequirement = SelectItemActionRequirement(id);
            OnItemActionRequirementAdded(itemActionRequirement);
        }

        public static void ModifyItemActionRequirement(int itemActionRequirementId, int itemActionRequirementItem, int itemActionRequirementAction)
        {
            UpdateItemActionRequirement(itemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction);
            ItemActionRequirement itemActionRequirement = SelectItemActionRequirement(itemActionRequirementId);
            OnItemActionRequirementModified(itemActionRequirement);
        }

        public static void GetItemActionRequirement(int itemActionRequirementId)
        {
            ItemActionRequirement itemActionRequirement = SelectItemActionRequirement(itemActionRequirementId);
            OnItemActionRequirementGet(itemActionRequirement);
        }

        public static void LoadAllItemActionRequirementsForAction(int action)
        {
            List<ItemActionRequirement> itemActionRequirements = SelectAllItemActionRequirementsForAction(action);
            foreach (var itemActionRequirement in itemActionRequirements)
                OnItemActionRequirementAdded(itemActionRequirement);
        }

        #endregion


        #region EventActionRequirements

        public static void AddEventActionRequirement(int evntActionRequirementEvent, int evntActionRequirementAction)
        {
            int id = InsertEventActionRequirement(evntActionRequirementEvent, evntActionRequirementAction);
            EventActionRequirement evntActionRequirement = SelectEventActionRequirement(id);
            OnEventActionRequirementAdded(evntActionRequirement);
        }

        public static void ModifyEventActionRequirement(int evntActionRequirementId, int evntActionRequirementEvent, int evntActionRequirementAction)
        {
            UpdateEventActionRequirement(evntActionRequirementId, evntActionRequirementEvent, evntActionRequirementAction);
            EventActionRequirement evntActionRequirement = SelectEventActionRequirement(evntActionRequirementId);
            OnEventActionRequirementModified(evntActionRequirement);
        }

        public static void GetEventActionRequirement(int evntActionRequirementId)
        {
            EventActionRequirement evntActionRequirement = SelectEventActionRequirement(evntActionRequirementId);
            OnEventActionRequirementGet(evntActionRequirement);
        }

        public static void LoadAllEventActionRequirementsForAction(int action)
        {
            List<EventActionRequirement> evntActionRequirements = SelectAllEventActionRequirementsForAction(action);
            foreach (var evntActionRequirement in evntActionRequirements)
                OnEventActionRequirementAdded(evntActionRequirement);
        }

        #endregion


        #region CharacterActionRequirements

        public static void AddCharacterActionRequirement(int characterActionRequirementCharacter, int characterActionRequirementAction)
        {
            int id = InsertCharacterActionRequirement(characterActionRequirementCharacter, characterActionRequirementAction);
            CharacterActionRequirement characterActionRequirement = SelectCharacterActionRequirement(id);
            OnCharacterActionRequirementAdded(characterActionRequirement);
        }

        public static void ModifyCharacterActionRequirement(int characterActionRequirementId, int characterActionRequirementCharacter, int characterActionRequirementAction)
        {
            UpdateCharacterActionRequirement(characterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction);
            CharacterActionRequirement characterActionRequirement = SelectCharacterActionRequirement(characterActionRequirementId);
            OnCharacterActionRequirementModified(characterActionRequirement);
        }

        public static void GetCharacterActionRequirement(int characterActionRequirementId)
        {
            CharacterActionRequirement characterActionRequirement = SelectCharacterActionRequirement(characterActionRequirementId);
            OnCharacterActionRequirementGet(characterActionRequirement);
        }

        public static void LoadAllCharacterActionRequirementsForAction(int action)
        {
            List<CharacterActionRequirement> characterActionRequirements = SelectAllCharacterActionRequirementsForAction(action);
            foreach (var characterActionRequirement in characterActionRequirements)
                OnCharacterActionRequirementAdded(characterActionRequirement);
        }

        #endregion


        #region Messages

        public static void AddMessage(string messageName, string messageText)
        {
            int id = InsertMessage(messageName, messageText);
            Message message = SelectMessage(id);
            OnMessageAdded(message);
        }

        public static void ModifyMessage(int messageId, string messageName, string messageText)
        {
            UpdateMessage(messageId, messageName, messageText);
            Message message = SelectMessage(messageId);
            OnMessageModified(message);
        }

        public static void GetMessage(int messageId)
        {
            Message message = SelectMessage(messageId);
            OnMessageGet(message);
        }

        public static void LoadAllMessages()
        {
            List<Message> messages = SelectAllMessages();
            foreach (var message in messages)
                OnMessageAdded(message);
        }

        #endregion


        #region MessageChoiceChoices

        public static void AddMessageChoice(string messageChoiceName, string messageChoiceText, int messageChoiceMessage)
        {
            int id = InsertMessageChoice(messageChoiceName, messageChoiceText, messageChoiceMessage);
            MessageChoice messageChoice = SelectMessageChoice(id);
            OnMessageChoiceAdded(messageChoice);
        }

        public static void ModifyMessageChoice(int messageChoiceId, string messageChoiceName, string messageChoiceText, int messageChoiceMessage)
        {
            UpdateMessageChoice(messageChoiceId, messageChoiceName, messageChoiceText, messageChoiceMessage);
            MessageChoice messageChoice = SelectMessageChoice(messageChoiceId);
            OnMessageChoiceModified(messageChoice);
        }

        public static void GetMessageChoice(int messageChoiceId)
        {
            MessageChoice messageChoice = SelectMessageChoice(messageChoiceId);
            OnMessageChoiceGet(messageChoice);
        }

        public static void LoadAllMessageChoicesForMessage(int messageId)
        {
            List<MessageChoice> messageChoices = SelectAllMessageChoicesForMessage(messageId);
            foreach (var messageChoice in messageChoices)
                OnMessageChoiceAdded(messageChoice);
        }

        #endregion


        #region MessageChoiceResults

        public static void AddMessageChoiceResult(int messageChoiceResultResult, int messageChoiceResultMessageChoice)
        {
            int id = InsertMessageChoiceResult(messageChoiceResultResult, messageChoiceResultMessageChoice);
            MessageChoiceResult messageChoiceResult = SelectMessageChoiceResult(id);
            OnMessageChoiceResultAdded(messageChoiceResult);
        }

        public static void ModifyMessageChoiceResult(int messageChoiceResultId, int messageChoiceResultResult, int messageChoiceResultMessageChoice)
        {
            UpdateMessageChoiceResult(messageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice);
            MessageChoiceResult messageChoiceResult = SelectMessageChoiceResult(messageChoiceResultId);
            OnMessageChoiceResultModified(messageChoiceResult);
        }

        public static void GetMessageChoiceResult(int messageChoiceResultId)
        {
            MessageChoiceResult messageChoiceResult = SelectMessageChoiceResult(messageChoiceResultId);
            OnMessageChoiceResultGet(messageChoiceResult);
        }

        public static void LoadAllMessageChoiceResultsForMessageChoice(int messageChoiceId)
        {
            List<MessageChoiceResult> messageChoiceResults = SelectAllMessageChoiceResultsForMessageChoice(messageChoiceId);
            foreach (var messageChoiceResult in messageChoiceResults)
                OnMessageChoiceResultAdded(messageChoiceResult);
        }

        #endregion


        #region RoomPreviews

        public static void GetRoomPreview(int room)
        {
            var roomPreview = SelectRoomPreview(room);
            foreach (var paragraphState in roomPreview.Item1)
                OnRoomPreviewParagraphStateGet(paragraphState);
        }

        #endregion


        #region AreaRoomOnInitialLoads

        public static void AddAreaRoomOnInitialLoad(int areaId, int roomId)
        {
            UpsertAreaRoomOnInitialLoad(areaId, roomId);
            AreaRoomOnInitialLoad areaRoomOnInitialLoad = SelectAreaRoomOnInitialLoad();
            OnAreaRoomOnInitialLoadAdded(areaRoomOnInitialLoad);
        }

        public static void ModifyAreaRoomOnInitialLoad(int areaId, int roomId)
        {
            UpsertAreaRoomOnInitialLoad(areaId, roomId);
            AreaRoomOnInitialLoad areaRoomOnInitialLoad = SelectAreaRoomOnInitialLoad();
            OnAreaRoomOnInitialLoadModified(areaRoomOnInitialLoad);
        }

        public static void LoadAreaRoomOnInitialLoad()
        {
            AreaRoomOnInitialLoad areaRoomOnInitialLoad = SelectAreaRoomOnInitialLoad();
            OnAreaRoomOnInitialLoadAdded(areaRoomOnInitialLoad);
        }

        #endregion

        #endregion


        #region Private Functionality

        private static void InitializeSprocsToDbModelMap()
        {
            Mapper.CreateMap<dev_GetArea_Result, Area>();
            Mapper.CreateMap<dev_GetAllAreas_Result, Area>();

            Mapper.CreateMap<dev_GetLocation_Result, Location>();
            Mapper.CreateMap<dev_GetAllLocations_Result, Location>();

            Mapper.CreateMap<dev_GetRoom_Result, Room>();
            Mapper.CreateMap<dev_GetAllRoomsInArea_Result, Room>();
            Mapper.CreateMap<dev_GetAllRoomsInAreaOnFloor_Result, Room>();

            Mapper.CreateMap<dev_GetRoomState_Result, RoomState>();
            Mapper.CreateMap<dev_GetAllRoomStatesForRoom_Result, RoomState>();

            Mapper.CreateMap<dev_GetParagraph_Result, Paragraph>();
            Mapper.CreateMap<dev_GetAllParagraphsForRoom_Result, Paragraph>();
            Mapper.CreateMap<dev_GetAllParagraphsForRoomAndRoomState_Result, Paragraph>();

            Mapper.CreateMap<dev_GetParagraphState_Result, ParagraphState>();
            Mapper.CreateMap<dev_GetAllParagraphStatesForParagraph_Result, ParagraphState>();
            Mapper.CreateMap<dev_GetParagraphStateForParagraphPreview_Result, ParagraphState>();

            Mapper.CreateMap<dev_GetNoun_Result, Noun>();
            Mapper.CreateMap<dev_GetAllNounsForParagraphState_Result, Noun>();

            Mapper.CreateMap<dev_GetVerbType_Result, VerbType>();
            Mapper.CreateMap<dev_GetAllVerbTypes_Result, VerbType>();

            Mapper.CreateMap<dev_GetVerb_Result, Verb>();
            Mapper.CreateMap<dev_GetAllVerbsForVerbType_Result, Verb>();

            Mapper.CreateMap<dev_GetAction_Result, Db.Action>();
            Mapper.CreateMap<dev_GetAllActionsForNoun_Result, Db.Action>();

            Mapper.CreateMap<dev_GetResultType_Result, ResultType>();
            Mapper.CreateMap<dev_GetAllResultTypes_Result, ResultType>();

            Mapper.CreateMap<dev_GetJSONPropertyDataType_Result, JSONPropertyDataType>();
            Mapper.CreateMap<dev_GetAllJSONPropertyDataTypes_Result, JSONPropertyDataType>();

            Mapper.CreateMap<dev_GetResultTypeJSONProperty_Result, ResultTypeJSONProperty>();
            Mapper.CreateMap<dev_GetAllResultTypeJSONPropertiesForResultType_Result, ResultTypeJSONProperty>();

            Mapper.CreateMap<dev_GetResult_Result, Result>();
            Mapper.CreateMap<dev_GetAllResultsForResultType_Result, Result>();
            Mapper.CreateMap<dev_GetAllResultsForMessageChoiceResultType_Result, Result>();
            Mapper.CreateMap<dev_GetAllResultsForActionResultType_Result, Result>();

            Mapper.CreateMap<dev_GetActionResult_Result, ActionResult>();
            Mapper.CreateMap<dev_GetAllActionResultsForAction_Result, ActionResult>();

            Mapper.CreateMap<dev_GetItem_Result, Item>();
            Mapper.CreateMap<dev_GetAllItems_Result, Item>();

            Mapper.CreateMap<dev_GetEvent_Result, Event>();
            Mapper.CreateMap<dev_GetAllEvents_Result, Event>();

            Mapper.CreateMap<dev_GetCharacter_Result, Character>();
            Mapper.CreateMap<dev_GetAllCharacters_Result, Character>();

            Mapper.CreateMap<dev_GetAllItemActionRequirementsForAction_Result, ItemActionRequirement>();
            Mapper.CreateMap<dev_GetItemActionRequirement_Result, ItemActionRequirement>();

            Mapper.CreateMap<dev_GetAllEventActionRequirementsForAction_Result, EventActionRequirement>();
            Mapper.CreateMap<dev_GetEventActionRequirement_Result, EventActionRequirement>();

            Mapper.CreateMap<dev_GetAllCharacterActionRequirementsForAction_Result, CharacterActionRequirement>();
            Mapper.CreateMap<dev_GetCharacterActionRequirement_Result, CharacterActionRequirement>();

            Mapper.CreateMap<dev_GetMessage_Result, Message>();
            Mapper.CreateMap<dev_GetAllMessages_Result, Message>();

            Mapper.CreateMap<dev_GetMessageChoice_Result, MessageChoice>();
            Mapper.CreateMap<dev_GetAllMessageChoicesForMessage_Result, MessageChoice>();

            Mapper.CreateMap<dev_GetMessageChoiceResult_Result, MessageChoiceResult>();
            Mapper.CreateMap<dev_GetAllMessageChoiceResultsForMessageChoice_Result, MessageChoiceResult>();

            Mapper.CreateMap<dev_GetRoomPreviewNouns_Result, RoomPreviewNoun>();
            Mapper.CreateMap<dev_GetRoomPreviewParagraphStates_Result, RoomPreviewParagraphState>();
            Mapper.CreateMap<RoomPreviewNoun[], RoomPreviewParagraphState>()
                .ForMember(dest => dest.Nouns, opt => opt.MapFrom(src => src));

            Mapper.CreateMap<dev_GetAreaRoomOnInitialLoad_Result, AreaRoomOnInitialLoad>();
        }

        #region Areas

        private static int InsertArea(string name)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddArea(name);
            }
            catch(Exception e)
            {
                throw new GinTubDatabaseException("dev_AddArea", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddArea", new Exception("No [Id] was returned after [Area] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateArea(int id, string name)
        {
            try
            {
                m_entities.dev_UpdateArea(id, name);
            }
            catch(Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateArea", e);
            }
        }

        private static Area SelectArea(int id)
        {
            ObjectResult<dev_GetArea_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetArea(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetArea", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetArea", new Exception(string.Format("No [Areas] record found with [Id] = {0}.", id)));

            Area area = Mapper.Map<Area>(databaseResult.Single());
            return area;
        }

        private static List<Area> SelectAllAreas()
        {
            ObjectResult<dev_GetAllAreas_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllAreas();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllAreas", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllAreas", new Exception("No [Areas] records found."));

            List<Area> areas = databaseResult.Select(r => Mapper.Map<Area>(r)).ToList();
            return areas;
        }

        #endregion


        #region Locations

        private static int InsertLocation(string name, string locationFile)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddLocation(name, locationFile);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddLocation", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddLocation", new Exception("No [Id] was returned after [Location] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateLocation(int id, string name, string locationFile)
        {
            try
            {
                m_entities.dev_UpdateLocation(id, name, locationFile);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateLocation", e);
            }
        }

        private static Location SelectLocation(int id)
        {
            ObjectResult<dev_GetLocation_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetLocation(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetLocation", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetLocation", new Exception(string.Format("No [Locations] record found with [Id] = {0}.", id)));

            Location location = Mapper.Map<Location>(databaseResult.Single());
            return location;
        }

        private static List<Location> SelectAllLocations()
        {
            ObjectResult<dev_GetAllLocations_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllLocations();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllLocations", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllLocations", new Exception("No [Locations] records found."));

            List<Location> locations = databaseResult.Select(r => Mapper.Map<Location>(r)).ToList();
            return locations;
        }

        #endregion


        #region Rooms

        private static int InsertRoom(string name, int x, int y, int z, int area)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddRoom(name, x, y, z, area);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddRoom", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddRoom", new Exception("No [Id] was returned after [Room] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateRoom(int id, string name, int x, int y, int z, int area)
        {
            try
            {
                m_entities.dev_UpdateRoom(id, name, x, y, z, area);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateRoom", e);
            }
        }

        private static Room SelectRoom(int id)
        {
            ObjectResult<dev_GetRoom_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetRoom(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetRoom", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetRoom", new Exception(string.Format("No [Rooms] record found with [Id] = {0}.", id)));

            Room room = Mapper.Map<Room>(databaseResult.Single());
            return room;
        }

        private static List<Room> SelectAllRoomsInArea(int area)
        {
            ObjectResult<dev_GetAllRoomsInArea_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllRoomsInArea(area);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllRoomsInArea", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllRoomsInArea", new Exception("No [Rooms] records found."));

            List<Room> rooms = databaseResult.Select(r => Mapper.Map<Room>(r)).ToList();
            return rooms;
        }

        private static List<Room> SelectAllRoomsInAreaOnFloor(int area, int z)
        {
            ObjectResult<dev_GetAllRoomsInAreaOnFloor_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllRoomsInAreaOnFloor(area, z);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllRoomsInAreaOnFloor", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllRoomsInAreaOnFloor", new Exception("No [Rooms] records found."));

            List<Room> rooms = databaseResult.Select(r => Mapper.Map<Room>(r)).ToList();
            return rooms;
        }

        #endregion


        #region RoomStates

        private static int InsertRoomState(DateTime? time, int location, int room)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddRoomState(time, location, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddRoomState", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddRoomState", new Exception("No [Id] was returned after [RoomState] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateRoomState(int id, int state, DateTime? time, int location, int room)
        {
            try
            {
                m_entities.dev_UpdateRoomState(id, state, time, location, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateRoomState", e);
            }
        }

        private static RoomState SelectRoomState(int id)
        {
            ObjectResult<dev_GetRoomState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetRoomState(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetRoomState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetRoomState", new Exception(string.Format("No [RoomStates] record found with [Id] = {0}.", id)));

            RoomState roomState = Mapper.Map<RoomState>(databaseResult.Single());
            return roomState;
        }

        private static List<RoomState> SelectAllRoomStatesForRoom(int room)
        {
            ObjectResult<dev_GetAllRoomStatesForRoom_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllRoomStatesForRoom(room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllRoomStatesForRoom", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllRoomStatesForRoom", new Exception("No [RoomStates] records found."));

            List<RoomState> roomStates = databaseResult.Select(r => Mapper.Map<RoomState>(r)).ToList();
            return roomStates;
        }

        #endregion


        #region Paragraphs

        private static int InsertParagraph(int order, int room, int? roomState)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddParagraph(order, room, roomState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddParagraph", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddParagraph", new Exception("No [Id] was returned after [Paragraph] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateParagraph(int id, int order, int room, int? roomState)
        {
            try
            {
                m_entities.dev_UpdateParagraph(id, order, room, roomState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateParagraph", e);
            }
        }

        private static Paragraph SelectParagraph(int id)
        {
            ObjectResult<dev_GetParagraph_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetParagraph(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetParagraph", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetParagraph", new Exception(string.Format("No [Paragraphs] record found with [Id] = {0}.", id)));

            Paragraph paragraph = Mapper.Map<Paragraph>(databaseResult.Single());
            return paragraph;
        }

        private static List<Paragraph> SelectAllParagraphsForRoom(int room)
        {
            ObjectResult<dev_GetAllParagraphsForRoom_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllParagraphsForRoom(room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllParagraphsForRoom", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllParagraphsForRoom", new Exception("No [Paragraphs] records found."));

            List<Paragraph> paragraphs = databaseResult.Select(r => Mapper.Map<Paragraph>(r)).ToList();
            return paragraphs;
        }

        private static List<Paragraph> SelectAllParagraphsForRoomAndRoomState(int room, int? roomState)
        {
            ObjectResult<dev_GetAllParagraphsForRoomAndRoomState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllParagraphsForRoomAndRoomState(room, roomState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllParagraphsForRoomAndRoomState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllParagraphsForRoomAndRoomState", new Exception("No [Paragraphs] records found."));

            List<Paragraph> paragraphs = databaseResult.Select(r => Mapper.Map<Paragraph>(r)).ToList();
            return paragraphs;
        }

        #endregion


        #region ParagraphStates

        private static int InsertParagraphState(string text, int paragraph)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddParagraphState(text, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddParagraphState", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddParagraphState", new Exception("No [Id] was returned after [ParagraphState] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateParagraphState(int id, string text, int state, int paragraph)
        {
            try
            {
                m_entities.dev_UpdateParagraphState(id, text, state, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateParagraphState", e);
            }
        }

        private static ParagraphState SelectParagraphState(int id)
        {
            ObjectResult<dev_GetParagraphState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetParagraphState(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetParagraphState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetParagraphState", new Exception(string.Format("No [ParagraphStates] record found with [Id] = {0}.", id)));

            ParagraphState paragraphState = Mapper.Map<ParagraphState>(databaseResult.Single());
            return paragraphState;
        }

        private static List<ParagraphState> SelectAllParagraphStatesForParagraph(int paragraph)
        {
            ObjectResult<dev_GetAllParagraphStatesForParagraph_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllParagraphStatesForParagraph(paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllParagraphStatesForParagraph", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllParagraphStatesForParagraph", new Exception("No [ParagraphStates] records found."));

            List<ParagraphState> paragraphStates = databaseResult.Select(r => Mapper.Map<ParagraphState>(r)).ToList();
            return paragraphStates;
        }

        private static ParagraphState SelectParagraphStateForParagraphPreview(int state, int paragraph)
        {
            ObjectResult<dev_GetParagraphStateForParagraphPreview_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetParagraphStateForParagraphPreview(state, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetParagraphStateForParagraphPreview", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetParagraphStateForParagraphPreview", new Exception(string.Format("No [ParagraphStates] record found with [State] = {0} for Paragraph with [Id] = {1}.", state, paragraph)));

            ParagraphState paragraphState = Mapper.Map<ParagraphState>(databaseResult.SingleOrDefault());
            return paragraphState;
        }

        #endregion


        #region Nouns

        private static int InsertNoun(string text, int paragraphState)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddNoun(text, paragraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddNoun", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddNoun", new Exception("No [Id] was returned after [Noun] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateNoun(int id, string text, int paragraphState)
        {
            try
            {
                m_entities.dev_UpdateNoun(id, text, paragraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateNoun", e);
            }
        }

        private static Noun SelectNoun(int id)
        {
            ObjectResult<dev_GetNoun_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetNoun(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetNoun", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetNoun", new Exception(string.Format("No [Nouns] record found with [Id] = {0}.", id)));

            Noun noun = Mapper.Map<Noun>(databaseResult.Single());
            return noun;
        }

        private static List<Noun> SelectAllNounsForParagraphState(int paragraphState)
        {
            ObjectResult<dev_GetAllNounsForParagraphState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllNounsForParagraphState(paragraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllNounsForParagraphState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllNounsForParagraphState", new Exception("No [Nouns] records found."));

            List<Noun> nouns = databaseResult.Select(r => Mapper.Map<Noun>(r)).ToList();
            return nouns;
        }

        #endregion


        #region VerbTypes

        private static int InsertVerbType(string name)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddVerbType(name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddVerbType", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddVerbType", new Exception("No [Id] was returned after [VerbType] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateVerbType(int id, string name)
        {
            try
            {
                m_entities.dev_UpdateVerbType(id, name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateVerbType", e);
            }
        }

        private static VerbType SelectVerbType(int id)
        {
            ObjectResult<dev_GetVerbType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetVerbType(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetVerbType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetVerbType", new Exception(string.Format("No [VerbTypes] record found with [Id] = {0}.", id)));

            VerbType verbType = Mapper.Map<VerbType>(databaseResult.Single());
            return verbType;
        }

        private static List<VerbType> SelectAllVerbTypes()
        {
            ObjectResult<dev_GetAllVerbTypes_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllVerbTypes();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllVerbTypes", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllVerbTypes", new Exception("No [VerbTypes] records found."));

            List<VerbType> verbTypes = databaseResult.Select(r => Mapper.Map<VerbType>(r)).ToList();
            return verbTypes;
        }

        #endregion


        #region Verbs

        private static int InsertVerb(string name, int verbType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddVerb(name, verbType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddVerb", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddVerb", new Exception("No [Id] was returned after [Verb] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateVerb(int id, string name, int verbType)
        {
            try
            {
                m_entities.dev_UpdateVerb(id, name, verbType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateVerb", e);
            }
        }

        private static Verb SelectVerb(int id)
        {
            ObjectResult<dev_GetVerb_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetVerb(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetVerb", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetVerb", new Exception(string.Format("No [Verbs] record found with [Id] = {0}.", id)));

            Verb verb = Mapper.Map<Verb>(databaseResult.Single());
            return verb;
        }

        private static List<Verb> SelectAllVerbsForVerbType(int verbType)
        {
            ObjectResult<dev_GetAllVerbsForVerbType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllVerbsForVerbType(verbType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllVerbsForVerbType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllVerbsForVerbType", new Exception("No [Verbs] records found."));

            List<Verb> verbs = databaseResult.Select(r => Mapper.Map<Verb>(r)).ToList();
            return verbs;
        }

        #endregion


        #region Actions

        private static int InsertAction(int verbType, int noun)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddAction(verbType, noun);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddAction", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddAction", new Exception("No [Id] was returned after [Action] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateAction(int id, int verbType, int noun)
        {
            try
            {
                m_entities.dev_UpdateAction(id, verbType, noun);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateAction", e);
            }
        }

        private static Db.Action SelectAction(int id)
        {
            ObjectResult<dev_GetAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAction(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAction", new Exception(string.Format("No [Actions] record found with [Id] = {0}.", id)));

            Db.Action action = Mapper.Map<Db.Action>(databaseResult.Single());
            return action;
        }

        private static List<Db.Action> SelectAllActionsForNoun(int noun)
        {
            ObjectResult<dev_GetAllActionsForNoun_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllActionsForNoun(noun);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllActionsForNoun", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllActionsForNoun", new Exception("No [Actions] records found."));

            List<Db.Action> actions = databaseResult.Select(r => Mapper.Map<Db.Action>(r)).ToList();
            return actions;
        }

        #endregion


        #region ResultTypes

        private static int InsertResultType(string name)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddResultType(name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddResultType", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddResultType", new Exception("No [Id] was returned after [ResultType] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateResultType(int id, string name)
        {
            try
            {
                m_entities.dev_UpdateResultType(id, name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateResultType", e);
            }
        }

        private static ResultType SelectResultType(int id)
        {
            ObjectResult<dev_GetResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetResultType(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetResultType", new Exception(string.Format("No [ResultTypes] record found with [Id] = {0}.", id)));

            ResultType resultType = Mapper.Map<ResultType>(databaseResult.Single());
            return resultType;
        }

        private static List<ResultType> SelectAllResultTypes()
        {
            ObjectResult<dev_GetAllResultTypes_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllResultTypes();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllResultTypes", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllResultTypes", new Exception("No [ResultTypes] records found."));

            List<ResultType> resultTypes = databaseResult.Select(r => Mapper.Map<ResultType>(r)).ToList();
            return resultTypes;
        }

        #endregion


        #region JSONPropertyDataTypes

        private static int InsertJSONPropertyDataType(string dataType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddJSONPropertyDataType(dataType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddJSONPropertyDataType", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddJSONPropertyDataType", new Exception("No [Id] was returned after [JSONPropertyDataType] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateJSONPropertyDataType(int id, string dataType)
        {
            try
            {
                m_entities.dev_UpdateJSONPropertyDataType(id, dataType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateJSONPropertyDataType", e);
            }
        }

        private static JSONPropertyDataType SelectJSONPropertyDataType(int id)
        {
            ObjectResult<dev_GetJSONPropertyDataType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetJSONPropertyDataType(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetJSONPropertyDataType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetJSONPropertyDataType", new Exception(string.Format("No [JSONPropertyDataTypes] record found with [Id] = {0}.", id)));

            JSONPropertyDataType jsonPropertyDataType = Mapper.Map<JSONPropertyDataType>(databaseResult.Single());
            return jsonPropertyDataType;
        }

        private static List<JSONPropertyDataType> SelectAllJSONPropertyDataTypes()
        {
            ObjectResult<dev_GetAllJSONPropertyDataTypes_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllJSONPropertyDataTypes();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllJSONPropertyDataTypes", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllJSONPropertyDataTypes", new Exception("No [JSONPropertyDataTypes] records found."));

            List<JSONPropertyDataType> jsonPropertyDataTypes = databaseResult.Select(r => Mapper.Map<JSONPropertyDataType>(r)).ToList();
            return jsonPropertyDataTypes;
        }

        #endregion


        #region ResultTypeJSONProperties

        private static int InsertResultTypeJSONProperty(string jsonProperty, int dataType, int resultType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddResultTypeJSONProperty(jsonProperty, dataType, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddResultTypeJSONProperty", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddResultTypeJSONProperty", new Exception("No [Id] was returned after [ResultTypeJSONProperty] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateResultTypeJSONProperty(int id, string jsonProperty, int dataType, int resultType)
        {
            try
            {
                m_entities.dev_UpdateResultTypeJSONProperty(id, jsonProperty, dataType, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateResultTypeJSONProperty", e);
            }
        }

        private static ResultTypeJSONProperty SelectResultTypeJSONProperty(int id)
        {
            ObjectResult<dev_GetResultTypeJSONProperty_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetResultTypeJSONProperty(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetResultTypeJSONProperty", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetResultTypeJSONProperty", new Exception(string.Format("No [ResultTypeJSONProperties] record found with [Id] = {0}.", id)));

            ResultTypeJSONProperty resultTypeJSONProperty = Mapper.Map<ResultTypeJSONProperty>(databaseResult.Single());
            return resultTypeJSONProperty;
        }

        private static List<ResultTypeJSONProperty> SelectAllResultTypeJSONPropertiesForResultType(int resultType)
        {
            ObjectResult<dev_GetAllResultTypeJSONPropertiesForResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllResultTypeJSONPropertiesForResultType(resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllResultTypeJSONPropertiesForResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllResultTypeJSONPropertiesForResultType", new Exception("No [ResultTypeJSONProperties] records found."));

            List<ResultTypeJSONProperty> resultTypeJSONProperties = databaseResult.Select(r => Mapper.Map<ResultTypeJSONProperty>(r)).ToList();
            return resultTypeJSONProperties;
        }

        #endregion


        #region Results

        private static int InsertResult(string name, string jsonData, int resultType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddResult(name, jsonData, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddResult", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddResult", new Exception("No [Id] was returned after [Result] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateResult(int id, string name, string jsonData, int resultType)
        {
            try
            {
                m_entities.dev_UpdateResult(id, name, jsonData, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateResult", e);
            }
        }

        private static Result SelectResult(int id)
        {
            ObjectResult<dev_GetResult_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetResult(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetResult", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetResult", new Exception(string.Format("No [Results] record found with [Id] = {0}.", id)));

            Result result = Mapper.Map<Result>(databaseResult.Single());
            return result;
        }

        private static List<Result> SelectAllResultsForResultType(int resultType)
        {
            ObjectResult<dev_GetAllResultsForResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllResultsForResultType(resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllResultsForResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllResultsForResultType", new Exception("No [Results] records found."));

            List<Result> results = databaseResult.Select(r => Mapper.Map<Result>(r)).ToList();
            return results;
        }

        private static List<Result> SelectAllResultsForActionResultType(int action)
        {
            ObjectResult<dev_GetAllResultsForActionResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllResultsForActionResultType(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllResultsForActionResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllResultsForActionResultType", new Exception("No [Results] records found."));

            List<Result> results = databaseResult.Select(r => Mapper.Map<Result>(r)).ToList();
            return results;
        }

        private static List<Result> SelectAllResultsForMessageChoiceResultType(int messageChoice)
        {
            ObjectResult<dev_GetAllResultsForMessageChoiceResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllResultsForMessageChoiceResultType(messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllResultsForMessageChoiceResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllResultsForMessageChoiceResultType", new Exception("No [Results] records found."));

            List<Result> results = databaseResult.Select(r => Mapper.Map<Result>(r)).ToList();
            return results;
        }

        #endregion


        #region ActionResults

        private static int InsertActionResult(int result, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddActionResult(result, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddActionResult", e);
            }
            var reslt = databaseResult.FirstOrDefault();
            if (!reslt.HasValue)
                throw new GinTubDatabaseException("dev_AddActionResult", new Exception("No [Id] was returned after [ActionResult] INSERT."));

            return (int)reslt.Value;
        }

        private static void UpdateActionResult(int id, int result, int action)
        {
            try
            {
                m_entities.dev_UpdateActionResult(id, result, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateActionResult", e);
            }
        }

        private static ActionResult SelectActionResult(int id)
        {
            ObjectResult<dev_GetActionResult_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetActionResult(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetActionResult", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetActionResult", new Exception(string.Format("No [ActionResults] record found with [Id] = {0}.", id)));

            ActionResult actionResult = Mapper.Map<ActionResult>(databaseResult.Single());
            return actionResult;
        }

        private static List<ActionResult> SelectAllActionResultsForAction(int action)
        {
            ObjectResult<dev_GetAllActionResultsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllActionResultsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllActionResultsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllActionResultsForAction", new Exception("No [ActionResults] records found."));

            List<ActionResult> actionResults = databaseResult.Select(r => Mapper.Map<ActionResult>(r)).ToList();
            return actionResults;
        }

        #endregion


        #region Items

        private static int InsertItem(string name, string description)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddItem(name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddItem", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddItem", new Exception("No [Id] was returned after [Item] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateItem(int id, string name, string description)
        {
            try
            {
                m_entities.dev_UpdateItem(id, name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateItem", e);
            }
        }

        private static Item SelectItem(int id)
        {
            ObjectResult<dev_GetItem_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetItem(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetItem", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetItem", new Exception(string.Format("No [Items] record found with [Id] = {0}.", id)));

            Item item = Mapper.Map<Item>(databaseResult.Single());
            return item;
        }

        private static List<Item> SelectAllItems()
        {
            ObjectResult<dev_GetAllItems_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllItems();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllItems", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllItems", new Exception("No [Items] records found."));

            List<Item> items = databaseResult.Select(r => Mapper.Map<Item>(r)).ToList();
            return items;
        }

        #endregion


        #region Events

        private static int InsertEvent(string name, string description)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddEvent(name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddEvent", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddEvent", new Exception("No [Id] was returned after [Event] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateEvent(int id, string name, string description)
        {
            try
            {
                m_entities.dev_UpdateEvent(id, name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateEvent", e);
            }
        }

        private static Event SelectEvent(int id)
        {
            ObjectResult<dev_GetEvent_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetEvent(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetEvent", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetEvent", new Exception(string.Format("No [Events] record found with [Id] = {0}.", id)));

            Event evnt = Mapper.Map<Event>(databaseResult.Single());
            return evnt;
        }

        private static List<Event> SelectAllEvents()
        {
            ObjectResult<dev_GetAllEvents_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllEvents();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllEvents", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllEvents", new Exception("No [Events] records found."));

            List<Event> evnts = databaseResult.Select(r => Mapper.Map<Event>(r)).ToList();
            return evnts;
        }

        #endregion


        #region Characters

        private static int InsertCharacter(string name, string description)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddCharacter(name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddCharacter", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddCharacter", new Exception("No [Id] was returned after [Character] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateCharacter(int id, string name, string description)
        {
            try
            {
                m_entities.dev_UpdateCharacter(id, name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateCharacter", e);
            }
        }

        private static Character SelectCharacter(int id)
        {
            ObjectResult<dev_GetCharacter_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetCharacter(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetCharacter", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetCharacter", new Exception(string.Format("No [Characters] record found with [Id] = {0}.", id)));

            Character character = Mapper.Map<Character>(databaseResult.Single());
            return character;
        }

        private static List<Character> SelectAllCharacters()
        {
            ObjectResult<dev_GetAllCharacters_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllCharacters();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllCharacters", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllCharacters", new Exception("No [Characters] records found."));

            List<Character> characters = databaseResult.Select(r => Mapper.Map<Character>(r)).ToList();
            return characters;
        }

        #endregion


        #region ItemActionRequirements

        private static int InsertItemActionRequirement(int item, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddItemActionRequirement(item, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddItemActionRequirement", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddItemActionRequirement", new Exception("No [Id] was returned after [ItemActionRequirement] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateItemActionRequirement(int id, int item, int action)
        {
            try
            {
                m_entities.dev_UpdateItemActionRequirement(id, item, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateItemActionRequirement", e);
            }
        }

        private static ItemActionRequirement SelectItemActionRequirement(int id)
        {
            ObjectResult<dev_GetItemActionRequirement_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetItemActionRequirement(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetItemActionRequirement", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetItemActionRequirement", new Exception(string.Format("No [ItemActionRequirements] record found with [Id] = {0}.", id)));

            ItemActionRequirement itemActionRequirement = Mapper.Map<ItemActionRequirement>(databaseResult.Single());
            return itemActionRequirement;
        }

        private static List<ItemActionRequirement> SelectAllItemActionRequirementsForAction(int action)
        {
            ObjectResult<dev_GetAllItemActionRequirementsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllItemActionRequirementsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllItemActionRequirementsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllItemActionRequirementsForAction", new Exception("No [ItemActionRequirements] records found."));

            List<ItemActionRequirement> itemActionRequirements = databaseResult.Select(r => Mapper.Map<ItemActionRequirement>(r)).ToList();
            return itemActionRequirements;
        }

        #endregion


        #region EventActionRequirements

        private static int InsertEventActionRequirement(int evnt, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddEventActionRequirement(evnt, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddEventActionRequirement", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddEventActionRequirement", new Exception("No [Id] was returned after [EventActionRequirement] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateEventActionRequirement(int id, int evnt, int action)
        {
            try
            {
                m_entities.dev_UpdateEventActionRequirement(id, evnt, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateEventActionRequirement", e);
            }
        }

        private static EventActionRequirement SelectEventActionRequirement(int id)
        {
            ObjectResult<dev_GetEventActionRequirement_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetEventActionRequirement(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetEventActionRequirement", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetEventActionRequirement", new Exception(string.Format("No [EventActionRequirements] record found with [Id] = {0}.", id)));

            EventActionRequirement evntActionRequirement = Mapper.Map<EventActionRequirement>(databaseResult.Single());
            return evntActionRequirement;
        }

        private static List<EventActionRequirement> SelectAllEventActionRequirementsForAction(int action)
        {
            ObjectResult<dev_GetAllEventActionRequirementsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllEventActionRequirementsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllEventActionRequirementsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllEventActionRequirementsForAction", new Exception("No [EventActionRequirements] records found."));

            List<EventActionRequirement> evntActionRequirements = databaseResult.Select(r => Mapper.Map<EventActionRequirement>(r)).ToList();
            return evntActionRequirements;
        }

        #endregion


        #region CharacterActionRequirements

        private static int InsertCharacterActionRequirement(int character, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddCharacterActionRequirement(character, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddCharacterActionRequirement", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddCharacterActionRequirement", new Exception("No [Id] was returned after [CharacterActionRequirement] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateCharacterActionRequirement(int id, int character, int action)
        {
            try
            {
                m_entities.dev_UpdateCharacterActionRequirement(id, character, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateCharacterActionRequirement", e);
            }
        }

        private static CharacterActionRequirement SelectCharacterActionRequirement(int id)
        {
            ObjectResult<dev_GetCharacterActionRequirement_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetCharacterActionRequirement(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetCharacterActionRequirement", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetCharacterActionRequirement", new Exception(string.Format("No [CharacterActionRequirements] record found with [Id] = {0}.", id)));

            CharacterActionRequirement characterActionRequirement = Mapper.Map<CharacterActionRequirement>(databaseResult.Single());
            return characterActionRequirement;
        }

        private static List<CharacterActionRequirement> SelectAllCharacterActionRequirementsForAction(int action)
        {
            ObjectResult<dev_GetAllCharacterActionRequirementsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllCharacterActionRequirementsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllCharacterActionRequirementsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllCharacterActionRequirementsForAction", new Exception("No [CharacterActionRequirements] records found."));

            List<CharacterActionRequirement> characterActionRequirements = databaseResult.Select(r => Mapper.Map<CharacterActionRequirement>(r)).ToList();
            return characterActionRequirements;
        }

        #endregion


        #region Messages

        private static int InsertMessage(string name, string text)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddMessage(name, text);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddMessage", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddMessage", new Exception("No [Id] was returned after [Message] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateMessage(int id, string name, string text)
        {
            try
            {
                m_entities.dev_UpdateMessage(id, name, text);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateMessage", e);
            }
        }

        private static Message SelectMessage(int id)
        {
            ObjectResult<dev_GetMessage_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetMessage(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetMessage", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetMessage", new Exception(string.Format("No [Messages] record found with [Id] = {0}.", id)));

            Message message = Mapper.Map<Message>(databaseResult.Single());
            return message;
        }

        private static List<Message> SelectAllMessages()
        {
            ObjectResult<dev_GetAllMessages_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllMessages();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllMessages", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllMessages", new Exception("No [Messages] records found."));

            List<Message> messages = databaseResult.Select(r => Mapper.Map<Message>(r)).ToList();
            return messages;
        }

        #endregion


        #region MessageChoices

        private static int InsertMessageChoice(string name, string text, int message)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddMessageChoice(name, text, message);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddMessageChoice", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_AddMessageChoice", new Exception("No [Id] was returned after [MessageChoice] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateMessageChoice(int id, string name, string text, int message)
        {
            try
            {
                m_entities.dev_UpdateMessageChoice(id, name, text, message);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateMessageChoice", e);
            }
        }

        private static MessageChoice SelectMessageChoice(int id)
        {
            ObjectResult<dev_GetMessageChoice_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetMessageChoice(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetMessageChoice", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetMessageChoice", new Exception(string.Format("No [MessageChoices] record found with [Id] = {0}.", id)));

            MessageChoice messageChoice = Mapper.Map<MessageChoice>(databaseResult.Single());
            return messageChoice;
        }

        private static List<MessageChoice> SelectAllMessageChoicesForMessage(int message)
        {
            ObjectResult<dev_GetAllMessageChoicesForMessage_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllMessageChoicesForMessage(message);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllMessageChoicesForMessage", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllMessageChoicesForMessage", new Exception("No [MessageChoices] records found."));

            List<MessageChoice> messageChoices = databaseResult.Select(r => Mapper.Map<MessageChoice>(r)).ToList();
            return messageChoices;
        }

        #endregion


        #region MessageChoiceResults

        private static int InsertMessageChoiceResult(int result, int messageChoice)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_AddMessageChoiceResult(result, messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_AddMessageChoiceResult", e);
            }
            var reslt = databaseResult.FirstOrDefault();
            if (!reslt.HasValue)
                throw new GinTubDatabaseException("dev_AddMessageChoiceResult", new Exception("No [Id] was returned after [MessageChoiceResult] INSERT."));

            return (int)reslt.Value;
        }

        private static void UpdateMessageChoiceResult(int id, int result, int messageChoice)
        {
            try
            {
                m_entities.dev_UpdateMessageChoiceResult(id, result, messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateMessageChoiceResult", e);
            }
        }

        private static MessageChoiceResult SelectMessageChoiceResult(int id)
        {
            ObjectResult<dev_GetMessageChoiceResult_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetMessageChoiceResult(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetMessageChoiceResult", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetMessageChoiceResult", new Exception(string.Format("No [MessageChoiceResults] record found with [Id] = {0}.", id)));

            MessageChoiceResult messageChoiceResult = Mapper.Map<MessageChoiceResult>(databaseResult.Single());
            return messageChoiceResult;
        }

        private static List<MessageChoiceResult> SelectAllMessageChoiceResultsForMessageChoice(int messageChoice)
        {
            ObjectResult<dev_GetAllMessageChoiceResultsForMessageChoice_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAllMessageChoiceResultsForMessageChoice(messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAllMessageChoiceResultsForMessageChoice", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAllMessageChoiceResultsForMessageChoice", new Exception("No [MessageChoiceResults] records found."));

            List<MessageChoiceResult> messageChoiceResults = databaseResult.Select(r => Mapper.Map<MessageChoiceResult>(r)).ToList();
            return messageChoiceResults;
        }

        #endregion


        #region RoomPreviews

        private static Tuple<List<RoomPreviewParagraphState>> SelectRoomPreview(int room)
        {
            List<RoomPreviewParagraphState> roomPreviewParagraphStates = new List<RoomPreviewParagraphState>();
            List<RoomPreviewNoun> roomPreviewNouns = new List<RoomPreviewNoun>();
            try
            {
                var paragraphStates = m_entities.dev_GetRoomPreview(room);
                roomPreviewParagraphStates.AddRange(paragraphStates.Select(r => Mapper.Map<RoomPreviewParagraphState>(r)));

                var noun = paragraphStates.GetNextResult<dev_GetRoomPreviewNouns_Result>();
                roomPreviewNouns.AddRange(noun.Select(r => Mapper.Map<RoomPreviewNoun>(r)));
                foreach (var roomPreviewParagraphState in roomPreviewParagraphStates)
                    Mapper.Map(roomPreviewNouns.Where(n => n.ParagraphState == roomPreviewParagraphState.Id).ToArray(), roomPreviewParagraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetRoomPreview", e);
            }

            return new Tuple<List<RoomPreviewParagraphState>>(roomPreviewParagraphStates);
        }

        #endregion


        #region AreaRoomOnInitialLoads

        private static void UpsertAreaRoomOnInitialLoad(int area, int room)
        {
            try
            {
                m_entities.dev_UpsertAreaRoomOnInitialLoad(area, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpsertAreaRoomOnInitialLoad", e);
            }
        }

        private static AreaRoomOnInitialLoad SelectAreaRoomOnInitialLoad()
        {
            ObjectResult<dev_GetAreaRoomOnInitialLoad_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_GetAreaRoomOnInitialLoad();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_GetAreaRoomOnInitialLoad", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_GetAreaRoomOnInitialLoad", new Exception("No [AreaRoomOnInitialLoads] record found."));

            AreaRoomOnInitialLoad areaRoomOnInitialLoad = Mapper.Map<AreaRoomOnInitialLoad>(databaseResult.Single());
            return areaRoomOnInitialLoad;
        }

        #endregion

        #endregion

        #endregion

    }
}
