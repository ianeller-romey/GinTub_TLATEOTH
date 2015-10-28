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
    public class GameStateOnInitialLoad
    {
        int m_area;
        int m_room;
        TimeSpan m_time;

        [XmlAttribute("Area")]
        public int Area 
        {
            get { return m_area; }
            set
            {
                if (m_area != value)
                    m_area = value;
            }
        }

        [XmlElement("Room")]
        public int Room
        {
            get { return m_room; }
            set
            {
                if (m_room != value)
                    m_room = value;
            }
        }

        [XmlElement("Time")]
        public TimeSpan Time
        {
            get { return m_time; }
            set
            {
                if (m_time != value)
                    m_time = value;
            }
        }
    }
}
