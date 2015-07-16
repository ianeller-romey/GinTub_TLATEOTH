using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AutoMapper;

using TBGINTB_Builder.Lib.Exceptions;
using TBGINTB_Builder.Lib.Repository;

using Db = TBGINTB_Builder.Lib.Model.DbModel;
using Xml = TBGINTB_Builder.Lib.Model.XmlModel;


namespace TBGINTB_Builder.Lib
{
    public static partial class GinTubBuilderManager
    {

        #region MEMBER METHODS

        #region Public Functionality

        public static void ExportToXml(string fileName)
        {
            Xml.GinTub ginTub = ExportGinTubToXml();
            ginTub.ExportDate = DateTime.Now;

            XmlSerializer serializer = new XmlSerializer(typeof(Xml.GinTub));
            using(TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, ginTub);
            }
        }

        public static void ImportFromXml(string fileName, string backupFile)
        {
            if (backupFile != null)
            {
                if (backupFile.Any(c => Path.GetInvalidPathChars().Contains(c)) ||
                    backupFile.Any(c => char.IsWhiteSpace(c)) ||
                    backupFile.Contains('\'') ||
                    backupFile == string.Empty)
                    throw new ArgumentException("Incorrectly formatted or potentially dangerous file name provided.", "backupFile");
            }
            Xml.GinTub ginTub;

            XmlSerializer serializer = new XmlSerializer(typeof(Xml.GinTub));
            using(TextReader textReader = new StreamReader(fileName))
            {
                ginTub = serializer.Deserialize(textReader) as Xml.GinTub;
            }
            if (ginTub == null)
                throw new GinTubXmlException("Deserialize");

            try
            {
                m_entities.Database.ExecuteSqlCommand
                (
                    TransactionalBehavior.DoNotEnsureTransaction, 
                    "EXEC [dev].[dev_ClearDatabase] @backupfile = N'" + backupFile + "'"
                );
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ClearDatabase", e);
            }

            ImportGinTubFromXml(ginTub);
        }

        #endregion


        #region Private Functionality

        private static void InitializeDbModelToXmlModelMap()
        {
            Mapper.CreateMap<Db.Area, Xml.Area>();

            Mapper.CreateMap<Db.Location, Xml.Location>();

            Mapper.CreateMap<Db.Room, Xml.Room>();

            Mapper.CreateMap<Db.RoomState, Xml.RoomState>();

            Mapper.CreateMap<Db.Paragraph, Xml.Paragraph>();

            Mapper.CreateMap<Db.ParagraphState, Xml.ParagraphState>();

            Mapper.CreateMap<Db.Noun, Xml.Noun>();

            Mapper.CreateMap<Db.VerbType, Xml.VerbType>();

            Mapper.CreateMap<Db.Verb, Xml.Verb>();

            Mapper.CreateMap<Db.Action, Xml.Action>();

            Mapper.CreateMap<Db.ResultType, Xml.ResultType>();

            Mapper.CreateMap<Db.JSONPropertyDataType, Xml.JSONPropertyDataType>();

            Mapper.CreateMap<Db.ResultTypeJSONProperty, Xml.ResultTypeJSONProperty>();

            Mapper.CreateMap<Db.Result, Xml.Result>();

            Mapper.CreateMap<Db.ActionResult, Xml.ActionResult>();

            Mapper.CreateMap<Db.Item, Xml.Item>();

            Mapper.CreateMap<Db.Event, Xml.Event>();

            Mapper.CreateMap<Db.Character, Xml.Character>();

            Mapper.CreateMap<Db.ItemActionRequirement, Xml.ItemActionRequirement>();

            Mapper.CreateMap<Db.EventActionRequirement, Xml.EventActionRequirement>();

            Mapper.CreateMap<Db.CharacterActionRequirement, Xml.CharacterActionRequirement>();

            Mapper.CreateMap<Db.Message, Xml.Message>();

            Mapper.CreateMap<Db.MessageChoice, Xml.MessageChoice>();

            Mapper.CreateMap<Db.MessageChoiceResult, Xml.MessageChoiceResult>();

            Mapper.CreateMap<Db.AreaRoomOnInitialLoad, Xml.AreaRoomOnInitialLoad>();
        }

        #region Export

