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
    public class EventActionRequirement
    {
        int m_id;
        int m_event;

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

        [XmlElement("Event")]
        public int Event
        {
            get { return m_event; }
            set
            {
                if (m_event != value)
                    m_event = value;
            }
        }
    }
}
