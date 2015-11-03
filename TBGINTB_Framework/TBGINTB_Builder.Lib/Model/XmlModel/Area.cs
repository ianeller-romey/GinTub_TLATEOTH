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
    public class Area
    {
        int m_id;
        string m_name;
        bool m_displayTime;
        int? m_audio;
        Room[] m_rooms;

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

        [XmlElement("DisplayTime")]
        public bool DisplayTime
        {
            get { return m_displayTime; }
            set
            {
                if (m_displayTime != value)
                    m_displayTime = value;
            }
        }

        [XmlElement("Audio")]
        public int? Audio
        {
            get { return m_audio; }
            set
            {
                if (m_audio != value)
                    m_audio = value;
            }
        }

        [XmlArray("Rooms")]
        public Room[] Rooms
        {
            get { return m_rooms; }
            set
            {
                if (m_rooms != value)
                    m_rooms = value;
            }
        }
    }
}