        private static Xml.GinTub ExportGinTubToXml()
        {
            Xml.Item[] items = ReadAllItemsDb().Select(i => Mapper.Map<Xml.Item>(i)).ToArray();

            Xml.Event[] events = ReadAllEventsDb().Select(e => Mapper.Map<Xml.Event>(e)).ToArray();

            Xml.Character[] characters = ReadAllCharactersDb().Select(c => Mapper.Map<Xml.Character>(c)).ToArray();

            Xml.JSONPropertyDataType[] jsonPropertyDataTypes = ReadAllJSONPropertyDataTypesDb().Select(t => Mapper.Map<Xml.JSONPropertyDataType>(t)).ToArray();

            Xml.ResultType[] resultTypes = ReadAllResultTypesDb().Select(rt => Mapper.Map<Xml.ResultType>(rt)).ToArray();
            for (int i = 0; i < resultTypes.Length; ++i)
                ExportResultTypeToXml(ref resultTypes[i]);

            Xml.VerbType[] verbTypes = ReadAllVerbTypesDb().Select(rt => Mapper.Map<Xml.VerbType>(rt)).ToArray();
            for (int i = 0; i < verbTypes.Length; ++i)
                ExportVerbTypeToXml(ref verbTypes[i]);

            Xml.Location[] locations = ReadAllLocationsDb().Select(l => Mapper.Map<Xml.Location>(l)).ToArray();

            Xml.Message[] messages = ReadAllMessagesDb().Select(a => Mapper.Map<Xml.Message>(a)).ToArray();
            for (int i = 0; i < messages.Length; ++i)
                ExportMessageToXml(ref messages[i]);

            Xml.Area[] areas = ReadAllAreasDb().Select(a => Mapper.Map<Xml.Area>(a)).ToArray();
            for (int i = 0; i < areas.Length; ++i)
                ExportAreaToXml(ref areas[i]);

            Xml.AreaRoomOnInitialLoad areaRoomOnInitialLoad = Mapper.Map<Xml.AreaRoomOnInitialLoad>(ReadAreaRoomOnInitialLoadDb());

            Xml.GinTub ginTub = new Xml.GinTub();
            ginTub.Items = items;
            ginTub.Events = events;
            ginTub.Characters = characters;
            ginTub.JSONPropertyTypes = jsonPropertyDataTypes;
            ginTub.ResultTypes = resultTypes;
            ginTub.VerbTypes = verbTypes;
            ginTub.Locations = locations;
            ginTub.Messages = messages;
            ginTub.Areas = areas;
            ginTub.AreaRoomOnInitialLoad = areaRoomOnInitialLoad;
            return ginTub;
        }

        private static void ExportResultTypeToXml(ref Xml.ResultType resultType)
        {
            resultType.ResultTypeJSONProperties = ReadAllResultTypeJSONPropertiesForResultTypeDb(resultType.Id).
                Select(rtjp => Mapper.Map<Xml.ResultTypeJSONProperty>(rtjp)).ToArray();
            resultType.Results = ReadAllResultsForResultTypeDb(resultType.Id).
                Select(r => Mapper.Map<Xml.Result>(r)).ToArray();
        }

        private static void ExportVerbTypeToXml(ref Xml.VerbType verbType)
        {
            verbType.Verbs = ReadAllVerbsForVerbTypeDb(verbType.Id).
                Select(v => Mapper.Map<Xml.Verb>(v)).ToArray();
        }

        private static void ExportMessageToXml(ref Xml.Message message)
        {
            message.MessageChoices = ReadAllMessageChoicesForMessageDb(message.Id).
                Select(mc => Mapper.Map<Xml.MessageChoice>(mc)).ToArray();
            for (int i = 0; i < message.MessageChoices.Length; ++i)
                ExportMessageChoiceToXml(ref message.MessageChoices[i]);
        }

        private static void ExportMessageChoiceToXml(ref Xml.MessageChoice messageChoice)
        {
            messageChoice.MessageChoiceResults = ReadAllMessageChoiceResultsForMessageChoiceDb(messageChoice.Id).
                Select(r => Mapper.Map<Xml.MessageChoiceResult>(r)).ToArray();
        }

        private static void ExportAreaToXml(ref Xml.Area area)
        {
            area.Rooms = ReadAllRoomsInAreaDb(area.Id).
                Select(r => Mapper.Map<Xml.Room>(r)).ToArray();
            for (int i = 0; i < area.Rooms.Length; ++i)
                ExportRoomToXml(ref area.Rooms[i]);
        }

