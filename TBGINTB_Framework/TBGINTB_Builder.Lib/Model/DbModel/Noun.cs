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
    class Noun
    {
        int m_id;
        string m_text;
        int m_paragraphState;

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
        public int ParagraphState
        {
            get { return m_paragraphState; }
            private set
            {
                if (m_paragraphState != value)
                    m_paragraphState = value;
            }
        }
    }
}
