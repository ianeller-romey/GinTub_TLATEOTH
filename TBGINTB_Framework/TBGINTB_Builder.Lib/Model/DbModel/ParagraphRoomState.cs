using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace TBGINTB_Builder.Lib.Model.DbModel
{
    [DataContract]
    class ParagraphRoomState
    {
        int m_id;
        int m_roomState;
        int m_paragraph;

        [DataMember]
        public int Id
        {
            get { return m_id; }
            private set
            {
                if (m_id != value)
                    m_id = value;
            }
        }

        [DataMember]
        public int RoomState
        {
            get { return m_roomState; }
            private set
            {
                if (m_roomState != value)
                    m_roomState = value;
            }
        }

        [DataMember]
        public int Paragraph
        {
            get { return m_paragraph; }
            private set
            {
                if (m_paragraph != value)
                    m_paragraph = value;
            }
        }
    }
}