        private static void ExportRoomToXml(ref Xml.Room room)
        {
            room.Paragraphs = ReadAllParagraphsForRoomAndRoomStateDb(room.Id, null).
                Select(p => Mapper.Map<Xml.Paragraph>(p)).ToArray();
            for (int i = 0; i < room.Paragraphs.Length; ++i)
                ExportParagraphToXml(ref room.Paragraphs[i]);
            room.RoomStates = ReadAllRoomStatesForRoomDb(room.Id).
                Select(rs => Mapper.Map<Xml.RoomState>(rs)).ToArray();
            foreach(var roomState in room.RoomStates)
            {
                roomState.Paragraphs = ReadAllParagraphsForRoomAndRoomStateDb(room.Id, roomState.Id).
                    Select(p => Mapper.Map<Xml.Paragraph>(p)).ToArray();
                for (int i = 0; i < roomState.Paragraphs.Length; ++i)
                    ExportParagraphToXml(ref roomState.Paragraphs[i]);
            }
        }

        private static void ExportParagraphToXml(ref Xml.Paragraph paragraph)
        {
            paragraph.ParagraphStates = ReadAllParagraphStatesForParagraphDb(paragraph.Id).
                Select(ps => Mapper.Map<Xml.ParagraphState>(ps)).ToArray();
            for (int i = 0; i < paragraph.ParagraphStates.Length; ++i)
                ExportParagraphStateToXml(ref paragraph.ParagraphStates[i]);
        }

        private static void ExportParagraphStateToXml(ref Xml.ParagraphState paragraphState)
        {
            paragraphState.Nouns = ReadAllNounsForParagraphStateDb(paragraphState.Id).
                Select(n => Mapper.Map<Xml.Noun>(n)).ToArray();
            for (int i = 0; i < paragraphState.Nouns.Length; ++i)
                ExportNounToXml(ref paragraphState.Nouns[i]);
        }

        private static void ExportNounToXml(ref Xml.Noun noun)
        {
            noun.Actions = ReadAllActionsForNounDb(noun.Id).
                Select(a => Mapper.Map<Xml.Action>(a)).ToArray();
            for(int i = 0; i < noun.Actions.Length; ++i)
                ExportActionToXml(ref noun.Actions[i]);
        }

        private static void ExportActionToXml(ref Xml.Action action)
        {
            action.ActionResults = ReadAllActionResultsForActionDb(action.Id).
                Select(a => Mapper.Map<Xml.ActionResult>(a)).ToArray();
            action.ItemActionRequirements = ReadAllItemActionRequirementsForActionDb(action.Id).
                Select(i => Mapper.Map<Xml.ItemActionRequirement>(i)).ToArray();
            action.EventActionRequirements = ReadAllEventActionRequirementsForActionDb(action.Id).
                Select(e => Mapper.Map<Xml.EventActionRequirement>(e)).ToArray();
            action.CharacterActionRequirements = ReadAllCharacterActionRequirementsForActionDb(action.Id).
                Select(c => Mapper.Map<Xml.CharacterActionRequirement>(c)).ToArray();
        }

        #endregion


        #region Import

        #region XmlModel

        private static void ImportGinTubFromXml(Xml.GinTub ginTub)
        {
            foreach (var item in ginTub.Items)
                ImportItem(item.Id, item.Name, item.Description);
            foreach (var evnt in ginTub.Events)
                ImportEvent(evnt.Id, evnt.Name, evnt.Description);
            foreach (var character in ginTub.Characters)
                ImportCharacter(character.Id, character.Name, character.Description);
            foreach (var jsonPropertyDataType in ginTub.JSONPropertyTypes)
                ImportJSONPropertyDataType(jsonPropertyDataType.Id, jsonPropertyDataType.DataType);
            foreach (var resultType in ginTub.ResultTypes)
                ImportResultTypeFromXml(resultType);
            foreach (var verbType in ginTub.VerbTypes)
                ImportVerbTypeFromXml(verbType);
            foreach (var location in ginTub.Locations)
                ImportLocation(location.Id, location.Name, location.LocationFile);
            foreach (var message in ginTub.Messages)
                ImportMessageFromXml(message);
            foreach (var area in ginTub.Areas)
                ImportAreaFromXml(area);
            if(ginTub.AreaRoomOnInitialLoad != null)
                ImportAreaRoomOnInitialLoad(ginTub.AreaRoomOnInitialLoad.Area, ginTub.AreaRoomOnInitialLoad.Room);
        }

