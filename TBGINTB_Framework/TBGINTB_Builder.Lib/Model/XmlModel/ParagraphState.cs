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
    public class ParagraphState
    {
        int m_id;
        string m_text;
        int m_state;
        Noun[] m_nouns;

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

        [XmlElement("State")]
        public int State
        {
            get { return m_state; }
            set
            {
                if (m_state != value)
                    m_state = value;
            }
        }

        [XmlArray("Nouns")]
        public Noun[] Nouns
        {
            get { return m_nouns; }
            set
            {
                if (m_nouns != value)
                    m_nouns = value;
            }
        }
    }
}
