using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace TBGINTB_Builder.Lib
{

    public static class JSONPropertyManager
    {
        #region MEMBER FIELDS

        private delegate object ValidateBeforeFormatting(string valueAsString);
        private static ValidateBeforeFormatting
            ValidateNumber = (x) =>
                {
                    int outInt;
                    if (!int.TryParse(x, out outInt))
                    {
                        double outDouble;
                        if (!double.TryParse(x, out outDouble))
                            throw new FormatException("Number value not formatted correctly");
                        return outDouble;
                    }
                    return outInt;
                },
            ValidateString = (x) =>
                {
                    return x;
                },
            ValidateBoolean = (x) =>
                {
                    bool outBool;
                    if (!bool.TryParse(x, out outBool))
                        throw new FormatException("Boolean value not formatted correctly");
                    return outBool;
                },
            ValidateDateTime = (x) =>
                {
                    DateTime outDateTime;
                    if (!DateTime.TryParse(x, out outDateTime))
                        throw new FormatException("DateTime value not formatted correctly");
                    return outDateTime.Ticks;
                };

        private static Dictionary<int, string> m_dictionary_dataTypeFormatters;
        private static Dictionary<int, ValidateBeforeFormatting> m_dictionary_dataTypeFormatterValidators;
        private static Dictionary<JTokenType, int> m_dictionary_dataTypeJTokenTypes;
        private static readonly string 
            c_string_numberFormatter = "{0}",
            c_string_stringFormatter = "\"{0}\"",
            c_string_booleanFormatter = "{0}",
            c_string_datetimeFormatter = "\"\\/Date({0})\\/\"";


        #endregion


        #region MEMBER PROPERTIES

        public class JSONProperty
        {
            #region MEMBER FIELDS
            #endregion


            #region MEMBER PROPERTIES

            public string Name { get; private set; }
            public object Value { get; private set; }
            public int DataTypeId { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public JSONProperty(string propertyName, object propertyValue, int propertyDataTypeId)
            {
                Name = propertyName;
                Value = propertyValue;
                DataTypeId = propertyDataTypeId;
            }

            #endregion


            #region Private Functionality
            #endregion
            
            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public static void Initialize()
        {
            m_dictionary_dataTypeFormatters = new Dictionary<int, string>();
            m_dictionary_dataTypeFormatterValidators = new Dictionary<int, ValidateBeforeFormatting>();
            m_dictionary_dataTypeJTokenTypes = new Dictionary<JTokenType, int>();

            GinTubBuilderManager.JSONPropertyDataTypeAdded += GinTubBuilderManager_JSONPropertyDataTypeAdded;
            GinTubBuilderManager.LoadAllJSONPropertyDataTypes();
            GinTubBuilderManager.JSONPropertyDataTypeAdded -= GinTubBuilderManager_JSONPropertyDataTypeAdded;
        }

        public static List<JSONProperty> ParseJSONIntoJSONProperties(string json)
        {
            List<JSONProperty> jsonProperties = new List<JSONProperty>();

            dynamic convertedJSON = JsonConvert.DeserializeObject(json);
            JObject jObject = (JObject)convertedJSON;
            foreach (JToken jToken in jObject.Children())
            {
                if (jToken is JProperty)
                {
                    JProperty jProperty = jToken as JProperty;
                    jsonProperties.Add(new JSONProperty(jProperty.Name, jProperty.Value, JSONPropertyManager.GetJSONPropertyDataTypeIdFromJTokenType(jProperty.Value.Type)));
                }
            }

            return jsonProperties;
        }

        public static int GetJSONPropertyDataTypeIdFromJTokenType(JTokenType type)
        {
            return m_dictionary_dataTypeJTokenTypes[type];
        }

        public static string FormatJSONPropertyStringValueFromDataTypeId(string valueAsString, int dataTypeId)
        {
            return string.Format(m_dictionary_dataTypeFormatters[dataTypeId], m_dictionary_dataTypeFormatterValidators[dataTypeId](valueAsString));
        }

        #endregion


        #region Private Functionality

        private static void GinTubBuilderManager_JSONPropertyDataTypeAdded(object sender, GinTubBuilderManager.JSONPropertyDataTypeAddedEventArgs args)
        {
 	        switch(args.DataType)
            {
                case "Number":
                    m_dictionary_dataTypeFormatters.Add(args.Id, c_string_numberFormatter);
                    m_dictionary_dataTypeFormatterValidators.Add(args.Id, ValidateNumber);
                    m_dictionary_dataTypeJTokenTypes.Add(JTokenType.Integer, args.Id);
                    m_dictionary_dataTypeJTokenTypes.Add(JTokenType.Float, args.Id);
                    break;
                case "String":
                    m_dictionary_dataTypeFormatters.Add(args.Id, c_string_stringFormatter);
                    m_dictionary_dataTypeFormatterValidators.Add(args.Id, ValidateString);
                    m_dictionary_dataTypeJTokenTypes.Add(JTokenType.String, args.Id);
                    break;
                case "Boolean":
                    m_dictionary_dataTypeFormatters.Add(args.Id, c_string_booleanFormatter);
                    m_dictionary_dataTypeFormatterValidators.Add(args.Id, ValidateBoolean);
                    m_dictionary_dataTypeJTokenTypes.Add(JTokenType.Boolean, args.Id);
                    break;
                case "DateTime":
                    m_dictionary_dataTypeFormatters.Add(args.Id, c_string_datetimeFormatter);
                    m_dictionary_dataTypeFormatterValidators.Add(args.Id, ValidateDateTime);
                    m_dictionary_dataTypeJTokenTypes.Add(JTokenType.Date, args.Id);
                    break;
                default:
                    throw new ArgumentException("Unsupported JSONPropertyDataType", "args.DataType");
            }
        }

        #endregion

        #endregion
    }

}