        private static void ImportResultTypeFromXml(Xml.ResultType resultType)
        {
            ImportResultType(resultType.Id, resultType.Name);
            foreach (var resultTypeJSONProperty in resultType.ResultTypeJSONProperties)
                ImportResultTypeJSONProperty(resultTypeJSONProperty.Id, resultTypeJSONProperty.JSONProperty, resultTypeJSONProperty.DataType, resultType.Id);
            foreach (var result in resultType.Results)
                ImportResult(result.Id, result.Name, result.JSONData, resultType.Id);
        }

        private static void ImportVerbTypeFromXml(Xml.VerbType verbType)
        {
            ImportVerbType(verbType.Id, verbType.Name);
            foreach (var verb in verbType.Verbs)
                ImportVerb(verb.Id, verb.Name, verbType.Id);
        }

        private static void ImportMessageFromXml(Xml.Message message)
        {
            ImportMessage(message.Id, message.Name, message.Text);
            foreach (var messageChoice in message.MessageChoices)
                ImportMessageChoiceFromXml(messageChoice, message.Id);
        }

        private static void ImportMessageChoiceFromXml(Xml.MessageChoice messageChoice, int messageId)
        {
            ImportMessageChoice(messageChoice.Id, messageChoice.Name, messageChoice.Text, messageId);
            foreach (var messageChoiceResult in messageChoice.MessageChoiceResults)
                ImportMessageChoiceResult(messageChoiceResult.Id, messageChoiceResult.Result, messageChoice.Id);
        }

        private static void ImportAreaFromXml(Xml.Area area)
        {
            ImportArea(area.Id, area.Name);
            foreach (var room in area.Rooms)
                ImportRoomFromXml(room, area.Id);
        }

        private static void ImportRoomFromXml(Xml.Room room, int areaId)
        {
            ImportRoom(room.Id, room.Name, room.X, room.Y, room.Z, areaId);
            /*foreach (var paragraph in room.Paragraphs)
                ImportParagraphFromXml(paragraph, room.Id, null);
            foreach(var roomState in room.RoomStates)
            {
                ImportRoomState(roomState.Id, roomState.State, roomState.Time, roomState.Location, room.Id);
                foreach (var paragraph in roomState.Paragraphs)
                    ImportParagraphFromXml(paragraph, room.Id, roomState.Id);
            }*/
        }

        private static void ImportParagraphFromXml(Xml.Paragraph paragraph, int roomId)
        {
            ImportParagraph(paragraph.Id, paragraph.Order, roomId);
            foreach (var paragraphState in paragraph.ParagraphStates)
                ImportParagraphStateFromXml(paragraphState, paragraph.Id);
        }

        private static void ImportParagraphStateFromXml(Xml.ParagraphState paragraphState, int paragraphId)
        {
            ImportParagraphState(paragraphState.Id, paragraphState.State, paragraphState.Text, paragraphId);
            foreach (var noun in paragraphState.Nouns)
                ImportNounFromXml(noun, paragraphState.Id);
        }

        private static void ImportNounFromXml(Xml.Noun noun, int paragraphStateId)
        {
            ImportNoun(noun.Id, noun.Text, paragraphStateId);
            foreach (var action in noun.Actions)
                ImportActionFromXml(action, noun.Id);
        }

        private static void ImportActionFromXml(Xml.Action action, int nounId)
        {
            ImportAction(action.Id, action.VerbType, nounId);
            foreach (var actionResult in action.ActionResults)
                ImportActionResult(actionResult.Id, actionResult.Result, action.Id);
            foreach (var requirement in action.ItemActionRequirements)
                ImportItemActionRequirement(requirement.Id, requirement.Item, action.Id);
            foreach (var requirement in action.EventActionRequirements)
                ImportEventActionRequirement(requirement.Id, requirement.Event, action.Id);
            foreach (var requirement in action.CharacterActionRequirements)
                ImportCharacterActionRequirement(requirement.Id, requirement.Character, action.Id);
        }

        #endregion


        #region DbModel

