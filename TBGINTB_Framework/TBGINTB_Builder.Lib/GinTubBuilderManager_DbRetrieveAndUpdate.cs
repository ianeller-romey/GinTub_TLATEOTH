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


        public class AreaReadEventArgs : AreaEventArgs
        {
            public AreaReadEventArgs(int id, string name) :  base(id, name) {}
        }
        public delegate void AreaReadEventHandler(object sender, AreaReadEventArgs args);
        public static event AreaReadEventHandler AreaRead;
        private static void OnAreaRead(Area area)
        {
            if (AreaRead != null)
                AreaRead(typeof(GinTubBuilderManager), new AreaReadEventArgs(area.Id, area.Name));
        }


        public class AreaUpdatedEventArgs : AreaEventArgs
        {
            public AreaUpdatedEventArgs(int id, string name) : base(id, name) {}
        }
        public delegate void AreaUpdatedEventHandler(object sender, AreaUpdatedEventArgs args);
        public static event AreaUpdatedEventHandler AreaUpdated;
        private static void OnAreaUpdated(Area area)
        {
            if (AreaUpdated != null)
                AreaUpdated(typeof(GinTubBuilderManager), new AreaUpdatedEventArgs(area.Id, area.Name));
        }


        public class AreaSelectEventArgs : AreaEventArgs
        {
            public int MaxX { get; set; }
            public int MinX { get; set; }
            public int MaxY { get; set; }
            public int MinY { get; set; }
            public int MaxZ { get; set; }
            public int MinZ { get; set; }
            public int NumRooms { get; set; }
            public AreaSelectEventArgs(int id, string name, int maxX, int minX, int maxY, int minY, int maxZ, int minZ, int numRooms) :
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
        public delegate void AreaSelectEventHandler(object sender, AreaSelectEventArgs args);
        public static event AreaSelectEventHandler AreaSelect;
        private static void OnAreaSelect(Area area)
        {
            if (AreaSelect != null)
                AreaSelect(typeof(GinTubBuilderManager), new AreaSelectEventArgs(area.Id, area.Name, area.MaxX, area.MinX, area.MaxY, area.MinY, area.MaxZ, area.MinZ, area.NumRooms));
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


        public class LocationReadEventArgs : LocationEventArgs
        {
            public LocationReadEventArgs(int id, string name, string locationFile) : base(id, name, locationFile) { }
        }
        public delegate void LocationReadEventHandler(object sender, LocationReadEventArgs args);
        public static event LocationReadEventHandler LocationRead;
        private static void OnLocationRead(Location location)
        {
            if (LocationRead != null)
                LocationRead(typeof(GinTubBuilderManager), new LocationReadEventArgs(location.Id, location.Name, location.LocationFile));
        }


        public class LocationUpdatedEventArgs : LocationEventArgs
        {
            public LocationUpdatedEventArgs(int id, string name, string locationFile) : base(id, name, locationFile) { }
        }
        public delegate void LocationUpdatedEventHandler(object sender, LocationUpdatedEventArgs args);
        public static event LocationUpdatedEventHandler LocationUpdated;
        private static void OnLocationUpdated(Location location)
        {
            if (LocationUpdated != null)
                LocationUpdated(typeof(GinTubBuilderManager), new LocationUpdatedEventArgs(location.Id, location.Name, location.LocationFile));
        }


        public class LocationSelectEventArgs : LocationEventArgs
        {
            public LocationSelectEventArgs(int id, string name, string locationFile) : base(id, name, locationFile) { }
        }
        public delegate void LocationSelectEventHandler(object sender, LocationSelectEventArgs args);
        public static event LocationSelectEventHandler LocationSelect;
        private static void OnLocationSelect(Location location)
        {
            if (LocationSelect != null)
                LocationSelect(typeof(GinTubBuilderManager), new LocationSelectEventArgs(location.Id, location.Name, location.LocationFile));
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


        public class RoomReadEventArgs : RoomEventArgs
        {
            public RoomReadEventArgs(int id, string name, int x, int y, int z, int area) : base(id, name, x, y, z, area) {}
        }
        public delegate void RoomReadEventHandler(object sender, RoomReadEventArgs args);
        public static event RoomReadEventHandler RoomRead;
        private static void OnRoomRead(Room room)
        {
            if (RoomRead != null)
                RoomRead(typeof(GinTubBuilderManager), new RoomReadEventArgs(room.Id, room.Name, room.X, room.Y, room.Z, room.Area));
        }


        public class RoomUpdatedEventArgs : RoomEventArgs
        {
            public RoomUpdatedEventArgs(int id, string name, int x, int y, int z, int area) : base(id, name, x, y, z, area) {}
        }
        public delegate void RoomUpdatedEventHandler(object sender, RoomUpdatedEventArgs args);
        public static event RoomUpdatedEventHandler RoomUpdated;
        private static void OnRoomUpdated(Room room)
        {
            if (RoomUpdated != null)
                RoomUpdated(typeof(GinTubBuilderManager), new RoomUpdatedEventArgs(room.Id, room.Name, room.X, room.Y, room.Z, room.Area));
        }


        public class RoomSelectEventArgs : RoomEventArgs
        {
            public RoomSelectEventArgs(int id, string name, int x, int y, int z, int area) : base(id, name, x, y, z, area) {}
        }
        public delegate void RoomSelectEventHandler(object sender, RoomSelectEventArgs args);
        public static event RoomSelectEventHandler RoomSelect;
        private static void OnRoomSelect(Room room)
        {
            if (RoomSelect != null)
                RoomSelect(typeof(GinTubBuilderManager), new RoomSelectEventArgs(room.Id, room.Name, room.X, room.Y, room.Z, room.Area));
        }

        #endregion


        #region RoomStates

        public class RoomStateEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int State { get; set; }
            public TimeSpan Time { get; set; }
            public int Location { get; set; }
            public string Name { get; set; }
            public int Room { get; set; }
            public RoomStateEventArgs(int id, int state, TimeSpan time, int location, string name, int room)
            {
                Id = id;
                State = state;
                Time = time;
                Location = location;
                Name = name;
                Room = room;
            }
        }


        public class RoomStateReadEventArgs : RoomStateEventArgs
        {
            public RoomStateReadEventArgs(int id, int state, TimeSpan time, int location, string name, int room) :
                base(id, state, time, location, name, room) { }
        }
        public delegate void RoomStateReadEventHandler(object sender, RoomStateReadEventArgs args);
        public static event RoomStateReadEventHandler RoomStateRead;
        private static void OnRoomStateRead(RoomState roomState)
        {
            if (RoomStateRead != null)
                RoomStateRead(typeof(GinTubBuilderManager),
                    new RoomStateReadEventArgs(roomState.Id, roomState.State, roomState.Time, roomState.Location, roomState.Name, roomState.Room));
        }


        public class RoomStateUpdatedEventArgs : RoomStateEventArgs
        {
            public RoomStateUpdatedEventArgs(int id, int state, TimeSpan time, int location, string name, int room) :
                base(id, state, time, location, name, room) { }
        }
        public delegate void RoomStateUpdatedEventHandler(object sender, RoomStateUpdatedEventArgs args);
        public static event RoomStateUpdatedEventHandler RoomStateUpdated;
        private static void OnRoomStateUpdated(RoomState roomState)
        {
            if (RoomStateUpdated != null)
                RoomStateUpdated(typeof(GinTubBuilderManager),
                    new RoomStateUpdatedEventArgs(roomState.Id, roomState.State, roomState.Time, roomState.Location, roomState.Name, roomState.Room));
        }


        public class RoomStateSelectEventArgs : RoomStateEventArgs
        {
            public RoomStateSelectEventArgs(int id, int state, TimeSpan time, int location, string name, int room) :
                base(id, state, time, location, name, room) { }
        }
        public delegate void RoomStateSelectEventHandler(object sender, RoomStateSelectEventArgs args);
        public static event RoomStateSelectEventHandler RoomStateSelect;
        private static void OnRoomStateSelect(RoomState roomState)
        {
            if (RoomStateSelect != null)
                RoomStateSelect(typeof(GinTubBuilderManager),
                    new RoomStateSelectEventArgs(roomState.Id, roomState.State, roomState.Time, roomState.Location, roomState.Name, roomState.Room));
        }

        #endregion


        #region Paragraphs

        public class ParagraphEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int Order { get; set; }
            public int Room { get; set; }
            public ParagraphEventArgs(int id, int order, int room)
            {
                Id = id;
                Order = order;
                Room = room;
            }
        }


        public class ParagraphReadEventArgs : ParagraphEventArgs
        {
            public ParagraphReadEventArgs(int id, int order, int room) :
                base(id, order, room) { }
        }
        public delegate void ParagraphReadEventHandler(object sender, ParagraphReadEventArgs args);
        public static event ParagraphReadEventHandler ParagraphRead;
        private static void OnParagraphRead(Paragraph paragraph)
        {
            if (ParagraphRead != null)
                ParagraphRead(typeof(GinTubBuilderManager),
                    new ParagraphReadEventArgs(paragraph.Id, paragraph.Order, paragraph.Room));
        }


        public class ParagraphUpdatedEventArgs : ParagraphEventArgs
        {
            public ParagraphUpdatedEventArgs(int id, int order, int room) :
                base(id, order, room) { }
        }
        public delegate void ParagraphUpdatedEventHandler(object sender, ParagraphUpdatedEventArgs args);
        public static event ParagraphUpdatedEventHandler ParagraphUpdated;
        private static void OnParagraphUpdated(Paragraph paragraph)
        {
            if (ParagraphUpdated != null)
                ParagraphUpdated(typeof(GinTubBuilderManager),
                    new ParagraphUpdatedEventArgs(paragraph.Id, paragraph.Order, paragraph.Room));
        }


        public class ParagraphSelectEventArgs : ParagraphEventArgs
        {
            public ParagraphSelectEventArgs(int id, int order, int room) :
                base(id, order, room) { }
        }
        public delegate void ParagraphSelectEventHandler(object sender, ParagraphSelectEventArgs args);
        public static event ParagraphSelectEventHandler ParagraphSelect;
        private static void OnParagraphSelect(Paragraph paragraph)
        {
            if (ParagraphSelect != null)
                ParagraphSelect(typeof(GinTubBuilderManager),
                    new ParagraphSelectEventArgs(paragraph.Id, paragraph.Order, paragraph.Room));
        }

        #endregion


        #region ParagraphRoomStates

        public class ParagraphRoomStateEventArgs : EventArgs
        {
            public int Id { get; set; }
            public int RoomState { get; set; }
            public string RoomStateName { get; set; }
            public int RoomStateState { get; set; }
            public TimeSpan RoomStateTime { get; set; }
            public int Paragraph { get; set; }
            public ParagraphRoomStateEventArgs(int id, int roomState, string roomStateName, int roomStateState, TimeSpan roomStateTime, int paragraph)
            {
                Id = id;
                RoomState = roomState;
                RoomStateName = roomStateName;
                RoomStateState = roomStateState;
                RoomStateTime = roomStateTime;
                Paragraph = paragraph;
            }
        }


        public class ParagraphRoomStateReadEventArgs : ParagraphRoomStateEventArgs
        {
            public ParagraphRoomStateReadEventArgs(int id, int roomState, string roomStateName, int roomStateState, TimeSpan roomStateTime, int paragraph)
                : base(id, roomState, roomStateName, roomStateState, roomStateTime, paragraph) { }
        }
        public delegate void ParagraphRoomStateReadEventHandler(object sender, ParagraphRoomStateReadEventArgs args);
        public static event ParagraphRoomStateReadEventHandler ParagraphRoomStateRead;
        private static void OnParagraphRoomStateRead(ParagraphRoomState paragraphRoomState)
        {
            if (ParagraphRoomStateRead != null)
                ParagraphRoomStateRead(typeof(GinTubBuilderManager),
                    new ParagraphRoomStateReadEventArgs
                    (
                        paragraphRoomState.Id, 
                        paragraphRoomState.RoomState, 
                        paragraphRoomState.RoomStateName,
                        paragraphRoomState.RoomStateState,
                        paragraphRoomState.RoomStateTime,
                        paragraphRoomState.Paragraph
                    ));
        }


        public class ParagraphRoomStateUpdatedEventArgs : ParagraphRoomStateEventArgs
        {
            public ParagraphRoomStateUpdatedEventArgs(int id, int roomState, string roomStateName, int roomStateState, TimeSpan roomStateTime, int paragraph)
                : base(id, roomState, roomStateName, roomStateState, roomStateTime, paragraph) { }
        }
        public delegate void ParagraphRoomStateUpdatedEventHandler(object sender, ParagraphRoomStateUpdatedEventArgs args);
        public static event ParagraphRoomStateUpdatedEventHandler ParagraphRoomStateUpdated;
        private static void OnParagraphRoomStateUpdated(ParagraphRoomState paragraphRoomState)
        {
            if (ParagraphRoomStateUpdated != null)
                ParagraphRoomStateUpdated(typeof(GinTubBuilderManager),
                    new ParagraphRoomStateUpdatedEventArgs
                    (
                        paragraphRoomState.Id,
                        paragraphRoomState.RoomState,
                        paragraphRoomState.RoomStateName,
                        paragraphRoomState.RoomStateState,
                        paragraphRoomState.RoomStateTime,
                        paragraphRoomState.Paragraph
                    ));
        }


        public class ParagraphRoomStateSelectEventArgs : ParagraphRoomStateEventArgs
        {
            public ParagraphRoomStateSelectEventArgs(int id, int roomState, string roomStateName, int roomStateState, TimeSpan roomStateTime, int paragraph)
                : base(id, roomState, roomStateName, roomStateState, roomStateTime, paragraph) { }
        }
        public delegate void ParagraphRoomStateSelectEventHandler(object sender, ParagraphRoomStateSelectEventArgs args);
        public static event ParagraphRoomStateSelectEventHandler ParagraphRoomStateSelect;
        private static void OnParagraphRoomStateSelect(ParagraphRoomState paragraphRoomState)
        {
            if (ParagraphRoomStateSelect != null)
                ParagraphRoomStateSelect(typeof(GinTubBuilderManager),
                    new ParagraphRoomStateSelectEventArgs
                    (
                        paragraphRoomState.Id,
                        paragraphRoomState.RoomState,
                        paragraphRoomState.RoomStateName,
                        paragraphRoomState.RoomStateState,
                        paragraphRoomState.RoomStateTime,
                        paragraphRoomState.Paragraph
                    ));
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


        public class ParagraphStateReadEventArgs : ParagraphStateEventArgs
        {
            public ParagraphStateReadEventArgs(int id, string text, int state, int paragraphState) :
                base(id, text, state, paragraphState) { }
        }
        public delegate void ParagraphStateReadEventHandler(object sender, ParagraphStateReadEventArgs args);
        public static event ParagraphStateReadEventHandler ParagraphStateRead;
        private static void OnParagraphStateRead(ParagraphState paragraphState)
        {
            if (ParagraphStateRead != null)
                ParagraphStateRead(typeof(GinTubBuilderManager),
                    new ParagraphStateReadEventArgs(paragraphState.Id, paragraphState.Text, paragraphState.State, paragraphState.Paragraph));
        }


        public class ParagraphStateUpdatedEventArgs : ParagraphStateEventArgs
        {
            public ParagraphStateUpdatedEventArgs(int id, string text, int state, int paragraphState) :
                base(id, text, state, paragraphState) { }
        }
        public delegate void ParagraphStateUpdatedEventHandler(object sender, ParagraphStateUpdatedEventArgs args);
        public static event ParagraphStateUpdatedEventHandler ParagraphStateUpdated;
        private static void OnParagraphStateUpdated(ParagraphState paragraphState)
        {
            if (ParagraphStateUpdated != null)
                ParagraphStateUpdated(typeof(GinTubBuilderManager),
                    new ParagraphStateUpdatedEventArgs(paragraphState.Id, paragraphState.Text, paragraphState.State, paragraphState.Paragraph));
        }


        public class ParagraphStateSelectEventArgs : ParagraphStateEventArgs
        {
            public ParagraphStateSelectEventArgs(int id, string text, int state, int paragraphState) :
                base(id, text, state, paragraphState) { }
        }
        public delegate void ParagraphStateSelectEventHandler(object sender, ParagraphStateSelectEventArgs args);
        public static event ParagraphStateSelectEventHandler ParagraphStateSelect;
        private static void OnParagraphStateSelect(ParagraphState paragraphState)
        {
            if (ParagraphStateSelect != null)
                ParagraphStateSelect(typeof(GinTubBuilderManager),
                    new ParagraphStateSelectEventArgs(paragraphState.Id, paragraphState.Text, paragraphState.State, paragraphState.Paragraph));
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


        public class NounReadEventArgs : NounEventArgs
        {
            public NounReadEventArgs(int id, string text, int paragraphState) :
                base(id, text, paragraphState) { }
        }
        public delegate void NounReadEventHandler(object sender, NounReadEventArgs args);
        public static event NounReadEventHandler NounRead;
        private static void OnNounRead(Noun noun)
        {
            if (NounRead != null)
                NounRead(typeof(GinTubBuilderManager),
                    new NounReadEventArgs(noun.Id, noun.Text, noun.ParagraphState));
        }


        public class NounUpdatedEventArgs : NounEventArgs
        {
            public NounUpdatedEventArgs(int id, string text, int paragraphState) :
                base(id, text, paragraphState) { }
        }
        public delegate void NounUpdatedEventHandler(object sender, NounUpdatedEventArgs args);
        public static event NounUpdatedEventHandler NounUpdated;
        private static void OnNounUpdated(Noun noun)
        {
            if (NounUpdated != null)
                NounUpdated(typeof(GinTubBuilderManager),
                    new NounUpdatedEventArgs(noun.Id, noun.Text, noun.ParagraphState));
        }


        public class NounSelectEventArgs : NounEventArgs
        {
            public NounSelectEventArgs(int id, string text, int paragraphState) :
                base(id, text, paragraphState) { }
        }
        public delegate void NounSelectEventHandler(object sender, NounSelectEventArgs args);
        public static event NounSelectEventHandler NounSelect;
        private static void OnNounSelect(Noun noun)
        {
            if (NounSelect != null)
                NounSelect(typeof(GinTubBuilderManager),
                    new NounSelectEventArgs(noun.Id, noun.Text, noun.ParagraphState));
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


        public class VerbTypeReadEventArgs : VerbTypeEventArgs
        {
            public VerbTypeReadEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void VerbTypeReadEventHandler(object sender, VerbTypeReadEventArgs args);
        public static event VerbTypeReadEventHandler VerbTypeRead;
        private static void OnVerbTypeRead(VerbType verbType)
        {
            if (VerbTypeRead != null)
                VerbTypeRead(typeof(GinTubBuilderManager), new VerbTypeReadEventArgs(verbType.Id, verbType.Name));
        }


        public class VerbTypeUpdatedEventArgs : VerbTypeEventArgs
        {
            public VerbTypeUpdatedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void VerbTypeUpdatedEventHandler(object sender, VerbTypeUpdatedEventArgs args);
        public static event VerbTypeUpdatedEventHandler VerbTypeUpdated;
        private static void OnVerbTypeUpdated(VerbType verbType)
        {
            if (VerbTypeUpdated != null)
                VerbTypeUpdated(typeof(GinTubBuilderManager), new VerbTypeUpdatedEventArgs(verbType.Id, verbType.Name));
        }


        public class VerbTypeSelectEventArgs : VerbTypeEventArgs
        {
            public VerbTypeSelectEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void VerbTypeSelectEventHandler(object sender, VerbTypeSelectEventArgs args);
        public static event VerbTypeSelectEventHandler VerbTypeSelect;
        private static void OnVerbTypeSelect(VerbType verbType)
        {
            if (VerbTypeSelect != null)
                VerbTypeSelect(typeof(GinTubBuilderManager), new VerbTypeSelectEventArgs(verbType.Id, verbType.Name));
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


        public class VerbReadEventArgs : VerbEventArgs
        {
            public VerbReadEventArgs(int id, string name, int verbType) : base(id, name, verbType) { }
        }
        public delegate void VerbReadEventHandler(object sender, VerbReadEventArgs args);
        public static event VerbReadEventHandler VerbRead;
        private static void OnVerbRead(Verb verb)
        {
            if (VerbRead != null)
                VerbRead(typeof(GinTubBuilderManager), new VerbReadEventArgs(verb.Id, verb.Name, verb.VerbType));
        }


        public class VerbUpdatedEventArgs : VerbEventArgs
        {
            public VerbUpdatedEventArgs(int id, string name, int verbType) : base(id, name, verbType) { }
        }
        public delegate void VerbUpdatedEventHandler(object sender, VerbUpdatedEventArgs args);
        public static event VerbUpdatedEventHandler VerbUpdated;
        private static void OnVerbUpdated(Verb verb)
        {
            if (VerbUpdated != null)
                VerbUpdated(typeof(GinTubBuilderManager), new VerbUpdatedEventArgs(verb.Id, verb.Name, verb.VerbType));
        }


        public class VerbSelectEventArgs : VerbEventArgs
        {
            public VerbSelectEventArgs(int id, string name, int verbType) : base(id, name, verbType) { }
        }
        public delegate void VerbSelectEventHandler(object sender, VerbSelectEventArgs args);
        public static event VerbSelectEventHandler VerbSelect;
        private static void OnVerbSelect(Verb verb)
        {
            if (VerbSelect != null)
                VerbSelect(typeof(GinTubBuilderManager), new VerbSelectEventArgs(verb.Id, verb.Name, verb.VerbType));
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


        public class ActionReadEventArgs : ActionEventArgs
        {
            public ActionReadEventArgs(int id, int verbType, int noun, string name) : base(id, verbType, noun, name) { }
        }
        public delegate void ActionReadEventHandler(object sender, ActionReadEventArgs args);
        public static event ActionReadEventHandler ActionRead;
        private static void OnActionRead(Db.Action action)
        {
            if (ActionRead != null)
                ActionRead(typeof(GinTubBuilderManager), new ActionReadEventArgs(action.Id, action.VerbType, action.Noun, action.Name));
        }


        public class ActionUpdatedEventArgs : ActionEventArgs
        {
            public ActionUpdatedEventArgs(int id, int verbType, int noun, string name) : base(id, verbType, noun, name) { }
        }
        public delegate void ActionUpdatedEventHandler(object sender, ActionUpdatedEventArgs args);
        public static event ActionUpdatedEventHandler ActionUpdated;
        private static void OnActionUpdated(Db.Action action)
        {
            if (ActionUpdated != null)
                ActionUpdated(typeof(GinTubBuilderManager), new ActionUpdatedEventArgs(action.Id, action.VerbType, action.Noun, action.Name));
        }


        public class ActionSelectEventArgs : ActionEventArgs
        {
            public ActionSelectEventArgs(int id, int verbType, int noun, string name) : base(id, verbType, noun, name) { }
        }
        public delegate void ActionSelectEventHandler(object sender, ActionSelectEventArgs args);
        public static event ActionSelectEventHandler ActionSelect;
        private static void OnActionSelect(Db.Action action)
        {
            if (ActionSelect != null)
                ActionSelect(typeof(GinTubBuilderManager), new ActionSelectEventArgs(action.Id, action.VerbType, action.Noun, action.Name));
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


        public class ResultTypeReadEventArgs : ResultTypeEventArgs
        {
            public ResultTypeReadEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void ResultTypeReadEventHandler(object sender, ResultTypeReadEventArgs args);
        public static event ResultTypeReadEventHandler ResultTypeRead;
        private static void OnResultTypeRead(ResultType resultType)
        {
            if (ResultTypeRead != null)
                ResultTypeRead(typeof(GinTubBuilderManager), new ResultTypeReadEventArgs(resultType.Id, resultType.Name));
        }


        public class ResultTypeUpdatedEventArgs : ResultTypeEventArgs
        {
            public ResultTypeUpdatedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void ResultTypeUpdatedEventHandler(object sender, ResultTypeUpdatedEventArgs args);
        public static event ResultTypeUpdatedEventHandler ResultTypeUpdated;
        private static void OnResultTypeUpdated(ResultType resultType)
        {
            if (ResultTypeUpdated != null)
                ResultTypeUpdated(typeof(GinTubBuilderManager), new ResultTypeUpdatedEventArgs(resultType.Id, resultType.Name));
        }


        public class ResultTypeSelectEventArgs : ResultTypeEventArgs
        {
            public ResultTypeSelectEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void ResultTypeSelectEventHandler(object sender, ResultTypeSelectEventArgs args);
        public static event ResultTypeSelectEventHandler ResultTypeSelect;
        private static void OnResultTypeSelect(ResultType resultType)
        {
            if (ResultTypeSelect != null)
                ResultTypeSelect(typeof(GinTubBuilderManager), new ResultTypeSelectEventArgs(resultType.Id, resultType.Name));
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


        public class JSONPropertyDataTypeReadEventArgs : JSONPropertyDataTypeEventArgs
        {
            public JSONPropertyDataTypeReadEventArgs(int id, string dataType) : base(id, dataType) { }
        }
        public delegate void JSONPropertyDataTypeReadEventHandler(object sender, JSONPropertyDataTypeReadEventArgs args);
        public static event JSONPropertyDataTypeReadEventHandler JSONPropertyDataTypeRead;
        private static void OnJSONPropertyDataTypeRead(JSONPropertyDataType jsonPropertyDataType)
        {
            if (JSONPropertyDataTypeRead != null)
                JSONPropertyDataTypeRead(typeof(GinTubBuilderManager), new JSONPropertyDataTypeReadEventArgs(jsonPropertyDataType.Id, jsonPropertyDataType.DataType));
        }


        public class JSONPropertyDataTypeUpdatedEventArgs : JSONPropertyDataTypeEventArgs
        {
            public JSONPropertyDataTypeUpdatedEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void JSONPropertyDataTypeUpdatedEventHandler(object sender, JSONPropertyDataTypeUpdatedEventArgs args);
        public static event JSONPropertyDataTypeUpdatedEventHandler JSONPropertyDataTypeUpdated;
        private static void OnJSONPropertyDataTypeUpdated(JSONPropertyDataType jsonPropertyDataType)
        {
            if (JSONPropertyDataTypeUpdated != null)
                JSONPropertyDataTypeUpdated(typeof(GinTubBuilderManager), new JSONPropertyDataTypeUpdatedEventArgs(jsonPropertyDataType.Id, jsonPropertyDataType.DataType));
        }


        public class JSONPropertyDataTypeSelectEventArgs : JSONPropertyDataTypeEventArgs
        {
            public JSONPropertyDataTypeSelectEventArgs(int id, string name) : base(id, name) { }
        }
        public delegate void JSONPropertyDataTypeSelectEventHandler(object sender, JSONPropertyDataTypeSelectEventArgs args);
        public static event JSONPropertyDataTypeSelectEventHandler JSONPropertyDataTypeSelect;
        private static void OnJSONPropertyDataTypeSelect(JSONPropertyDataType jsonPropertyDataType)
        {
            if (JSONPropertyDataTypeSelect != null)
                JSONPropertyDataTypeSelect(typeof(GinTubBuilderManager), new JSONPropertyDataTypeSelectEventArgs(jsonPropertyDataType.Id, jsonPropertyDataType.DataType));
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


        public class ResultTypeJSONPropertyReadEventArgs : ResultTypeJSONPropertyEventArgs
        {
            public ResultTypeJSONPropertyReadEventArgs(int id, string jsonProperty, int dataType, int resultType) :
                base(id, jsonProperty, dataType, resultType) { }
        }
        public delegate void ResultTypeJSONPropertyReadEventHandler(object sender, ResultTypeJSONPropertyReadEventArgs args);
        public static event ResultTypeJSONPropertyReadEventHandler ResultTypeJSONPropertyRead;
        private static void OnResultTypeJSONPropertyRead(ResultTypeJSONProperty resultTypeJSONProperty)
        {
            if (ResultTypeJSONPropertyRead != null)
                ResultTypeJSONPropertyRead
                (
                    typeof(GinTubBuilderManager), 
                    new ResultTypeJSONPropertyReadEventArgs
                    (
                        resultTypeJSONProperty.Id, 
                        resultTypeJSONProperty.JSONProperty, 
                        resultTypeJSONProperty.DataType,
                        resultTypeJSONProperty.ResultType
                    )
                );
        }


        public class ResultTypeJSONPropertyUpdatedEventArgs : ResultTypeJSONPropertyEventArgs
        {
            public ResultTypeJSONPropertyUpdatedEventArgs(int id, string name, int dataType, int resultType) : base(id, name, dataType, resultType) { }
        }
        public delegate void ResultTypeJSONPropertyUpdatedEventHandler(object sender, ResultTypeJSONPropertyUpdatedEventArgs args);
        public static event ResultTypeJSONPropertyUpdatedEventHandler ResultTypeJSONPropertyUpdated;
        private static void OnResultTypeJSONPropertyUpdated(ResultTypeJSONProperty resultTypeJSONProperty)
        {
            if (ResultTypeJSONPropertyUpdated != null)
                ResultTypeJSONPropertyUpdated
                (
                    typeof(GinTubBuilderManager),
                    new ResultTypeJSONPropertyUpdatedEventArgs
                    (
                        resultTypeJSONProperty.Id,
                        resultTypeJSONProperty.JSONProperty,
                        resultTypeJSONProperty.DataType,
                        resultTypeJSONProperty.ResultType
                    )
                );
        }


        public class ResultTypeJSONPropertySelectEventArgs : ResultTypeJSONPropertyEventArgs
        {
            public ResultTypeJSONPropertySelectEventArgs(int id, string name, int dataType, int resultType) : base(id, name, dataType, resultType) { }
        }
        public delegate void ResultTypeJSONPropertySelectEventHandler(object sender, ResultTypeJSONPropertySelectEventArgs args);
        public static event ResultTypeJSONPropertySelectEventHandler ResultTypeJSONPropertySelect;
        private static void OnResultTypeJSONPropertySelect(ResultTypeJSONProperty resultTypeJSONProperty)
        {
            if (ResultTypeJSONPropertySelect != null)
                ResultTypeJSONPropertySelect
                (
                    typeof(GinTubBuilderManager),
                    new ResultTypeJSONPropertySelectEventArgs
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


        public class ResultReadEventArgs : ResultEventArgs
        {
            public ResultReadEventArgs(int id, string name, string jsonData, int resultType) : base(id, name, jsonData, resultType) { }
        }
        public delegate void ResultReadEventHandler(object sender, ResultReadEventArgs args);
        public static event ResultReadEventHandler ResultRead;
        private static void OnResultRead(Result result)
        {
            if (ResultRead != null)
                ResultRead(typeof(GinTubBuilderManager), new ResultReadEventArgs(result.Id, result.Name, result.JSONData, result.ResultType));
        }


        public class ResultUpdatedEventArgs : ResultEventArgs
        {
            public ResultUpdatedEventArgs(int id, string name, string jsonData, int resultType) : base(id, name, jsonData, resultType) { }
        }
        public delegate void ResultUpdatedEventHandler(object sender, ResultUpdatedEventArgs args);
        public static event ResultUpdatedEventHandler ResultUpdated;
        private static void OnResultUpdated(Result result)
        {
            if (ResultUpdated != null)
                ResultUpdated(typeof(GinTubBuilderManager), new ResultUpdatedEventArgs(result.Id, result.Name, result.JSONData, result.ResultType));
        }


        public class ResultSelectEventArgs : ResultEventArgs
        {
            public ResultSelectEventArgs(int id, string name, string jsonData, int resultType) : base(id, name, jsonData, resultType) { }
        }
        public delegate void ResultSelectEventHandler(object sender, ResultSelectEventArgs args);
        public static event ResultSelectEventHandler ResultSelect;
        private static void OnResultSelect(Result result)
        {
            if (ResultSelect != null)
                ResultSelect(typeof(GinTubBuilderManager), new ResultSelectEventArgs(result.Id, result.Name, result.JSONData, result.ResultType));
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


        public class ActionResultReadEventArgs : ActionResultEventArgs
        {
            public ActionResultReadEventArgs(int id, int result, int action) : base(id, result, action) { }
        }
        public delegate void ActionResultReadEventHandler(object sender, ActionResultReadEventArgs args);
        public static event ActionResultReadEventHandler ActionResultRead;
        private static void OnActionResultRead(ActionResult actionResult)
        {
            if (ActionResultRead != null)
                ActionResultRead(typeof(GinTubBuilderManager),
                    new ActionResultReadEventArgs(actionResult.Id, actionResult.Result, actionResult.Action));
        }


        public class ActionResultUpdatedEventArgs : ActionResultEventArgs
        {
            public ActionResultUpdatedEventArgs(int id, int result, int action) : base(id, result, action) { }
        }
        public delegate void ActionResultUpdatedEventHandler(object sender, ActionResultUpdatedEventArgs args);
        public static event ActionResultUpdatedEventHandler ActionResultUpdated;
        private static void OnActionResultUpdated(ActionResult actionResult)
        {
            if (ActionResultUpdated != null)
                ActionResultUpdated(typeof(GinTubBuilderManager),
                    new ActionResultUpdatedEventArgs(actionResult.Id, actionResult.Result, actionResult.Action));
        }


        public class ActionResultSelectEventArgs : ActionResultEventArgs
        {
            public ActionResultSelectEventArgs(int id, int result, int action) : base(id, result, action) { }
        }
        public delegate void ActionResultSelectEventHandler(object sender, ActionResultSelectEventArgs args);
        public static event ActionResultSelectEventHandler ActionResultSelect;
        private static void OnActionResultSelect(ActionResult actionResult)
        {
            if (ActionResultSelect != null)
                ActionResultSelect(typeof(GinTubBuilderManager),
                    new ActionResultSelectEventArgs(actionResult.Id, actionResult.Result, actionResult.Action));
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


        public class ItemReadEventArgs : ItemEventArgs
        {
            public ItemReadEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void ItemReadEventHandler(object sender, ItemReadEventArgs args);
        public static event ItemReadEventHandler ItemRead;
        private static void OnItemRead(Item item)
        {
            if (ItemRead != null)
                ItemRead(typeof(GinTubBuilderManager),
                    new ItemReadEventArgs(item.Id, item.Name, item.Description));
        }


        public class ItemUpdatedEventArgs : ItemEventArgs
        {
            public ItemUpdatedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void ItemUpdatedEventHandler(object sender, ItemUpdatedEventArgs args);
        public static event ItemUpdatedEventHandler ItemUpdated;
        private static void OnItemUpdated(Item item)
        {
            if (ItemUpdated != null)
                ItemUpdated(typeof(GinTubBuilderManager),
                    new ItemUpdatedEventArgs(item.Id, item.Name, item.Description));
        }


        public class ItemSelectEventArgs : ItemEventArgs
        {
            public ItemSelectEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void ItemSelectEventHandler(object sender, ItemSelectEventArgs args);
        public static event ItemSelectEventHandler ItemSelect;
        private static void OnItemSelect(Item item)
        {
            if (ItemSelect != null)
                ItemSelect(typeof(GinTubBuilderManager),
                    new ItemSelectEventArgs(item.Id, item.Name, item.Description));
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


        public class EventReadEventArgs : EventEventArgs
        {
            public EventReadEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void EventReadEventHandler(object sender, EventReadEventArgs args);
        public static event EventReadEventHandler EventRead;
        private static void OnEventRead(Event evnt)
        {
            if (EventRead != null)
                EventRead(typeof(GinTubBuilderManager),
                    new EventReadEventArgs(evnt.Id, evnt.Name, evnt.Description));
        }


        public class EventUpdatedEventArgs : EventEventArgs
        {
            public EventUpdatedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void EventUpdatedEventHandler(object sender, EventUpdatedEventArgs args);
        public static event EventUpdatedEventHandler EventUpdated;
        private static void OnEventUpdated(Event evnt)
        {
            if (EventUpdated != null)
                EventUpdated(typeof(GinTubBuilderManager),
                    new EventUpdatedEventArgs(evnt.Id, evnt.Name, evnt.Description));
        }


        public class EventSelectEventArgs : EventEventArgs
        {
            public EventSelectEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void EventSelectEventHandler(object sender, EventSelectEventArgs args);
        public static event EventSelectEventHandler EventSelect;
        private static void OnEventSelect(Event evnt)
        {
            if (EventSelect != null)
                EventSelect(typeof(GinTubBuilderManager),
                    new EventSelectEventArgs(evnt.Id, evnt.Name, evnt.Description));
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


        public class CharacterReadEventArgs : CharacterEventArgs
        {
            public CharacterReadEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void CharacterReadCharacterHandler(object sender, CharacterReadEventArgs args);
        public static event CharacterReadCharacterHandler CharacterRead;
        private static void OnCharacterRead(Character character)
        {
            if (CharacterRead != null)
                CharacterRead(typeof(GinTubBuilderManager),
                    new CharacterReadEventArgs(character.Id, character.Name, character.Description));
        }


        public class CharacterUpdatedEventArgs : CharacterEventArgs
        {
            public CharacterUpdatedEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void CharacterUpdatedCharacterHandler(object sender, CharacterUpdatedEventArgs args);
        public static event CharacterUpdatedCharacterHandler CharacterUpdated;
        private static void OnCharacterUpdated(Character character)
        {
            if (CharacterUpdated != null)
                CharacterUpdated(typeof(GinTubBuilderManager),
                    new CharacterUpdatedEventArgs(character.Id, character.Name, character.Description));
        }


        public class CharacterSelectEventArgs : CharacterEventArgs
        {
            public CharacterSelectEventArgs(int id, string name, string description) :
                base(id, name, description) { }
        }
        public delegate void CharacterSelectCharacterHandler(object sender, CharacterSelectEventArgs args);
        public static event CharacterSelectCharacterHandler CharacterSelect;
        private static void OnCharacterSelect(Character character)
        {
            if (CharacterSelect != null)
                CharacterSelect(typeof(GinTubBuilderManager),
                    new CharacterSelectEventArgs(character.Id, character.Name, character.Description));
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


        public class ItemActionRequirementReadEventArgs : ItemActionRequirementEventArgs
        {
            public ItemActionRequirementReadEventArgs(int id, int item, int action) :
                base(id, item, action) { }
        }
        public delegate void ItemActionRequirementReadEventHandler(object sender, ItemActionRequirementReadEventArgs args);
        public static event ItemActionRequirementReadEventHandler ItemActionRequirementRead;
        private static void OnItemActionRequirementRead(ItemActionRequirement itemActionRequirement)
        {
            if (ItemActionRequirementRead != null)
                ItemActionRequirementRead(typeof(GinTubBuilderManager),
                    new ItemActionRequirementReadEventArgs(itemActionRequirement.Id, itemActionRequirement.Item, itemActionRequirement.Action));
        }


        public class ItemActionRequirementUpdatedEventArgs : ItemActionRequirementEventArgs
        {
            public ItemActionRequirementUpdatedEventArgs(int id, int item, int action) :
                base(id, item, action) { }
        }
        public delegate void ItemActionRequirementUpdatedEventHandler(object sender, ItemActionRequirementUpdatedEventArgs args);
        public static event ItemActionRequirementUpdatedEventHandler ItemActionRequirementUpdated;
        private static void OnItemActionRequirementUpdated(ItemActionRequirement itemActionRequirement)
        {
            if (ItemActionRequirementUpdated != null)
                ItemActionRequirementUpdated(typeof(GinTubBuilderManager),
                    new ItemActionRequirementUpdatedEventArgs(itemActionRequirement.Id, itemActionRequirement.Item, itemActionRequirement.Action));
        }


        public class ItemActionRequirementSelectEventArgs : ItemActionRequirementEventArgs
        {
            public ItemActionRequirementSelectEventArgs(int id, int item, int action) :
                base(id, item, action) { }
        }
        public delegate void ItemActionRequirementSelectEventHandler(object sender, ItemActionRequirementSelectEventArgs args);
        public static event ItemActionRequirementSelectEventHandler ItemActionRequirementSelect;
        private static void OnItemActionRequirementSelect(ItemActionRequirement itemActionRequirement)
        {
            if (ItemActionRequirementSelect != null)
                ItemActionRequirementSelect(typeof(GinTubBuilderManager),
                    new ItemActionRequirementSelectEventArgs(itemActionRequirement.Id, itemActionRequirement.Item, itemActionRequirement.Action));
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


        public class EventActionRequirementReadEventArgs : EventActionRequirementEventArgs
        {
            public EventActionRequirementReadEventArgs(int id, int evnt, int action) :
                base(id, evnt, action) { }
        }
        public delegate void EventActionRequirementReadEventHandler(object sender, EventActionRequirementReadEventArgs args);
        public static event EventActionRequirementReadEventHandler EventActionRequirementRead;
        private static void OnEventActionRequirementRead(EventActionRequirement evntActionRequirement)
        {
            if (EventActionRequirementRead != null)
                EventActionRequirementRead(typeof(GinTubBuilderManager),
                    new EventActionRequirementReadEventArgs(evntActionRequirement.Id, evntActionRequirement.Event, evntActionRequirement.Action));
        }


        public class EventActionRequirementUpdatedEventArgs : EventActionRequirementEventArgs
        {
            public EventActionRequirementUpdatedEventArgs(int id, int evnt, int action) :
                base(id, evnt, action) { }
        }
        public delegate void EventActionRequirementUpdatedEventHandler(object sender, EventActionRequirementUpdatedEventArgs args);
        public static event EventActionRequirementUpdatedEventHandler EventActionRequirementUpdated;
        private static void OnEventActionRequirementUpdated(EventActionRequirement evntActionRequirement)
        {
            if (EventActionRequirementUpdated != null)
                EventActionRequirementUpdated(typeof(GinTubBuilderManager),
                    new EventActionRequirementUpdatedEventArgs(evntActionRequirement.Id, evntActionRequirement.Event, evntActionRequirement.Action));
        }


        public class EventActionRequirementSelectEventArgs : EventActionRequirementEventArgs
        {
            public EventActionRequirementSelectEventArgs(int id, int evnt, int action) :
                base(id, evnt, action) { }
        }
        public delegate void EventActionRequirementSelectEventHandler(object sender, EventActionRequirementSelectEventArgs args);
        public static event EventActionRequirementSelectEventHandler EventActionRequirementSelect;
        private static void OnEventActionRequirementSelect(EventActionRequirement evntActionRequirement)
        {
            if (EventActionRequirementSelect != null)
                EventActionRequirementSelect(typeof(GinTubBuilderManager),
                    new EventActionRequirementSelectEventArgs(evntActionRequirement.Id, evntActionRequirement.Event, evntActionRequirement.Action));
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


        public class CharacterActionRequirementReadEventArgs : CharacterActionRequirementEventArgs
        {
            public CharacterActionRequirementReadEventArgs(int id, int character, int action) :
                base(id, character, action) { }
        }
        public delegate void CharacterActionRequirementReadEventHandler(object sender, CharacterActionRequirementReadEventArgs args);
        public static event CharacterActionRequirementReadEventHandler CharacterActionRequirementRead;
        private static void OnCharacterActionRequirementRead(CharacterActionRequirement characterActionRequirement)
        {
            if (CharacterActionRequirementRead != null)
                CharacterActionRequirementRead(typeof(GinTubBuilderManager),
                    new CharacterActionRequirementReadEventArgs(characterActionRequirement.Id, characterActionRequirement.Character, characterActionRequirement.Action));
        }


        public class CharacterActionRequirementUpdatedEventArgs : CharacterActionRequirementEventArgs
        {
            public CharacterActionRequirementUpdatedEventArgs(int id, int character, int action) :
                base(id, character, action) { }
        }
        public delegate void CharacterActionRequirementUpdatedEventHandler(object sender, CharacterActionRequirementUpdatedEventArgs args);
        public static event CharacterActionRequirementUpdatedEventHandler CharacterActionRequirementUpdated;
        private static void OnCharacterActionRequirementUpdated(CharacterActionRequirement characterActionRequirement)
        {
            if (CharacterActionRequirementUpdated != null)
                CharacterActionRequirementUpdated(typeof(GinTubBuilderManager),
                    new CharacterActionRequirementUpdatedEventArgs(characterActionRequirement.Id, characterActionRequirement.Character, characterActionRequirement.Action));
        }


        public class CharacterActionRequirementSelectEventArgs : CharacterActionRequirementEventArgs
        {
            public CharacterActionRequirementSelectEventArgs(int id, int character, int action) :
                base(id, character, action) { }
        }
        public delegate void CharacterActionRequirementSelectEventHandler(object sender, CharacterActionRequirementSelectEventArgs args);
        public static event CharacterActionRequirementSelectEventHandler CharacterActionRequirementSelect;
        private static void OnCharacterActionRequirementSelect(CharacterActionRequirement characterActionRequirement)
        {
            if (CharacterActionRequirementSelect != null)
                CharacterActionRequirementSelect(typeof(GinTubBuilderManager),
                    new CharacterActionRequirementSelectEventArgs(characterActionRequirement.Id, characterActionRequirement.Character, characterActionRequirement.Action));
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


        public class MessageReadEventArgs : MessageEventArgs
        {
            public MessageReadEventArgs(int id, string name, string text) :
                base(id, name, text) { }
        }
        public delegate void MessageReadMessageHandler(object sender, MessageReadEventArgs args);
        public static event MessageReadMessageHandler MessageRead;
        private static void OnMessageRead(Message message)
        {
            if (MessageRead != null)
                MessageRead(typeof(GinTubBuilderManager),
                    new MessageReadEventArgs(message.Id, message.Name, message.Text));
        }


        public class MessageUpdatedEventArgs : MessageEventArgs
        {
            public MessageUpdatedEventArgs(int id, string name, string text) :
                base(id, name, text) { }
        }
        public delegate void MessageUpdatedMessageHandler(object sender, MessageUpdatedEventArgs args);
        public static event MessageUpdatedMessageHandler MessageUpdated;
        private static void OnMessageUpdated(Message message)
        {
            if (MessageUpdated != null)
                MessageUpdated(typeof(GinTubBuilderManager),
                    new MessageUpdatedEventArgs(message.Id, message.Name, message.Text));
        }


        public class MessageSelectEventArgs : MessageEventArgs
        {
            public MessageSelectEventArgs(int id, string name, string text) :
                base(id, name, text) { }
        }
        public delegate void MessageSelectMessageHandler(object sender, MessageSelectEventArgs args);
        public static event MessageSelectMessageHandler MessageSelect;
        private static void OnMessageSelect(Message message)
        {
            if (MessageSelect != null)
                MessageSelect(typeof(GinTubBuilderManager),
                    new MessageSelectEventArgs(message.Id, message.Name, message.Text));
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


        public class MessageChoiceReadEventArgs : MessageChoiceEventArgs
        {
            public MessageChoiceReadEventArgs(int id, string name, string text, int message) :
                base(id, name, text, message) { }
        }
        public delegate void MessageChoiceReadMessageChoiceHandler(object sender, MessageChoiceReadEventArgs args);
        public static event MessageChoiceReadMessageChoiceHandler MessageChoiceRead;
        private static void OnMessageChoiceRead(MessageChoice messageChoice)
        {
            if (MessageChoiceRead != null)
                MessageChoiceRead(typeof(GinTubBuilderManager),
                    new MessageChoiceReadEventArgs(messageChoice.Id, messageChoice.Name, messageChoice.Text, messageChoice.Message));
        }


        public class MessageChoiceUpdatedEventArgs : MessageChoiceEventArgs
        {
            public MessageChoiceUpdatedEventArgs(int id, string name, string text, int message) :
                base(id, name, text, message) { }
        }
        public delegate void MessageChoiceUpdatedMessageChoiceHandler(object sender, MessageChoiceUpdatedEventArgs args);
        public static event MessageChoiceUpdatedMessageChoiceHandler MessageChoiceUpdated;
        private static void OnMessageChoiceUpdated(MessageChoice messageChoice)
        {
            if (MessageChoiceUpdated != null)
                MessageChoiceUpdated(typeof(GinTubBuilderManager),
                    new MessageChoiceUpdatedEventArgs(messageChoice.Id, messageChoice.Name, messageChoice.Text, messageChoice.Message));
        }


        public class MessageChoiceSelectEventArgs : MessageChoiceEventArgs
        {
            public MessageChoiceSelectEventArgs(int id, string name, string text, int message) :
                base(id, name, text, message) { }
        }
        public delegate void MessageChoiceSelectMessageChoiceHandler(object sender, MessageChoiceSelectEventArgs args);
        public static event MessageChoiceSelectMessageChoiceHandler MessageChoiceSelect;
        private static void OnMessageChoiceSelect(MessageChoice messageChoice)
        {
            if (MessageChoiceSelect != null)
                MessageChoiceSelect(typeof(GinTubBuilderManager),
                    new MessageChoiceSelectEventArgs(messageChoice.Id, messageChoice.Name, messageChoice.Text, messageChoice.Message));
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


        public class MessageChoiceResultReadEventArgs : MessageChoiceResultEventArgs
        {
            public MessageChoiceResultReadEventArgs(int id, int result, int messageChoice) : base(id, result, messageChoice) { }
        }
        public delegate void MessageChoiceResultReadEventHandler(object sender, MessageChoiceResultReadEventArgs args);
        public static event MessageChoiceResultReadEventHandler MessageChoiceResultRead;
        private static void OnMessageChoiceResultRead(MessageChoiceResult messageChoiceResult)
        {
            if (MessageChoiceResultRead != null)
                MessageChoiceResultRead(typeof(GinTubBuilderManager),
                    new MessageChoiceResultReadEventArgs(messageChoiceResult.Id, messageChoiceResult.Result, messageChoiceResult.MessageChoice));
        }


        public class MessageChoiceResultUpdatedEventArgs : MessageChoiceResultEventArgs
        {
            public MessageChoiceResultUpdatedEventArgs(int id, int result, int messageChoice) : base(id, result, messageChoice) { }
        }
        public delegate void MessageChoiceResultUpdatedEventHandler(object sender, MessageChoiceResultUpdatedEventArgs args);
        public static event MessageChoiceResultUpdatedEventHandler MessageChoiceResultUpdated;
        private static void OnMessageChoiceResultUpdated(MessageChoiceResult messageChoiceResult)
        {
            if (MessageChoiceResultUpdated != null)
                MessageChoiceResultUpdated(typeof(GinTubBuilderManager),
                    new MessageChoiceResultUpdatedEventArgs(messageChoiceResult.Id, messageChoiceResult.Result, messageChoiceResult.MessageChoice));
        }


        public class MessageChoiceResultSelectEventArgs : MessageChoiceResultEventArgs
        {
            public MessageChoiceResultSelectEventArgs(int id, int result, int messageChoice) : base(id, result, messageChoice) { }
        }
        public delegate void MessageChoiceResultSelectEventHandler(object sender, MessageChoiceResultSelectEventArgs args);
        public static event MessageChoiceResultSelectEventHandler MessageChoiceResultSelect;
        private static void OnMessageChoiceResultSelect(MessageChoiceResult messageChoiceResult)
        {
            if (MessageChoiceResultSelect != null)
                MessageChoiceResultSelect(typeof(GinTubBuilderManager),
                    new MessageChoiceResultSelectEventArgs(messageChoiceResult.Id, messageChoiceResult.Result, messageChoiceResult.MessageChoice));
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


        public class RoomPreviewParagraphStateSelectEventArgs : RoomPreviewParagraphStateEventArgs
        {
            public RoomPreviewParagraphStateSelectEventArgs(int id, string text, int state, int paragraphState, int room, RoomPreviewNounEventArgs[] nouns) :
                base(id, text, state, paragraphState, room, nouns) { }
        }
        public delegate void RoomPreviewParagraphStateSelectEventHandler(object sender, RoomPreviewParagraphStateSelectEventArgs args);
        public static event RoomPreviewParagraphStateSelectEventHandler RoomPreviewParagraphStateSelect;
        private static void OnRoomPreviewParagraphStateSelect(RoomPreviewParagraphState roomPreviewParagraphState)
        {
            if (RoomPreviewParagraphStateSelect != null)
                RoomPreviewParagraphStateSelect(typeof(GinTubBuilderManager),
                    new RoomPreviewParagraphStateSelectEventArgs
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


        public class RoomPreviewNounSelectEventArgs : RoomPreviewNounEventArgs
        {
            public RoomPreviewNounSelectEventArgs(int id, string text, int paragraphState, int room) :
                base(id, text, paragraphState, room) { }
        }
        public delegate void RoomPreviewNounSelectEventHandler(object sender, RoomPreviewNounSelectEventArgs args);
        public static event RoomPreviewNounSelectEventHandler RoomPreviewNounSelect;
        private static void OnRoomPreviewNounSelect(RoomPreviewNoun roomPreviewNoun)
        {
            if (RoomPreviewNounSelect != null)
                RoomPreviewNounSelect(typeof(GinTubBuilderManager),
                    new RoomPreviewNounSelectEventArgs
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


        public class AreaRoomOnInitialLoadReadEventArgs : AreaRoomOnInitialLoadEventArgs
        {
            public AreaRoomOnInitialLoadReadEventArgs(int? area, int? room) : base(area, room) { }
        }
        public delegate void AreaRoomOnInitialLoadReadEventHandler(object sender, AreaRoomOnInitialLoadReadEventArgs args);
        public static event AreaRoomOnInitialLoadReadEventHandler AreaRoomOnInitialLoadRead;
        private static void OnAreaRoomOnInitialLoadRead(AreaRoomOnInitialLoad areaRoomOnInitialRead)
        {
            if (AreaRoomOnInitialLoadRead != null)
                AreaRoomOnInitialLoadRead(typeof(GinTubBuilderManager), new AreaRoomOnInitialLoadReadEventArgs(areaRoomOnInitialRead.Area, areaRoomOnInitialRead.Room));
        }


        public class AreaRoomOnInitialLoadUpdatedEventArgs : AreaRoomOnInitialLoadEventArgs
        {
            public AreaRoomOnInitialLoadUpdatedEventArgs(int? area, int? room) : base(area, room) { }
        }
        public delegate void AreaRoomOnInitialLoadUpdatedEventHandler(object sender, AreaRoomOnInitialLoadUpdatedEventArgs args);
        public static event AreaRoomOnInitialLoadUpdatedEventHandler AreaRoomOnInitialLoadUpdated;
        private static void OnAreaRoomOnInitialLoadUpdated(AreaRoomOnInitialLoad areaRoomOnInitialRead)
        {
            if (AreaRoomOnInitialLoadUpdated != null)
                AreaRoomOnInitialLoadUpdated(typeof(GinTubBuilderManager), new AreaRoomOnInitialLoadUpdatedEventArgs(areaRoomOnInitialRead.Area, areaRoomOnInitialRead.Room));
        }


        public class AreaRoomOnInitialLoadSelectEventArgs : AreaRoomOnInitialLoadEventArgs
        {
            public AreaRoomOnInitialLoadSelectEventArgs(int? area, int? room) : base(area, room) { }
        }
        public delegate void AreaRoomOnInitialLoadSelectEventHandler(object sender, AreaRoomOnInitialLoadSelectEventArgs args);
        public static event AreaRoomOnInitialLoadSelectEventHandler AreaRoomOnInitialLoadSelect;
        private static void OnAreaRoomOnInitialLoadSelect(AreaRoomOnInitialLoad areaRoomOnInitialRead)
        {
            if (AreaRoomOnInitialLoadSelect != null)
                AreaRoomOnInitialLoadSelect(typeof(GinTubBuilderManager), new AreaRoomOnInitialLoadSelectEventArgs(areaRoomOnInitialRead.Area, areaRoomOnInitialRead.Room));
        }

        #endregion

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        #region Areas

        public static void CreateArea(string areaName)
        {
            int id = CreateAreaDb(areaName);
            Area area = ReadAreaDb(id);
            OnAreaRead(area);
        }

        public static void UpdateArea(int areaId, string areaName)
        {
            UpdateAreaDb(areaId, areaName);
            Area area = ReadAreaDb(areaId);
            OnAreaUpdated(area);
        }

        public static void SelectArea(int areaId)
        {
            Area area = ReadAreaDb(areaId);
            OnAreaSelect(area);
        }

        public static void ReadAllAreas()
        {
            List<Area> areas = ReadAllAreasDb();
            foreach (var area in areas)
                OnAreaRead(area);
        }

        #endregion


        #region Locations

        public static void CreateLocation(string locationName, string locationFile)
        {
            int id = CreateLocationDb(locationName, locationFile);
            Location location = ReadLocationDb(id);
            OnLocationRead(location);
        }

        public static void UpdateLocation(int locationId, string locationName, string locationFile)
        {
            UpdateLocationDb(locationId, locationName, locationFile);
            Location location = ReadLocationDb(locationId);
            OnLocationUpdated(location);
        }

        public static void SelectLocation(int locationId)
        {
            Location location = ReadLocationDb(locationId);
            OnLocationSelect(location);
        }

        public static void ReadAllLocations()
        {
            List<Location> locations = ReadAllLocationsDb();
            foreach (var location in locations)
                OnLocationRead(location);
        }

        #endregion


        #region Rooms

        public static void CreateRoom(string roomName, int roomX, int roomY, int roomZ, int areaId)
        {
            Tuple<int, int> ids = CreateRoomDb(roomName, roomX, roomY, roomZ, areaId);

            Room room = ReadRoomDb(ids.Item1);
            OnRoomRead(room);

            RoomState roomState = ReadRoomStateDb(ids.Item2);
            OnRoomStateRead(roomState);

        }

        public static void UpdateRoom(int roomId, string roomName, int roomX, int roomY, int roomZ, int areaId)
        {
            UpdateRoomDb(roomId, roomName, roomX, roomY, roomZ, areaId);
            Room room = ReadRoomDb(roomId);
            OnRoomUpdated(room);
        }

        public static void SelectRoom(int roomId)
        {
            Room room = ReadRoomDb(roomId);
            OnRoomSelect(room);
        }

        public static void ReadAllRoomsInArea(int areaId)
        {
            List<Room> rooms = ReadAllRoomsInAreaDb(areaId);
            foreach (var room in rooms)
                OnRoomRead(room);
        }

        public static void ReadAllRoomsInAreaOnFloor(int areaId, int z)
        {
            List<Room> rooms = ReadAllRoomsInAreaOnFloorDb(areaId, z);
            foreach (var room in rooms)
                OnRoomRead(room);
        }

        #endregion


        #region RoomStates

        public static void CreateRoomState(TimeSpan roomStateTime, int locationId, int roomId)
        {
            int id = CreateRoomStateDb(roomStateTime, locationId, roomId);
            RoomState roomState = ReadRoomStateDb(id);
            OnRoomStateRead(roomState);
        }

        public static void UpdateRoomState(int roomStateId, int roomStateState, TimeSpan roomStateTime, int locationId, int roomId)
        {
            UpdateRoomStateDb(roomStateId, roomStateState, roomStateTime, locationId, roomId);
            RoomState roomState = ReadRoomStateDb(roomStateId);
            OnRoomStateUpdated(roomState);
        }

        public static void SelectRoomState(int roomStateId)
        {
            RoomState roomState = ReadRoomStateDb(roomStateId);
            OnRoomStateSelect(roomState);
        }

        public static void ReadAllRoomStatesForRoom(int roomId)
        {
            List<RoomState> roomStates = ReadAllRoomStatesForRoomDb(roomId);
            foreach (var roomState in roomStates)
                OnRoomStateRead(roomState);
        }

        #endregion


        #region Paragraphs

        public static void CreateParagraph(int paragraphOrder, int roomId)
        {
            Tuple<int, int> ids = CreateParagraphDb(paragraphOrder, roomId);

            Paragraph paragraph = ReadParagraphDb(ids.Item1);
            OnParagraphRead(paragraph);

            ParagraphState paragraphState = ReadParagraphStateDb(ids.Item2);
            OnParagraphStateRead(paragraphState);
        }

        public static void UpdateParagraph(int paragraphId, int paragraphOrder, int roomId)
        {
            UpdateParagraphDb(paragraphId, paragraphOrder, roomId);
            Paragraph paragraph = ReadParagraphDb(paragraphId);
            OnParagraphUpdated(paragraph);
        }

        public static void SelectParagraph(int paragraphId)
        {
            Paragraph paragraph = ReadParagraphDb(paragraphId);
            OnParagraphSelect(paragraph);
        }

        public static void ReadAllParagraphForRoom(int roomId)
        {
            List<Paragraph> paragraphs = ReadAllParagraphsForRoomDb(roomId);
            foreach (var paragraph in paragraphs)
                OnParagraphRead(paragraph);
        }

        public static void ReadAllParagraphsForRoomAndRoomState(int roomId, int? roomStateId)
        {
            List<Paragraph> paragraphs = ReadAllParagraphsForRoomAndRoomStateDb(roomId, roomStateId);
            foreach (var paragraph in paragraphs)
                OnParagraphRead(paragraph);
        }

        #endregion


        #region ParagraphRoomStates

        public static void CreateParagraphRoomState(int paragraphRoomStateRoomState, int paragraphRoomStateParagraph)
        {
            int id = CreateParagraphRoomStateDb(paragraphRoomStateRoomState, paragraphRoomStateParagraph);
            ParagraphRoomState paragraphRoomState = ReadParagraphRoomStateDb(id);
            OnParagraphRoomStateRead(paragraphRoomState);
        }

        public static void UpdateParagraphRoomState(int paragraphRoomStateId, int paragraphRoomStateRoomState, int paragraphRoomStateParagraph)
        {
            UpdateParagraphRoomStateDb(paragraphRoomStateId, paragraphRoomStateRoomState, paragraphRoomStateParagraph);
            ParagraphRoomState paragraphRoomState = ReadParagraphRoomStateDb(paragraphRoomStateId);
            OnParagraphRoomStateUpdated(paragraphRoomState);
        }

        public static void SelectParagraphRoomState(int paragraphRoomStateId)
        {
            ParagraphRoomState paragraphRoomState = ReadParagraphRoomStateDb(paragraphRoomStateId);
            OnParagraphRoomStateSelect(paragraphRoomState);
        }

        public static void ReadAllParagraphRoomStatesForParagraph(int paragraphId)
        {
            List<ParagraphRoomState> paragraphRoomStates = ReadAllParagraphRoomStatesForParagraphDb(paragraphId);
            foreach (var paragraphRoomState in paragraphRoomStates)
                OnParagraphRoomStateRead(paragraphRoomState);
        }

        public static void DeleteAllParagraphRoomStatesForParagraph(int paragraphId)
        {
            DeleteAllParagraphRoomStatesForParagraphDb(paragraphId);
        }

        #endregion


        #region ParagraphStates

        public static void CreateParagraphState(string paragraphStateText, int paragraphId)
        {
            int id = CreateParagraphStateDb(paragraphStateText, paragraphId);
            ParagraphState paragraphState = ReadParagraphStateDb(id);
            OnParagraphStateRead(paragraphState);
        }

        public static void UpdateParagraphState(int paragraphStateId, string paragraphStateText, int paragraphStateState, int paragraphId)
        {
            UpdateParagraphStateDb(paragraphStateId, paragraphStateText, paragraphStateState, paragraphId);
            ParagraphState paragraphState = ReadParagraphStateDb(paragraphStateId);
            OnParagraphStateUpdated(paragraphState);
        }

        public static void SelectParagraphState(int paragraphStateId)
        {
            ParagraphState paragraphState = ReadParagraphStateDb(paragraphStateId);
            OnParagraphStateSelect(paragraphState);
        }

        public static void ReadAllParagraphStatesForParagraph(int paragraphId)
        {
            List<ParagraphState> paragraphStates = ReadAllParagraphStatesForParagraphDb(paragraphId);
            foreach (var paragraphState in paragraphStates)
                OnParagraphStateRead(paragraphState);
        }

        public static void ReadParagraphStateNounPossibilities(int paragraphStateId)
        {
            ParagraphState paragraphState = ReadParagraphStateDb(paragraphStateId);
            OnParagraphStateRead(paragraphState);
        }

        public static void ReadParagraphStateForParagraphPreview(int paragraphStateState, int paragraphId)
        {
            ParagraphState paragraphState = ReadParagraphStateForParagraphPreviewDb(paragraphStateState, paragraphId);
            if(paragraphState != null)
                OnParagraphStateRead(paragraphState);
        }

        public static void SelectParagraphStateForParagraphPreview(int paragraphStateState, int paragraphId)
        {
            ParagraphState paragraphState = ReadParagraphStateForParagraphPreviewDb(paragraphStateState, paragraphId);
            if (paragraphState != null)
                OnParagraphStateSelect(paragraphState);
        }

        #endregion


        #region Nouns

        public static void CreateNoun(string nounText, int paragraphStateId)
        {
            int id = CreateNounDb(nounText, paragraphStateId);
            Noun noun = ReadNounDb(id);
            OnNounRead(noun);
        }

        public static void UpdateNoun(int nounId, string nounText, int paragraphStateId)
        {
            UpdateNounDb(nounId, nounText, paragraphStateId);
            Noun noun = ReadNounDb(nounId);
            OnNounUpdated(noun);
        }

        public static void SelectNoun(int nounId)
        {
            Noun noun = ReadNounDb(nounId);
            OnNounSelect(noun);
        }

        public static void ReadAllNounsForParagraphState(int paragraphStateId)
        {
            List<Noun> nouns = ReadAllNounsForParagraphStateDb(paragraphStateId);
            foreach (var noun in nouns)
                OnNounRead(noun);
        }

        #endregion


        #region VerbTypes

        public static void CreateVerbType(string verbTypeName)
        {
            int id = CreateVerbTypeDb(verbTypeName);
            VerbType verbType = ReadVerbTypeDb(id);
            OnVerbTypeRead(verbType);
        }

        public static void UpdateVerbType(int verbTypeId, string verbTypeName)
        {
            UpdateVerbTypeDb(verbTypeId, verbTypeName);
            VerbType verbType = ReadVerbTypeDb(verbTypeId);
            OnVerbTypeUpdated(verbType);
        }

        public static void SelectVerbType(int verbTypeId)
        {
            VerbType verbType = ReadVerbTypeDb(verbTypeId);
            OnVerbTypeSelect(verbType);
        }

        public static void ReadAllVerbTypes()
        {
            List<VerbType> verbTypes = ReadAllVerbTypesDb();
            foreach (var verbType in verbTypes)
                OnVerbTypeRead(verbType);
        }

        #endregion


        #region Verbs

        public static void CreateVerb(string verbName, int verbTypeId)
        {
            int id = CreateVerbDb(verbName, verbTypeId);
            Verb verb = ReadVerbDb(id);
            OnVerbRead(verb);
        }

        public static void UpdateVerb(int verbId, string verbName, int verbTypeId)
        {
            UpdateVerbDb(verbId, verbName, verbTypeId);
            Verb verb = ReadVerbDb(verbId);
            OnVerbUpdated(verb);
        }

        public static void SelectVerb(int verbId)
        {
            Verb verb = ReadVerbDb(verbId);
            OnVerbSelect(verb);
        }

        public static void ReadAllVerbsForVerbType(int verbTypeId)
        {
            List<Verb> verbs = ReadAllVerbsForVerbTypeDb(verbTypeId);
            foreach (var verb in verbs)
                OnVerbRead(verb);
        }

        #endregion


        #region Actions

        public static void CreateAction(int actionVerbType, int actionNoun)
        {
            int id = CreateActionDb(actionVerbType, actionNoun);
            Db.Action action = ReadActionDb(id);
            OnActionRead(action);
        }

        public static void UpdateAction(int actionId, int actionVerbType, int actionNoun)
        {
            UpdateActionDb(actionId, actionVerbType, actionNoun);
            Db.Action action = ReadActionDb(actionId);
            OnActionUpdated(action);
        }

        public static void SelectAction(int actionId)
        {
            Db.Action action = ReadActionDb(actionId);
            OnActionSelect(action);
        }

        public static void ReadAllActionsForNoun(int nounId)
        {
            List<Db.Action> actions = ReadAllActionsForNounDb(nounId);
            foreach (var action in actions)
                OnActionRead(action);
        }

        #endregion


        #region ResultTypes

        public static void CreateResultType(string resultTypeName)
        {
            int id = CreateResultTypeDb(resultTypeName);
            ResultType resultType = ReadResultTypeDb(id);
            OnResultTypeRead(resultType);
        }

        public static void UpdateResultType(int resultTypeId, string resultTypeName)
        {
            UpdateResultTypeDb(resultTypeId, resultTypeName);
            ResultType resultType = ReadResultTypeDb(resultTypeId);
            OnResultTypeUpdated(resultType);
        }

        public static void SelectResultType(int resultTypeId)
        {
            ResultType resultType = ReadResultTypeDb(resultTypeId);
            OnResultTypeSelect(resultType);
        }

        public static void ReadAllResultTypes()
        {
            List<ResultType> resultTypes = ReadAllResultTypesDb();
            foreach (var resultType in resultTypes)
                OnResultTypeRead(resultType);
        }

        #endregion


        #region JSONPropertyDataTypes

        public static void CreateJSONPropertyDataType(string jsonPropertyDataTypeDataType)
        {
            int id = CreateJSONPropertyDataTypeDb(jsonPropertyDataTypeDataType);
            JSONPropertyDataType jsonPropertyDataType = ReadJSONPropertyDataTypeDb(id);
            OnJSONPropertyDataTypeRead(jsonPropertyDataType);
        }

        public static void UpdateJSONPropertyDataType(int jsonPropertyDataTypeId, string jsonPropertyDataTypeDataType)
        {
            UpdateJSONPropertyDataTypeDb(jsonPropertyDataTypeId, jsonPropertyDataTypeDataType);
            JSONPropertyDataType jsonPropertyDataType = ReadJSONPropertyDataTypeDb(jsonPropertyDataTypeId);
            OnJSONPropertyDataTypeUpdated(jsonPropertyDataType);
        }

        public static void SelectJSONPropertyDataType(int jsonPropertyDataTypeId)
        {
            JSONPropertyDataType jsonPropertyDataType = ReadJSONPropertyDataTypeDb(jsonPropertyDataTypeId);
            OnJSONPropertyDataTypeSelect(jsonPropertyDataType);
        }

        public static void ReadAllJSONPropertyDataTypes()
        {
            List<JSONPropertyDataType> jsonPropertyDataTypes = ReadAllJSONPropertyDataTypesDb();
            foreach (var jsonPropertyDataType in jsonPropertyDataTypes)
                OnJSONPropertyDataTypeRead(jsonPropertyDataType);
        }

        #endregion


        #region ResultTypeJSONProperties

        public static void CreateResultTypeJSONProperty(string resultTypeJSONPropertyJSONProperty, int resultTypeJSONPropertyDataType, int resultTypeJSONPropertyResultTypeId)
        {
            int id = CreateResultTypeJSONPropertyDb(resultTypeJSONPropertyJSONProperty, resultTypeJSONPropertyDataType, resultTypeJSONPropertyResultTypeId);
            ResultTypeJSONProperty resultTypeJSONProperty = ReadResultTypeJSONPropertyDb(id);
            OnResultTypeJSONPropertyRead(resultTypeJSONProperty);
        }

        public static void UpdateResultTypeJSONProperty(int resultTypeJSONPropertyId, string resultTypeJSONPropertyJSONProperty, int resultTypeJSONPropertyDataType, int resultTypeJSONPropertyResultTypeId)
        {
            UpdateResultTypeJSONPropertyDb(resultTypeJSONPropertyId, resultTypeJSONPropertyJSONProperty, resultTypeJSONPropertyDataType, resultTypeJSONPropertyResultTypeId);
            ResultTypeJSONProperty resultTypeJSONProperty = ReadResultTypeJSONPropertyDb(resultTypeJSONPropertyId);
            OnResultTypeJSONPropertyUpdated(resultTypeJSONProperty);
        }

        public static void SelectResultTypeJSONProperty(int resultTypeJSONPropertyId)
        {
            ResultTypeJSONProperty resultTypeJSONProperty = ReadResultTypeJSONPropertyDb(resultTypeJSONPropertyId);
            OnResultTypeJSONPropertySelect(resultTypeJSONProperty);
        }

        public static void ReadAllResultTypeJSONPropertiesForResultType(int resultTypeId)
        {
            List<ResultTypeJSONProperty> resultTypeJSONProperties = ReadAllResultTypeJSONPropertiesForResultTypeDb(resultTypeId);
            foreach (var resultTypeJSONProperty in resultTypeJSONProperties)
                OnResultTypeJSONPropertyRead(resultTypeJSONProperty);
        }

        #endregion


        #region Results

        public static void CreateResult(string resultName, string resultJSONData, int resultTypeId)
        {
            int id = CreateResultDb(resultName, resultJSONData, resultTypeId);
            Result result = ReadResultDb(id);
            OnResultRead(result);
        }

        public static void UpdateResult(int resultId, string resultName, string resultJSONData, int resultTypeId)
        {
            UpdateResultDb(resultId, resultName, resultJSONData, resultTypeId);
            Result result = ReadResultDb(resultId);
            OnResultUpdated(result);
        }

        public static void SelectResult(int resultId)
        {
            Result result = ReadResultDb(resultId);
            OnResultSelect(result);
        }

        public static void ReadAllResultsForResultType(int resultTypeId)
        {
            List<Result> results = ReadAllResultsForResultTypeDb(resultTypeId);
            foreach (var result in results)
                OnResultRead(result);
        }

        public static void ReadAllResultsForActionResultType(int actionId)
        {
            List<Result> results = ReadAllResultsForActionResultTypeDb(actionId);
            foreach (var result in results)
                OnResultRead(result);
        }

        public static void ReadAllResultsForMessageChoiceResultType(int messageChoiceId)
        {
            List<Result> results = ReadAllResultsForMessageChoiceResultTypeDb(messageChoiceId);
            foreach (var result in results)
                OnResultRead(result);
        }

        #endregion


        #region ActionResults

        public static void CreateActionResult(int actionResultResult, int actionResultAction)
        {
            int id = CreateActionResultDb(actionResultResult, actionResultAction);
            ActionResult actionResult = ReadActionResultDb(id);
            OnActionResultRead(actionResult);
        }

        public static void UpdateActionResult(int actionResultId, int actionResultResult, int actionResultAction)
        {
            UpdateActionResultDb(actionResultId, actionResultResult, actionResultAction);
            ActionResult actionResult = ReadActionResultDb(actionResultId);
            OnActionResultUpdated(actionResult);
        }

        public static void SelectActionResult(int actionResultId)
        {
            ActionResult actionResult = ReadActionResultDb(actionResultId);
            OnActionResultSelect(actionResult);
        }

        public static void ReadAllActionResultsForAction(int actionId)
        {
            List<ActionResult> actionResults = ReadAllActionResultsForActionDb(actionId);
            foreach (var actionResult in actionResults)
                OnActionResultRead(actionResult);
        }

        #endregion


        #region Items

        public static void CreateItem(string itemName, string itemDescription)
        {
            int id = CreateItemDb(itemName, itemDescription);
            Item item = ReadItemDb(id);
            OnItemRead(item);
        }

        public static void UpdateItem(int itemId, string itemName, string itemDescription)
        {
            UpdateItemDb(itemId, itemName, itemDescription);
            Item item = ReadItemDb(itemId);
            OnItemUpdated(item);
        }

        public static void SelectItem(int itemId)
        {
            Item item = ReadItemDb(itemId);
            OnItemSelect(item);
        }

        public static void ReadAllItems()
        {
            List<Item> items = ReadAllItemsDb();
            foreach (var item in items)
                OnItemRead(item);
        }

        #endregion


        #region Events

        public static void CreateEvent(string evntName, string evntDescription)
        {
            int id = CreateEventDb(evntName, evntDescription);
            Event evnt = ReadEventDb(id);
            OnEventRead(evnt);
        }

        public static void UpdateEvent(int evntId, string evntName, string evntDescription)
        {
            UpdateEventDb(evntId, evntName, evntDescription);
            Event evnt = ReadEventDb(evntId);
            OnEventUpdated(evnt);
        }

        public static void SelectEvent(int evntId)
        {
            Event evnt = ReadEventDb(evntId);
            OnEventSelect(evnt);
        }

        public static void ReadAllEvents()
        {
            List<Event> evnts = ReadAllEventsDb();
            foreach (var evnt in evnts)
                OnEventRead(evnt);
        }

        #endregion


        #region Characters

        public static void CreateCharacter(string characterName, string characterDescription)
        {
            int id = CreateCharacterDb(characterName, characterDescription);
            Character character = ReadCharacterDb(id);
            OnCharacterRead(character);
        }

        public static void UpdateCharacter(int characterId, string characterName, string characterDescription)
        {
            UpdateCharacterDb(characterId, characterName, characterDescription);
            Character character = ReadCharacterDb(characterId);
            OnCharacterUpdated(character);
        }

        public static void SelectCharacter(int characterId)
        {
            Character character = ReadCharacterDb(characterId);
            OnCharacterSelect(character);
        }

        public static void ReadAllCharacters()
        {
            List<Character> characters = ReadAllCharactersDb();
            foreach (var character in characters)
                OnCharacterRead(character);
        }

        #endregion


        #region ItemActionRequirements

        public static void CreateItemActionRequirement(int itemActionRequirementItem, int itemActionRequirementAction)
        {
            int id = CreateItemActionRequirementDb(itemActionRequirementItem, itemActionRequirementAction);
            ItemActionRequirement itemActionRequirement = ReadItemActionRequirementDb(id);
            OnItemActionRequirementRead(itemActionRequirement);
        }

        public static void UpdateItemActionRequirement(int itemActionRequirementId, int itemActionRequirementItem, int itemActionRequirementAction)
        {
            UpdateItemActionRequirementDb(itemActionRequirementId, itemActionRequirementItem, itemActionRequirementAction);
            ItemActionRequirement itemActionRequirement = ReadItemActionRequirementDb(itemActionRequirementId);
            OnItemActionRequirementUpdated(itemActionRequirement);
        }

        public static void SelectItemActionRequirement(int itemActionRequirementId)
        {
            ItemActionRequirement itemActionRequirement = ReadItemActionRequirementDb(itemActionRequirementId);
            OnItemActionRequirementSelect(itemActionRequirement);
        }

        public static void ReadAllItemActionRequirementsForAction(int action)
        {
            List<ItemActionRequirement> itemActionRequirements = ReadAllItemActionRequirementsForActionDb(action);
            foreach (var itemActionRequirement in itemActionRequirements)
                OnItemActionRequirementRead(itemActionRequirement);
        }

        #endregion


        #region EventActionRequirements

        public static void CreateEventActionRequirement(int evntActionRequirementEvent, int evntActionRequirementAction)
        {
            int id = CreateEventActionRequirementDb(evntActionRequirementEvent, evntActionRequirementAction);
            EventActionRequirement evntActionRequirement = ReadEventActionRequirementDb(id);
            OnEventActionRequirementRead(evntActionRequirement);
        }

        public static void UpdateEventActionRequirement(int evntActionRequirementId, int evntActionRequirementEvent, int evntActionRequirementAction)
        {
            UpdateEventActionRequirementDb(evntActionRequirementId, evntActionRequirementEvent, evntActionRequirementAction);
            EventActionRequirement evntActionRequirement = ReadEventActionRequirementDb(evntActionRequirementId);
            OnEventActionRequirementUpdated(evntActionRequirement);
        }

        public static void SelectEventActionRequirement(int evntActionRequirementId)
        {
            EventActionRequirement evntActionRequirement = ReadEventActionRequirementDb(evntActionRequirementId);
            OnEventActionRequirementSelect(evntActionRequirement);
        }

        public static void ReadAllEventActionRequirementsForAction(int action)
        {
            List<EventActionRequirement> evntActionRequirements = ReadAllEventActionRequirementsForActionDb(action);
            foreach (var evntActionRequirement in evntActionRequirements)
                OnEventActionRequirementRead(evntActionRequirement);
        }

        #endregion


        #region CharacterActionRequirements

        public static void CreateCharacterActionRequirement(int characterActionRequirementCharacter, int characterActionRequirementAction)
        {
            int id = CreateCharacterActionRequirementDb(characterActionRequirementCharacter, characterActionRequirementAction);
            CharacterActionRequirement characterActionRequirement = ReadCharacterActionRequirementDb(id);
            OnCharacterActionRequirementRead(characterActionRequirement);
        }

        public static void UpdateCharacterActionRequirement(int characterActionRequirementId, int characterActionRequirementCharacter, int characterActionRequirementAction)
        {
            UpdateCharacterActionRequirementDb(characterActionRequirementId, characterActionRequirementCharacter, characterActionRequirementAction);
            CharacterActionRequirement characterActionRequirement = ReadCharacterActionRequirementDb(characterActionRequirementId);
            OnCharacterActionRequirementUpdated(characterActionRequirement);
        }

        public static void SelectCharacterActionRequirement(int characterActionRequirementId)
        {
            CharacterActionRequirement characterActionRequirement = ReadCharacterActionRequirementDb(characterActionRequirementId);
            OnCharacterActionRequirementSelect(characterActionRequirement);
        }

        public static void ReadAllCharacterActionRequirementsForAction(int action)
        {
            List<CharacterActionRequirement> characterActionRequirements = ReadAllCharacterActionRequirementsForActionDb(action);
            foreach (var characterActionRequirement in characterActionRequirements)
                OnCharacterActionRequirementRead(characterActionRequirement);
        }

        #endregion


        #region Messages

        public static void CreateMessage(string messageName, string messageText)
        {
            int id = CreateMessageDb(messageName, messageText);
            Message message = ReadMessageDb(id);
            OnMessageRead(message);
        }

        public static void UpdateMessage(int messageId, string messageName, string messageText)
        {
            UpdateMessageDb(messageId, messageName, messageText);
            Message message = ReadMessageDb(messageId);
            OnMessageUpdated(message);
        }

        public static void SelectMessage(int messageId)
        {
            Message message = ReadMessageDb(messageId);
            OnMessageSelect(message);
        }

        public static void ReadAllMessages()
        {
            List<Message> messages = ReadAllMessagesDb();
            foreach (var message in messages)
                OnMessageRead(message);
        }

        #endregion


        #region MessageChoiceChoices

        public static void CreateMessageChoice(string messageChoiceName, string messageChoiceText, int messageChoiceMessage)
        {
            int id = CreateMessageChoiceDb(messageChoiceName, messageChoiceText, messageChoiceMessage);
            MessageChoice messageChoice = ReadMessageChoiceDb(id);
            OnMessageChoiceRead(messageChoice);
        }

        public static void UpdateMessageChoice(int messageChoiceId, string messageChoiceName, string messageChoiceText, int messageChoiceMessage)
        {
            UpdateMessageChoiceDb(messageChoiceId, messageChoiceName, messageChoiceText, messageChoiceMessage);
            MessageChoice messageChoice = ReadMessageChoiceDb(messageChoiceId);
            OnMessageChoiceUpdated(messageChoice);
        }

        public static void SelectMessageChoice(int messageChoiceId)
        {
            MessageChoice messageChoice = ReadMessageChoiceDb(messageChoiceId);
            OnMessageChoiceSelect(messageChoice);
        }

        public static void ReadAllMessageChoicesForMessage(int messageId)
        {
            List<MessageChoice> messageChoices = ReadAllMessageChoicesForMessageDb(messageId);
            foreach (var messageChoice in messageChoices)
                OnMessageChoiceRead(messageChoice);
        }

        #endregion


        #region MessageChoiceResults

        public static void CreateMessageChoiceResult(int messageChoiceResultResult, int messageChoiceResultMessageChoice)
        {
            int id = CreateMessageChoiceResultDb(messageChoiceResultResult, messageChoiceResultMessageChoice);
            MessageChoiceResult messageChoiceResult = ReadMessageChoiceResultDb(id);
            OnMessageChoiceResultRead(messageChoiceResult);
        }

        public static void UpdateMessageChoiceResult(int messageChoiceResultId, int messageChoiceResultResult, int messageChoiceResultMessageChoice)
        {
            UpdateMessageChoiceResultDb(messageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice);
            MessageChoiceResult messageChoiceResult = ReadMessageChoiceResultDb(messageChoiceResultId);
            OnMessageChoiceResultUpdated(messageChoiceResult);
        }

        public static void SelectMessageChoiceResult(int messageChoiceResultId)
        {
            MessageChoiceResult messageChoiceResult = ReadMessageChoiceResultDb(messageChoiceResultId);
            OnMessageChoiceResultSelect(messageChoiceResult);
        }

        public static void ReadAllMessageChoiceResultsForMessageChoice(int messageChoiceId)
        {
            List<MessageChoiceResult> messageChoiceResults = ReadAllMessageChoiceResultsForMessageChoiceDb(messageChoiceId);
            foreach (var messageChoiceResult in messageChoiceResults)
                OnMessageChoiceResultRead(messageChoiceResult);
        }

        #endregion


        #region RoomPreviews

        public static void SelectRoomPreview(int room)
        {
            var roomPreview = ReadRoomPreviewDb(room);
            foreach (var paragraphState in roomPreview.Item1)
                OnRoomPreviewParagraphStateSelect(paragraphState);
        }

        #endregion


        #region AreaRoomOnInitialLoads

        public static void CreateAreaRoomOnInitialLoad(int areaId, int roomId)
        {
            UpsertAreaRoomOnInitialLoadDb(areaId, roomId);
            AreaRoomOnInitialLoad areaRoomOnInitialRead = ReadAreaRoomOnInitialLoadDb();
            OnAreaRoomOnInitialLoadRead(areaRoomOnInitialRead);
        }

        public static void UpdateAreaRoomOnInitialLoad(int areaId, int roomId)
        {
            UpsertAreaRoomOnInitialLoadDb(areaId, roomId);
            AreaRoomOnInitialLoad areaRoomOnInitialRead = ReadAreaRoomOnInitialLoadDb();
            OnAreaRoomOnInitialLoadUpdated(areaRoomOnInitialRead);
        }

        public static void ReadAreaRoomOnInitialLoad()
        {
            AreaRoomOnInitialLoad areaRoomOnInitialRead = ReadAreaRoomOnInitialLoadDb();
            OnAreaRoomOnInitialLoadRead(areaRoomOnInitialRead);
        }

        #endregion

        #endregion


        #region Private Functionality

        private static void InitializeSprocsToDbModelMap()
        {
            Mapper.CreateMap<dev_ReadArea_Result, Area>();
            Mapper.CreateMap<dev_ReadAllAreas_Result, Area>();

            Mapper.CreateMap<dev_ReadLocation_Result, Location>();
            Mapper.CreateMap<dev_ReadAllLocations_Result, Location>();

            Mapper.CreateMap<dev_ReadRoom_Result, Room>();
            Mapper.CreateMap<dev_ReadAllRoomsInArea_Result, Room>();
            Mapper.CreateMap<dev_ReadAllRoomsInAreaOnFloor_Result, Room>();

            Mapper.CreateMap<dev_ReadRoomState_Result, RoomState>();
            Mapper.CreateMap<dev_ReadAllRoomStatesForRoom_Result, RoomState>();

            Mapper.CreateMap<dev_ReadParagraph_Result, Paragraph>();
            Mapper.CreateMap<dev_ReadAllParagraphsForRoom_Result, Paragraph>();
            Mapper.CreateMap<dev_ReadAllParagraphsForRoomAndRoomState_Result, Paragraph>();

            Mapper.CreateMap<dev_ReadAllParagraphRoomStatesForParagraph_Result, ParagraphRoomState>();
            Mapper.CreateMap<dev_ReadAllParagraphRoomStatesForRoomState_Result, ParagraphRoomState>();
            Mapper.CreateMap<dev_ReadParagraphRoomState_Result, ParagraphRoomState>();

            Mapper.CreateMap<dev_ReadParagraphState_Result, ParagraphState>();
            Mapper.CreateMap<dev_ReadAllParagraphStatesForParagraph_Result, ParagraphState>();
            Mapper.CreateMap<dev_ReadParagraphStateForParagraphPreview_Result, ParagraphState>();

            Mapper.CreateMap<dev_ReadNoun_Result, Noun>();
            Mapper.CreateMap<dev_ReadAllNounsForParagraphState_Result, Noun>();

            Mapper.CreateMap<dev_ReadVerbType_Result, VerbType>();
            Mapper.CreateMap<dev_ReadAllVerbTypes_Result, VerbType>();

            Mapper.CreateMap<dev_ReadVerb_Result, Verb>();
            Mapper.CreateMap<dev_ReadAllVerbsForVerbType_Result, Verb>();

            Mapper.CreateMap<dev_ReadAction_Result, Db.Action>();
            Mapper.CreateMap<dev_ReadAllActionsForNoun_Result, Db.Action>();

            Mapper.CreateMap<dev_ReadResultType_Result, ResultType>();
            Mapper.CreateMap<dev_ReadAllResultTypes_Result, ResultType>();

            Mapper.CreateMap<dev_ReadJSONPropertyDataType_Result, JSONPropertyDataType>();
            Mapper.CreateMap<dev_ReadAllJSONPropertyDataTypes_Result, JSONPropertyDataType>();

            Mapper.CreateMap<dev_ReadResultTypeJSONProperty_Result, ResultTypeJSONProperty>();
            Mapper.CreateMap<dev_ReadAllResultTypeJSONPropertiesForResultType_Result, ResultTypeJSONProperty>();

            Mapper.CreateMap<dev_ReadResult_Result, Result>();
            Mapper.CreateMap<dev_ReadAllResultsForResultType_Result, Result>();
            Mapper.CreateMap<dev_ReadAllResultsForMessageChoiceResultType_Result, Result>();
            Mapper.CreateMap<dev_ReadAllResultsForActionResultType_Result, Result>();

            Mapper.CreateMap<dev_ReadActionResult_Result, ActionResult>();
            Mapper.CreateMap<dev_ReadAllActionResultsForAction_Result, ActionResult>();

            Mapper.CreateMap<dev_ReadItem_Result, Item>();
            Mapper.CreateMap<dev_ReadAllItems_Result, Item>();

            Mapper.CreateMap<dev_ReadEvent_Result, Event>();
            Mapper.CreateMap<dev_ReadAllEvents_Result, Event>();

            Mapper.CreateMap<dev_ReadCharacter_Result, Character>();
            Mapper.CreateMap<dev_ReadAllCharacters_Result, Character>();

            Mapper.CreateMap<dev_ReadAllItemActionRequirementsForAction_Result, ItemActionRequirement>();
            Mapper.CreateMap<dev_ReadItemActionRequirement_Result, ItemActionRequirement>();

            Mapper.CreateMap<dev_ReadAllEventActionRequirementsForAction_Result, EventActionRequirement>();
            Mapper.CreateMap<dev_ReadEventActionRequirement_Result, EventActionRequirement>();

            Mapper.CreateMap<dev_ReadAllCharacterActionRequirementsForAction_Result, CharacterActionRequirement>();
            Mapper.CreateMap<dev_ReadCharacterActionRequirement_Result, CharacterActionRequirement>();

            Mapper.CreateMap<dev_ReadMessage_Result, Message>();
            Mapper.CreateMap<dev_ReadAllMessages_Result, Message>();

            Mapper.CreateMap<dev_ReadMessageChoice_Result, MessageChoice>();
            Mapper.CreateMap<dev_ReadAllMessageChoicesForMessage_Result, MessageChoice>();

            Mapper.CreateMap<dev_ReadMessageChoiceResult_Result, MessageChoiceResult>();
            Mapper.CreateMap<dev_ReadAllMessageChoiceResultsForMessageChoice_Result, MessageChoiceResult>();

            Mapper.CreateMap<dev_ReadRoomPreviewNouns_Result, RoomPreviewNoun>();
            Mapper.CreateMap<dev_ReadRoomPreviewParagraphStates_Result, RoomPreviewParagraphState>();
            Mapper.CreateMap<RoomPreviewNoun[], RoomPreviewParagraphState>()
                .ForMember(dest => dest.Nouns, opt => opt.MapFrom(src => src));

            Mapper.CreateMap<dev_ReadAreaRoomOnInitialLoad_Result, AreaRoomOnInitialLoad>();
        }

        #region Areas

        private static int CreateAreaDb(string name)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateArea(name);
            }
            catch(Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateArea", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateArea", new Exception("No [Id] was returned after [Area] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateAreaDb(int id, string name)
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

        private static Area ReadAreaDb(int id)
        {
            ObjectResult<dev_ReadArea_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadArea(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadArea", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadArea", new Exception(string.Format("No [Areas] record found with [Id] = {0}.", id)));

            Area area = Mapper.Map<Area>(databaseResult.Single());
            return area;
        }

        private static List<Area> ReadAllAreasDb()
        {
            ObjectResult<dev_ReadAllAreas_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllAreas();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllAreas", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllAreas", new Exception("No [Areas] records found."));

            List<Area> areas = databaseResult.Select(r => Mapper.Map<Area>(r)).ToList();
            return areas;
        }

        #endregion


        #region Locations

        private static int CreateLocationDb(string name, string locationFile)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateLocation(name, locationFile);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateLocation", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateLocation", new Exception("No [Id] was returned after [Location] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateLocationDb(int id, string name, string locationFile)
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

        private static Location ReadLocationDb(int id)
        {
            ObjectResult<dev_ReadLocation_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadLocation(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadLocation", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadLocation", new Exception(string.Format("No [Locations] record found with [Id] = {0}.", id)));

            Location location = Mapper.Map<Location>(databaseResult.Single());
            return location;
        }

        private static List<Location> ReadAllLocationsDb()
        {
            ObjectResult<dev_ReadAllLocations_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllLocations();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllLocations", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllLocations", new Exception("No [Locations] records found."));

            List<Location> locations = databaseResult.Select(r => Mapper.Map<Location>(r)).ToList();
            return locations;
        }

        #endregion


        #region Rooms

        private static Tuple<int, int> CreateRoomDb(string name, int x, int y, int z, int area)
        {
            ObjectResult<decimal?> databaseResult = null;
            int roomId, roomStateId;
            try
            {
                databaseResult = m_entities.dev_CreateRoom(name, x, y, z, area);
                var result = databaseResult.FirstOrDefault();
                if (!result.HasValue)
                    throw new GinTubDatabaseException("dev_CreateRoom", new Exception("No [Id] was returned after [Room] INSERT."));

                roomId = (int)result.Value;

                databaseResult = databaseResult.GetNextResult<decimal?>();
                result = databaseResult.FirstOrDefault();
                if (!result.HasValue)
                    throw new GinTubDatabaseException("dev_CreateRoom", new Exception("No [Id] was returned for automatic [RoomState] INSERT after [Room] INSERT."));

                roomStateId = (int)result.Value;
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateRoom", e);
            }

            return new Tuple<int, int>(roomId, roomStateId);
        }

        private static void UpdateRoomDb(int id, string name, int x, int y, int z, int area)
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

        private static Room ReadRoomDb(int id)
        {
            ObjectResult<dev_ReadRoom_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadRoom(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadRoom", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadRoom", new Exception(string.Format("No [Rooms] record found with [Id] = {0}.", id)));

            Room room = Mapper.Map<Room>(databaseResult.Single());
            return room;
        }

        private static List<Room> ReadAllRoomsInAreaDb(int area)
        {
            ObjectResult<dev_ReadAllRoomsInArea_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllRoomsInArea(area);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllRoomsInArea", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllRoomsInArea", new Exception("No [Rooms] records found."));

            List<Room> rooms = databaseResult.Select(r => Mapper.Map<Room>(r)).ToList();
            return rooms;
        }

        private static List<Room> ReadAllRoomsInAreaOnFloorDb(int area, int z)
        {
            ObjectResult<dev_ReadAllRoomsInAreaOnFloor_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllRoomsInAreaOnFloor(area, z);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllRoomsInAreaOnFloor", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllRoomsInAreaOnFloor", new Exception("No [Rooms] records found."));

            List<Room> rooms = databaseResult.Select(r => Mapper.Map<Room>(r)).ToList();
            return rooms;
        }

        #endregion


        #region RoomStates

        private static int CreateRoomStateDb(TimeSpan time, int location, int room)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateRoomState(time, location, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateRoomState", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateRoomState", new Exception("No [Id] was returned after [RoomState] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateRoomStateDb(int id, int state, TimeSpan time, int location, int room)
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

        private static RoomState ReadRoomStateDb(int id)
        {
            ObjectResult<dev_ReadRoomState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadRoomState(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadRoomState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadRoomState", new Exception(string.Format("No [RoomStates] record found with [Id] = {0}.", id)));

            RoomState roomState = Mapper.Map<RoomState>(databaseResult.Single());
            return roomState;
        }

        private static List<RoomState> ReadAllRoomStatesForRoomDb(int room)
        {
            ObjectResult<dev_ReadAllRoomStatesForRoom_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllRoomStatesForRoom(room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllRoomStatesForRoom", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllRoomStatesForRoom", new Exception("No [RoomStates] records found."));

            List<RoomState> roomStates = databaseResult.Select(r => Mapper.Map<RoomState>(r)).ToList();
            return roomStates;
        }

        #endregion


        #region Paragraphs

        private static Tuple<int, int> CreateParagraphDb(int order, int room)
        {
            ObjectResult<decimal?> databaseResult = null;
            int paragraphId, paragraphStateId;
            try
            {
                databaseResult = m_entities.dev_CreateParagraph(order, room);
                var result = databaseResult.FirstOrDefault();
                if (!result.HasValue)
                    throw new GinTubDatabaseException("dev_CreateParagraph", new Exception("No [Id] was returned after [Paragraph] INSERT."));

                paragraphId = (int)result.Value;

                databaseResult = databaseResult.GetNextResult<decimal?>();
                result = databaseResult.FirstOrDefault();
                if (!result.HasValue)
                    throw new GinTubDatabaseException("dev_CreateParagraph", new Exception("No [Id] was returned after automatic [ParagraphState] INSERT after [Paragraph] INSERT."));

                paragraphStateId = (int)result.Value;
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateParagraph", e);
            }

            return new Tuple<int, int>(paragraphId, paragraphStateId);
        }

        private static void UpdateParagraphDb(int id, int order, int room)
        {
            try
            {
                m_entities.dev_UpdateParagraph(id, order, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateParagraph", e);
            }
        }

        private static Paragraph ReadParagraphDb(int id)
        {
            ObjectResult<dev_ReadParagraph_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadParagraph(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadParagraph", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadParagraph", new Exception(string.Format("No [Paragraphs] record found with [Id] = {0}.", id)));

            Paragraph paragraph = Mapper.Map<Paragraph>(databaseResult.Single());
            return paragraph;
        }

        private static List<Paragraph> ReadAllParagraphsForRoomDb(int room)
        {
            ObjectResult<dev_ReadAllParagraphsForRoom_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllParagraphsForRoom(room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllParagraphsForRoom", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllParagraphsForRoom", new Exception("No [Paragraphs] records found."));

            List<Paragraph> paragraphs = databaseResult.Select(r => Mapper.Map<Paragraph>(r)).ToList();
            return paragraphs;
        }

        private static List<Paragraph> ReadAllParagraphsForRoomAndRoomStateDb(int room, int? roomState)
        {
            ObjectResult<dev_ReadAllParagraphsForRoomAndRoomState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllParagraphsForRoomAndRoomState(room, roomState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllParagraphsForRoomAndRoomState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllParagraphsForRoomAndRoomState", new Exception("No [Paragraphs] records found."));

            List<Paragraph> paragraphs = databaseResult.Select(r => Mapper.Map<Paragraph>(r)).ToList();
            return paragraphs;
        }

        #endregion


        #region ParagraphStates

        private static int CreateParagraphStateDb(string text, int paragraph)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateParagraphState(text, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateParagraphState", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateParagraphState", new Exception("No [Id] was returned after [ParagraphState] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateParagraphStateDb(int id, string text, int state, int paragraph)
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

        private static ParagraphState ReadParagraphStateDb(int id)
        {
            ObjectResult<dev_ReadParagraphState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadParagraphState(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadParagraphState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadParagraphState", new Exception(string.Format("No [ParagraphStates] record found with [Id] = {0}.", id)));

            ParagraphState paragraphState = Mapper.Map<ParagraphState>(databaseResult.Single());
            return paragraphState;
        }

        private static List<ParagraphState> ReadAllParagraphStatesForParagraphDb(int paragraph)
        {
            ObjectResult<dev_ReadAllParagraphStatesForParagraph_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllParagraphStatesForParagraph(paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllParagraphStatesForParagraph", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllParagraphStatesForParagraph", new Exception("No [ParagraphStates] records found."));

            List<ParagraphState> paragraphStates = databaseResult.Select(r => Mapper.Map<ParagraphState>(r)).ToList();
            return paragraphStates;
        }

        private static ParagraphState ReadParagraphStateForParagraphPreviewDb(int state, int paragraph)
        {
            ObjectResult<dev_ReadParagraphStateForParagraphPreview_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadParagraphStateForParagraphPreview(state, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadParagraphStateForParagraphPreview", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadParagraphStateForParagraphPreview", new Exception(string.Format("No [ParagraphStates] record found with [State] = {0} for Paragraph with [Id] = {1}.", state, paragraph)));

            ParagraphState paragraphState = Mapper.Map<ParagraphState>(databaseResult.SingleOrDefault());
            return paragraphState;
        }

        #endregion


        #region ParagraphRoomStates

        private static int CreateParagraphRoomStateDb(int roomState, int paragraph)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateParagraphRoomState(roomState, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateParagraphRoomState", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateParagraphRoomState", new Exception("No [Id] was returned after [ParagraphRoomState] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateParagraphRoomStateDb(int id, int roomState, int paragraph)
        {
            try
            {
                m_entities.dev_UpdateParagraphRoomState(id, roomState, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_UpdateParagraphRoomState", e);
            }
        }

        private static ParagraphRoomState ReadParagraphRoomStateDb(int id)
        {
            ObjectResult<dev_ReadParagraphRoomState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadParagraphRoomState(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadParagraphRoomState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadParagraphRoomState", new Exception(string.Format("No [ParagraphRoomStates] record found with [Id] = {0}.", id)));

            ParagraphRoomState paragraphRoomState = Mapper.Map<ParagraphRoomState>(databaseResult.Single());
            return paragraphRoomState;
        }

        private static List<ParagraphRoomState> ReadAllParagraphRoomStatesForParagraphDb(int paragraph)
        {
            ObjectResult<dev_ReadAllParagraphRoomStatesForParagraph_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllParagraphRoomStatesForParagraph(paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllParagraphRoomStatesForParagraph", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllParagraphRoomStatesForParagraph", new Exception("No [ParagraphRoomStates] records found."));

            List<ParagraphRoomState> paragraphRoomStates = databaseResult.Select(r => Mapper.Map<ParagraphRoomState>(r)).ToList();
            return paragraphRoomStates;
        }

        private static void DeleteAllParagraphRoomStatesForParagraphDb(int paragraph)
        {
            try
            {
                m_entities.dev_DeleteAllParagraphRoomStatesForParagraph(paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateParagraphRoomState", e);
            }
        }

        #endregion


        #region Nouns

        private static int CreateNounDb(string text, int paragraphState)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateNoun(text, paragraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateNoun", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateNoun", new Exception("No [Id] was returned after [Noun] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateNounDb(int id, string text, int paragraphState)
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

        private static Noun ReadNounDb(int id)
        {
            ObjectResult<dev_ReadNoun_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadNoun(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadNoun", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadNoun", new Exception(string.Format("No [Nouns] record found with [Id] = {0}.", id)));

            Noun noun = Mapper.Map<Noun>(databaseResult.Single());
            return noun;
        }

        private static List<Noun> ReadAllNounsForParagraphStateDb(int paragraphState)
        {
            ObjectResult<dev_ReadAllNounsForParagraphState_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllNounsForParagraphState(paragraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllNounsForParagraphState", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllNounsForParagraphState", new Exception("No [Nouns] records found."));

            List<Noun> nouns = databaseResult.Select(r => Mapper.Map<Noun>(r)).ToList();
            return nouns;
        }

        #endregion


        #region VerbTypes

        private static int CreateVerbTypeDb(string name)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateVerbType(name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateVerbType", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateVerbType", new Exception("No [Id] was returned after [VerbType] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateVerbTypeDb(int id, string name)
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

        private static VerbType ReadVerbTypeDb(int id)
        {
            ObjectResult<dev_ReadVerbType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadVerbType(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadVerbType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadVerbType", new Exception(string.Format("No [VerbTypes] record found with [Id] = {0}.", id)));

            VerbType verbType = Mapper.Map<VerbType>(databaseResult.Single());
            return verbType;
        }

        private static List<VerbType> ReadAllVerbTypesDb()
        {
            ObjectResult<dev_ReadAllVerbTypes_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllVerbTypes();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllVerbTypes", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllVerbTypes", new Exception("No [VerbTypes] records found."));

            List<VerbType> verbTypes = databaseResult.Select(r => Mapper.Map<VerbType>(r)).ToList();
            return verbTypes;
        }

        #endregion


        #region Verbs

        private static int CreateVerbDb(string name, int verbType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateVerb(name, verbType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateVerb", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateVerb", new Exception("No [Id] was returned after [Verb] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateVerbDb(int id, string name, int verbType)
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

        private static Verb ReadVerbDb(int id)
        {
            ObjectResult<dev_ReadVerb_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadVerb(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadVerb", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadVerb", new Exception(string.Format("No [Verbs] record found with [Id] = {0}.", id)));

            Verb verb = Mapper.Map<Verb>(databaseResult.Single());
            return verb;
        }

        private static List<Verb> ReadAllVerbsForVerbTypeDb(int verbType)
        {
            ObjectResult<dev_ReadAllVerbsForVerbType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllVerbsForVerbType(verbType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllVerbsForVerbType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllVerbsForVerbType", new Exception("No [Verbs] records found."));

            List<Verb> verbs = databaseResult.Select(r => Mapper.Map<Verb>(r)).ToList();
            return verbs;
        }

        #endregion


        #region Actions

        private static int CreateActionDb(int verbType, int noun)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateAction(verbType, noun);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateAction", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateAction", new Exception("No [Id] was returned after [Action] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateActionDb(int id, int verbType, int noun)
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

        private static Db.Action ReadActionDb(int id)
        {
            ObjectResult<dev_ReadAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAction(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAction", new Exception(string.Format("No [Actions] record found with [Id] = {0}.", id)));

            Db.Action action = Mapper.Map<Db.Action>(databaseResult.Single());
            return action;
        }

        private static List<Db.Action> ReadAllActionsForNounDb(int noun)
        {
            ObjectResult<dev_ReadAllActionsForNoun_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllActionsForNoun(noun);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllActionsForNoun", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllActionsForNoun", new Exception("No [Actions] records found."));

            List<Db.Action> actions = databaseResult.Select(r => Mapper.Map<Db.Action>(r)).ToList();
            return actions;
        }

        #endregion


        #region ResultTypes

        private static int CreateResultTypeDb(string name)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateResultType(name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateResultType", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateResultType", new Exception("No [Id] was returned after [ResultType] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateResultTypeDb(int id, string name)
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

        private static ResultType ReadResultTypeDb(int id)
        {
            ObjectResult<dev_ReadResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadResultType(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadResultType", new Exception(string.Format("No [ResultTypes] record found with [Id] = {0}.", id)));

            ResultType resultType = Mapper.Map<ResultType>(databaseResult.Single());
            return resultType;
        }

        private static List<ResultType> ReadAllResultTypesDb()
        {
            ObjectResult<dev_ReadAllResultTypes_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllResultTypes();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllResultTypes", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllResultTypes", new Exception("No [ResultTypes] records found."));

            List<ResultType> resultTypes = databaseResult.Select(r => Mapper.Map<ResultType>(r)).ToList();
            return resultTypes;
        }

        #endregion


        #region JSONPropertyDataTypes

        private static int CreateJSONPropertyDataTypeDb(string dataType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateJSONPropertyDataType(dataType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateJSONPropertyDataType", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateJSONPropertyDataType", new Exception("No [Id] was returned after [JSONPropertyDataType] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateJSONPropertyDataTypeDb(int id, string dataType)
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

        private static JSONPropertyDataType ReadJSONPropertyDataTypeDb(int id)
        {
            ObjectResult<dev_ReadJSONPropertyDataType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadJSONPropertyDataType(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadJSONPropertyDataType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadJSONPropertyDataType", new Exception(string.Format("No [JSONPropertyDataTypes] record found with [Id] = {0}.", id)));

            JSONPropertyDataType jsonPropertyDataType = Mapper.Map<JSONPropertyDataType>(databaseResult.Single());
            return jsonPropertyDataType;
        }

        private static List<JSONPropertyDataType> ReadAllJSONPropertyDataTypesDb()
        {
            ObjectResult<dev_ReadAllJSONPropertyDataTypes_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllJSONPropertyDataTypes();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllJSONPropertyDataTypes", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllJSONPropertyDataTypes", new Exception("No [JSONPropertyDataTypes] records found."));

            List<JSONPropertyDataType> jsonPropertyDataTypes = databaseResult.Select(r => Mapper.Map<JSONPropertyDataType>(r)).ToList();
            return jsonPropertyDataTypes;
        }

        #endregion


        #region ResultTypeJSONProperties

        private static int CreateResultTypeJSONPropertyDb(string jsonProperty, int dataType, int resultType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateResultTypeJSONProperty(jsonProperty, dataType, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateResultTypeJSONProperty", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateResultTypeJSONProperty", new Exception("No [Id] was returned after [ResultTypeJSONProperty] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateResultTypeJSONPropertyDb(int id, string jsonProperty, int dataType, int resultType)
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

        private static ResultTypeJSONProperty ReadResultTypeJSONPropertyDb(int id)
        {
            ObjectResult<dev_ReadResultTypeJSONProperty_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadResultTypeJSONProperty(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadResultTypeJSONProperty", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadResultTypeJSONProperty", new Exception(string.Format("No [ResultTypeJSONProperties] record found with [Id] = {0}.", id)));

            ResultTypeJSONProperty resultTypeJSONProperty = Mapper.Map<ResultTypeJSONProperty>(databaseResult.Single());
            return resultTypeJSONProperty;
        }

        private static List<ResultTypeJSONProperty> ReadAllResultTypeJSONPropertiesForResultTypeDb(int resultType)
        {
            ObjectResult<dev_ReadAllResultTypeJSONPropertiesForResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllResultTypeJSONPropertiesForResultType(resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllResultTypeJSONPropertiesForResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllResultTypeJSONPropertiesForResultType", new Exception("No [ResultTypeJSONProperties] records found."));

            List<ResultTypeJSONProperty> resultTypeJSONProperties = databaseResult.Select(r => Mapper.Map<ResultTypeJSONProperty>(r)).ToList();
            return resultTypeJSONProperties;
        }

        #endregion


        #region Results

        private static int CreateResultDb(string name, string jsonData, int resultType)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateResult(name, jsonData, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateResult", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateResult", new Exception("No [Id] was returned after [Result] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateResultDb(int id, string name, string jsonData, int resultType)
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

        private static Result ReadResultDb(int id)
        {
            ObjectResult<dev_ReadResult_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadResult(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadResult", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadResult", new Exception(string.Format("No [Results] record found with [Id] = {0}.", id)));

            Result result = Mapper.Map<Result>(databaseResult.Single());
            return result;
        }

        private static List<Result> ReadAllResultsForResultTypeDb(int resultType)
        {
            ObjectResult<dev_ReadAllResultsForResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllResultsForResultType(resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllResultsForResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllResultsForResultType", new Exception("No [Results] records found."));

            List<Result> results = databaseResult.Select(r => Mapper.Map<Result>(r)).ToList();
            return results;
        }

        private static List<Result> ReadAllResultsForActionResultTypeDb(int action)
        {
            ObjectResult<dev_ReadAllResultsForActionResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllResultsForActionResultType(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllResultsForActionResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllResultsForActionResultType", new Exception("No [Results] records found."));

            List<Result> results = databaseResult.Select(r => Mapper.Map<Result>(r)).ToList();
            return results;
        }

        private static List<Result> ReadAllResultsForMessageChoiceResultTypeDb(int messageChoice)
        {
            ObjectResult<dev_ReadAllResultsForMessageChoiceResultType_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllResultsForMessageChoiceResultType(messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllResultsForMessageChoiceResultType", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllResultsForMessageChoiceResultType", new Exception("No [Results] records found."));

            List<Result> results = databaseResult.Select(r => Mapper.Map<Result>(r)).ToList();
            return results;
        }

        #endregion


        #region ActionResults

        private static int CreateActionResultDb(int result, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateActionResult(result, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateActionResult", e);
            }
            var reslt = databaseResult.FirstOrDefault();
            if (!reslt.HasValue)
                throw new GinTubDatabaseException("dev_CreateActionResult", new Exception("No [Id] was returned after [ActionResult] INSERT."));

            return (int)reslt.Value;
        }

        private static void UpdateActionResultDb(int id, int result, int action)
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

        private static ActionResult ReadActionResultDb(int id)
        {
            ObjectResult<dev_ReadActionResult_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadActionResult(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadActionResult", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadActionResult", new Exception(string.Format("No [ActionResults] record found with [Id] = {0}.", id)));

            ActionResult actionResult = Mapper.Map<ActionResult>(databaseResult.Single());
            return actionResult;
        }

        private static List<ActionResult> ReadAllActionResultsForActionDb(int action)
        {
            ObjectResult<dev_ReadAllActionResultsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllActionResultsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllActionResultsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllActionResultsForAction", new Exception("No [ActionResults] records found."));

            List<ActionResult> actionResults = databaseResult.Select(r => Mapper.Map<ActionResult>(r)).ToList();
            return actionResults;
        }

        #endregion


        #region Items

        private static int CreateItemDb(string name, string description)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateItem(name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateItem", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateItem", new Exception("No [Id] was returned after [Item] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateItemDb(int id, string name, string description)
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

        private static Item ReadItemDb(int id)
        {
            ObjectResult<dev_ReadItem_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadItem(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadItem", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadItem", new Exception(string.Format("No [Items] record found with [Id] = {0}.", id)));

            Item item = Mapper.Map<Item>(databaseResult.Single());
            return item;
        }

        private static List<Item> ReadAllItemsDb()
        {
            ObjectResult<dev_ReadAllItems_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllItems();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllItems", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllItems", new Exception("No [Items] records found."));

            List<Item> items = databaseResult.Select(r => Mapper.Map<Item>(r)).ToList();
            return items;
        }

        #endregion


        #region Events

        private static int CreateEventDb(string name, string description)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateEvent(name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateEvent", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateEvent", new Exception("No [Id] was returned after [Event] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateEventDb(int id, string name, string description)
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

        private static Event ReadEventDb(int id)
        {
            ObjectResult<dev_ReadEvent_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadEvent(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadEvent", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadEvent", new Exception(string.Format("No [Events] record found with [Id] = {0}.", id)));

            Event evnt = Mapper.Map<Event>(databaseResult.Single());
            return evnt;
        }

        private static List<Event> ReadAllEventsDb()
        {
            ObjectResult<dev_ReadAllEvents_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllEvents();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllEvents", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllEvents", new Exception("No [Events] records found."));

            List<Event> evnts = databaseResult.Select(r => Mapper.Map<Event>(r)).ToList();
            return evnts;
        }

        #endregion


        #region Characters

        private static int CreateCharacterDb(string name, string description)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateCharacter(name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateCharacter", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateCharacter", new Exception("No [Id] was returned after [Character] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateCharacterDb(int id, string name, string description)
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

        private static Character ReadCharacterDb(int id)
        {
            ObjectResult<dev_ReadCharacter_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadCharacter(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadCharacter", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadCharacter", new Exception(string.Format("No [Characters] record found with [Id] = {0}.", id)));

            Character character = Mapper.Map<Character>(databaseResult.Single());
            return character;
        }

        private static List<Character> ReadAllCharactersDb()
        {
            ObjectResult<dev_ReadAllCharacters_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllCharacters();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllCharacters", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllCharacters", new Exception("No [Characters] records found."));

            List<Character> characters = databaseResult.Select(r => Mapper.Map<Character>(r)).ToList();
            return characters;
        }

        #endregion


        #region ItemActionRequirements

        private static int CreateItemActionRequirementDb(int item, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateItemActionRequirement(item, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateItemActionRequirement", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateItemActionRequirement", new Exception("No [Id] was returned after [ItemActionRequirement] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateItemActionRequirementDb(int id, int item, int action)
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

        private static ItemActionRequirement ReadItemActionRequirementDb(int id)
        {
            ObjectResult<dev_ReadItemActionRequirement_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadItemActionRequirement(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadItemActionRequirement", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadItemActionRequirement", new Exception(string.Format("No [ItemActionRequirements] record found with [Id] = {0}.", id)));

            ItemActionRequirement itemActionRequirement = Mapper.Map<ItemActionRequirement>(databaseResult.Single());
            return itemActionRequirement;
        }

        private static List<ItemActionRequirement> ReadAllItemActionRequirementsForActionDb(int action)
        {
            ObjectResult<dev_ReadAllItemActionRequirementsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllItemActionRequirementsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllItemActionRequirementsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllItemActionRequirementsForAction", new Exception("No [ItemActionRequirements] records found."));

            List<ItemActionRequirement> itemActionRequirements = databaseResult.Select(r => Mapper.Map<ItemActionRequirement>(r)).ToList();
            return itemActionRequirements;
        }

        #endregion


        #region EventActionRequirements

        private static int CreateEventActionRequirementDb(int evnt, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateEventActionRequirement(evnt, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateEventActionRequirement", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateEventActionRequirement", new Exception("No [Id] was returned after [EventActionRequirement] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateEventActionRequirementDb(int id, int evnt, int action)
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

        private static EventActionRequirement ReadEventActionRequirementDb(int id)
        {
            ObjectResult<dev_ReadEventActionRequirement_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadEventActionRequirement(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadEventActionRequirement", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadEventActionRequirement", new Exception(string.Format("No [EventActionRequirements] record found with [Id] = {0}.", id)));

            EventActionRequirement evntActionRequirement = Mapper.Map<EventActionRequirement>(databaseResult.Single());
            return evntActionRequirement;
        }

        private static List<EventActionRequirement> ReadAllEventActionRequirementsForActionDb(int action)
        {
            ObjectResult<dev_ReadAllEventActionRequirementsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllEventActionRequirementsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllEventActionRequirementsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllEventActionRequirementsForAction", new Exception("No [EventActionRequirements] records found."));

            List<EventActionRequirement> evntActionRequirements = databaseResult.Select(r => Mapper.Map<EventActionRequirement>(r)).ToList();
            return evntActionRequirements;
        }

        #endregion


        #region CharacterActionRequirements

        private static int CreateCharacterActionRequirementDb(int character, int action)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateCharacterActionRequirement(character, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateCharacterActionRequirement", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateCharacterActionRequirement", new Exception("No [Id] was returned after [CharacterActionRequirement] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateCharacterActionRequirementDb(int id, int character, int action)
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

        private static CharacterActionRequirement ReadCharacterActionRequirementDb(int id)
        {
            ObjectResult<dev_ReadCharacterActionRequirement_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadCharacterActionRequirement(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadCharacterActionRequirement", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadCharacterActionRequirement", new Exception(string.Format("No [CharacterActionRequirements] record found with [Id] = {0}.", id)));

            CharacterActionRequirement characterActionRequirement = Mapper.Map<CharacterActionRequirement>(databaseResult.Single());
            return characterActionRequirement;
        }

        private static List<CharacterActionRequirement> ReadAllCharacterActionRequirementsForActionDb(int action)
        {
            ObjectResult<dev_ReadAllCharacterActionRequirementsForAction_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllCharacterActionRequirementsForAction(action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllCharacterActionRequirementsForAction", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllCharacterActionRequirementsForAction", new Exception("No [CharacterActionRequirements] records found."));

            List<CharacterActionRequirement> characterActionRequirements = databaseResult.Select(r => Mapper.Map<CharacterActionRequirement>(r)).ToList();
            return characterActionRequirements;
        }

        #endregion


        #region Messages

        private static int CreateMessageDb(string name, string text)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateMessage(name, text);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateMessage", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateMessage", new Exception("No [Id] was returned after [Message] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateMessageDb(int id, string name, string text)
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

        private static Message ReadMessageDb(int id)
        {
            ObjectResult<dev_ReadMessage_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadMessage(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadMessage", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadMessage", new Exception(string.Format("No [Messages] record found with [Id] = {0}.", id)));

            Message message = Mapper.Map<Message>(databaseResult.Single());
            return message;
        }

        private static List<Message> ReadAllMessagesDb()
        {
            ObjectResult<dev_ReadAllMessages_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllMessages();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllMessages", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllMessages", new Exception("No [Messages] records found."));

            List<Message> messages = databaseResult.Select(r => Mapper.Map<Message>(r)).ToList();
            return messages;
        }

        #endregion


        #region MessageChoices

        private static int CreateMessageChoiceDb(string name, string text, int message)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateMessageChoice(name, text, message);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateMessageChoice", e);
            }
            var result = databaseResult.FirstOrDefault();
            if (!result.HasValue)
                throw new GinTubDatabaseException("dev_CreateMessageChoice", new Exception("No [Id] was returned after [MessageChoice] INSERT."));

            return (int)result.Value;
        }

        private static void UpdateMessageChoiceDb(int id, string name, string text, int message)
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

        private static MessageChoice ReadMessageChoiceDb(int id)
        {
            ObjectResult<dev_ReadMessageChoice_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadMessageChoice(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadMessageChoice", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadMessageChoice", new Exception(string.Format("No [MessageChoices] record found with [Id] = {0}.", id)));

            MessageChoice messageChoice = Mapper.Map<MessageChoice>(databaseResult.Single());
            return messageChoice;
        }

        private static List<MessageChoice> ReadAllMessageChoicesForMessageDb(int message)
        {
            ObjectResult<dev_ReadAllMessageChoicesForMessage_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllMessageChoicesForMessage(message);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllMessageChoicesForMessage", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllMessageChoicesForMessage", new Exception("No [MessageChoices] records found."));

            List<MessageChoice> messageChoices = databaseResult.Select(r => Mapper.Map<MessageChoice>(r)).ToList();
            return messageChoices;
        }

        #endregion


        #region MessageChoiceResults

        private static int CreateMessageChoiceResultDb(int result, int messageChoice)
        {
            ObjectResult<decimal?> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_CreateMessageChoiceResult(result, messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_CreateMessageChoiceResult", e);
            }
            var reslt = databaseResult.FirstOrDefault();
            if (!reslt.HasValue)
                throw new GinTubDatabaseException("dev_CreateMessageChoiceResult", new Exception("No [Id] was returned after [MessageChoiceResult] INSERT."));

            return (int)reslt.Value;
        }

        private static void UpdateMessageChoiceResultDb(int id, int result, int messageChoice)
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

        private static MessageChoiceResult ReadMessageChoiceResultDb(int id)
        {
            ObjectResult<dev_ReadMessageChoiceResult_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadMessageChoiceResult(id);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadMessageChoiceResult", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadMessageChoiceResult", new Exception(string.Format("No [MessageChoiceResults] record found with [Id] = {0}.", id)));

            MessageChoiceResult messageChoiceResult = Mapper.Map<MessageChoiceResult>(databaseResult.Single());
            return messageChoiceResult;
        }

        private static List<MessageChoiceResult> ReadAllMessageChoiceResultsForMessageChoiceDb(int messageChoice)
        {
            ObjectResult<dev_ReadAllMessageChoiceResultsForMessageChoice_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAllMessageChoiceResultsForMessageChoice(messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAllMessageChoiceResultsForMessageChoice", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAllMessageChoiceResultsForMessageChoice", new Exception("No [MessageChoiceResults] records found."));

            List<MessageChoiceResult> messageChoiceResults = databaseResult.Select(r => Mapper.Map<MessageChoiceResult>(r)).ToList();
            return messageChoiceResults;
        }

        #endregion


        #region RoomPreviews

        private static Tuple<List<RoomPreviewParagraphState>> ReadRoomPreviewDb(int room)
        {
            List<RoomPreviewParagraphState> roomPreviewParagraphStates = new List<RoomPreviewParagraphState>();
            List<RoomPreviewNoun> roomPreviewNouns = new List<RoomPreviewNoun>();
            try
            {
                var paragraphStates = m_entities.dev_ReadRoomPreview(room);
                roomPreviewParagraphStates.AddRange(paragraphStates.Select(r => Mapper.Map<RoomPreviewParagraphState>(r)));

                var noun = paragraphStates.GetNextResult<dev_ReadRoomPreviewNouns_Result>();
                roomPreviewNouns.AddRange(noun.Select(r => Mapper.Map<RoomPreviewNoun>(r)));
                foreach (var roomPreviewParagraphState in roomPreviewParagraphStates)
                    Mapper.Map(roomPreviewNouns.Where(n => n.ParagraphState == roomPreviewParagraphState.Id).ToArray(), roomPreviewParagraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadRoomPreview", e);
            }

            return new Tuple<List<RoomPreviewParagraphState>>(roomPreviewParagraphStates);
        }

        #endregion


        #region AreaRoomOnInitialLoads

        private static void UpsertAreaRoomOnInitialLoadDb(int area, int room)
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

        private static AreaRoomOnInitialLoad ReadAreaRoomOnInitialLoadDb()
        {
            ObjectResult<dev_ReadAreaRoomOnInitialLoad_Result> databaseResult = null;
            try
            {
                databaseResult = m_entities.dev_ReadAreaRoomOnInitialLoad();
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ReadAreaRoomOnInitialLoad", e);
            }
            if (databaseResult == null)
                throw new GinTubDatabaseException("dev_ReadAreaRoomOnInitialLoad", new Exception("No [AreaRoomOnInitialLoads] record found."));

            AreaRoomOnInitialLoad areaRoomOnInitialRead = Mapper.Map<AreaRoomOnInitialLoad>(databaseResult.Single());
            return areaRoomOnInitialRead;
        }

        #endregion

        #endregion

        #endregion

    }
}
