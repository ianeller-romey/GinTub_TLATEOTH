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
    class MessageChoice
    {
        int m_id;
        string m_name;
        string m_text;
        int m_message;

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
        public string Name
        {
            get { return m_name; }
            private set
            {
                if (m_name != value)
                    m_name = value;
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
        public int Message
        {
            get { return m_message; }
            private set
            {
                if (m_message != value)
                    m_message = value;
            }
        }
    }
}
