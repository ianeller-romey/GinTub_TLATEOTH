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
