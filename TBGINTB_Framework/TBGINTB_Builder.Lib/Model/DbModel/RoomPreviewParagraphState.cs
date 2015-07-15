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
    class RoomPreviewParagraphState
    {
        int m_id;
        string m_text;
        int m_state;
        int m_paragraph;
        int m_room;
        RoomPreviewNoun[] m_nouns;

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
        public string Text
        {
            get { return m_text; }
            private set
            {
                if (m_text != value)
                    m_text = value;
            }
        }

        [DataMember]
        public int State
        {
            get { return m_state; }
            private set
            {
                if (m_state != value)
                    m_state = value;
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

        [DataMember]
        public int Room
        {
            get { return m_room; }
            private set
            {
                if (m_room != value)
                    m_room = value;
            }
        }

        [DataMember]
        public RoomPreviewNoun[] Nouns
        {
            get { return m_nouns; }
            private set
            {
                if (m_nouns != value)
                    m_nouns = value;
            }
        }
    }
}
