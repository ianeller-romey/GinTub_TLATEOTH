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
    public class RoomState
    {
        int m_id;
        int m_state;
        int m_location;
        DateTime m_time;
        Paragraph[] m_paragraphs;

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

        [XmlElement("Location")]
        public int Location
        {
            get { return m_location; }
            set
            {
                if (m_location != value)
                    m_location = value;
            }
        }

        [XmlElement("Time")]
        public DateTime Time
        {
            get { return m_time; }
            set
            {
                if (m_time != value)
                    m_time = value;
            }
        }

        [XmlArray("Paragraphs")]
        public Paragraph[] Paragraphs
        {
            get { return m_paragraphs; }
            set
            {
                if (m_paragraphs != value)
                    m_paragraphs = value;
            }
        }
    }
}