        private static void ImportItem(int id, string name, string description)
        {
            try
            {
                m_entities.dev_ImportItem(id, name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportItem", e);
            }
        }

        private static void ImportEvent(int id, string name, string description)
        {
            try
            {
                m_entities.dev_ImportEvent(id, name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportEvent", e);
            }
        }

        private static void ImportCharacter(int id, string name, string description)
        {
            try
            {
                m_entities.dev_ImportCharacter(id, name, description);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportCharacter", e);
            }
        }

        private static void ImportLocation(int id, string name, string locationFile)
        {
            try
            {
                m_entities.dev_ImportLocation(id, name, locationFile);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportLocation", e);
            }
        }

        private static void ImportResultType(int id, string name)
        {
            try
            {
                m_entities.dev_ImportResultType(id, name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportResultType", e);
            }
        }

        private static void ImportJSONPropertyDataType(int id, string dataType)
        {
            try
            {
                m_entities.dev_ImportJSONPropertyDataType(id, dataType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportJSONPropertyDataType", e);
            }
        }

        private static void ImportResultTypeJSONProperty(int id, string jsonProperty, int dataType, int resultType)
        {
            try
            {
                m_entities.dev_ImportResultTypeJSONProperty(id, jsonProperty, dataType, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportResultTypeJSONProperty", e);
            }
        }

        private static void ImportResult(int id, string name, string jsonData, int resultType)
        {
            try
            {
                m_entities.dev_ImportResult(id, name, jsonData, resultType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportResult", e);
            }
        }

        private static void ImportVerbType(int id, string name)
        {
            try
            {
                m_entities.dev_ImportVerbType(id, name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportVerbType", e);
            }
        }

        private static void ImportVerb(int id, string name, int verbType)
        {
            try
            {
                m_entities.dev_ImportVerb(id, name, verbType);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportVerb", e);
            }
        }

        private static void ImportMessage(int id, string name, string text)
        {
            try
            {
                m_entities.dev_ImportMessage(id, name, text);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportMessage", e);
            }
        }

        private static void ImportMessageChoice(int id, string name, string text, int message)
        {
            try
            {
                m_entities.dev_ImportMessageChoice(id, name, text, message);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportMessageChoice", e);
            }
        }

        private static void ImportMessageChoiceResult(int id, int result, int messageChoice)
        {
            try
            {
                m_entities.dev_ImportMessageChoiceResult(id, result, messageChoice);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportMessageChoiceResult", e);
            }
        }

        private static void ImportArea(int id, string name)
        {
            try
            {
                m_entities.dev_ImportArea(id, name);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportArea", e);
            }
        }

        private static void ImportRoom(int id, string name, int x, int y, int z, int area)
        {
            try
            {
                m_entities.dev_ImportRoom(id, name, x, y, z, area);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportRoom", e);
            }
        }

        private static void ImportRoomState(int id, int state, TimeSpan time, int location, int room)
        {
            try
            {
                m_entities.dev_ImportRoomState(id, state, time, location, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportRoomState", e);
            }
        }

        private static void ImportParagraph(int id, int order, int room)
        {
            try
            {
                m_entities.dev_ImportParagraph(id, order, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportParagraph", e);
            }
        }

        private static void ImportParagraphState(int id, int state, string text, int paragraph)
        {
            try
            {
                m_entities.dev_ImportParagraphState(id, state, text, paragraph);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportParagraphState", e);
            }
        }

        private static void ImportNoun(int id, string text, int paragraphState)
        {
            try
            {
                m_entities.dev_ImportNoun(id, text, paragraphState);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportNoun", e);
            }
        }

        private static void ImportAction(int id, int verbType, int noun)
        {
            try
            {
                m_entities.dev_ImportAction(id, verbType, noun);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportAction", e);
            }
        }

        private static void ImportActionResult(int id, int result, int action)
        {
            try
            {
                m_entities.dev_ImportActionResult(id, result, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportActionResult", e);
            }
        }

        private static void ImportItemActionRequirement(int id, int item, int action)
        {
            try
            {
                m_entities.dev_ImportItemActionRequirement(id, item, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportItemActionRequirement", e);
            }
        }

        private static void ImportEventActionRequirement(int id, int evnt, int action)
        {
            try
            {
                m_entities.dev_ImportEventActionRequirement(id, evnt, action);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportEventActionRequirement", e);
            }
        }

        private static void ImportCharacterActionRequirement(int id, int character, int action)
        {
            try
            {
                m_entities.dev_ImportCharacterActionRequirement(id, character, action);
            }
            catch(Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportCharacterActionRequirement", e);
            }
        }

        private static void ImportAreaRoomOnInitialLoad(int area, int room)
        {
            try
            {
                m_entities.dev_ImportAreaRoomOnInitialLoad(area, room);
            }
            catch (Exception e)
            {
                throw new GinTubDatabaseException("dev_ImportAreaRoomOnInitialLoad", e);
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

    }
}
