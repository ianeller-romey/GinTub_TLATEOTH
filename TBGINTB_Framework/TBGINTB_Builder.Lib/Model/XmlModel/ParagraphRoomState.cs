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
    public class ParagraphRoomState
    {
        int m_id;
        int m_roomState;

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

        [XmlElement("RoomState")]
        public int RoomState
        {
            get { return m_roomState; }
            set
            {
                if (m_roomState != value)
                    m_roomState = value;
            }
        }
    }
}
