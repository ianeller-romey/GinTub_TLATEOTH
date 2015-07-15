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
    public class Room
    {
        int m_id;
        string m_name;
        int m_x;
        int m_y;
        int m_z;
        RoomState[] m_roomStates;
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

        [XmlElement("X")]
        public int X
        {
            get { return m_x; }
            set
            {
                if (m_x != value)
                    m_x = value;
            }
        }

        [XmlElement("Y")]
        public int Y
        {
            get { return m_y; }
            set
            {
                if (m_y != value)
                    m_y = value;
            }
        }

        [XmlElement("Z")]
        public int Z
        {
            get { return m_z; }
            set
            {
                if (m_z != value)
                    m_z = value;
            }
        }

        [XmlArray("RoomStates")]
        public RoomState[] RoomStates
        {
            get { return m_roomStates; }
            set
            {
                if (m_roomStates != value)
                    m_roomStates = value;
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
