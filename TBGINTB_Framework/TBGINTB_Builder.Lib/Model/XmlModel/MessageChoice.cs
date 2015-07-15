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
    public class MessageChoice
    {
        int m_id;
        string m_name;
        string m_text;
        MessageChoiceResult[] m_messageChoiceResults;

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

        [XmlElement("Text")]
        public string Text
        {
            get { return m_text; }
            set
            {
                if (m_text != value)
                    m_text = value;
            }
        }

        [XmlArray("MessageChoiceResults")]
        public MessageChoiceResult[] MessageChoiceResults
        {
            get { return m_messageChoiceResults; }
            set
            {
                if (m_messageChoiceResults != value)
                    m_messageChoiceResults = value;
            }
        }
    }
}
