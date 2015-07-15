using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace TBGINTB_Builder.Lib.Model.XmlModel
{
    public class ResultType
    {
        int m_id;
        string m_name;
        Result[] m_results;
        ResultTypeJSONProperty[] m_resultTypeJSONProperties;

        [XmlAttribute("Id")]
        public int Id
        {
            get { return m_id; }
            set
            {
                if (m_id != value)
                    m_id = value;
            }
        }

        [XmlElement("Name")]
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name != value)
                    m_name = value;
            }
        }

        [XmlArray("Results")]
        public Result[] Results
        {
            get { return m_results; }
            set
            {
                if (m_results != value)
                    m_results = value;
            }
        }

        [XmlArray("ResultTypeJSONProperty")]
        public ResultTypeJSONProperty[] ResultTypeJSONProperties
        {
            get { return m_resultTypeJSONProperties; }
            set
            {
                if (m_resultTypeJSONProperties != value)
                    m_resultTypeJSONProperties = value;
            }
        }
    }
}
