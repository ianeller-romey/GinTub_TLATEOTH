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
    class Action
    {
        int m_id;
        int m_verbType;
        int m_noun;
        string m_name;

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
        public int VerbType
        {
            get { return m_verbType; }
            private set
            {
                if (m_verbType != value)
                    m_verbType = value;
            }
        }

        [DataMember]
        public int Noun
        {
            get { return m_noun; }
            private set
            {
                if (m_noun != value)
                    m_noun = value;
            }
        }

        public string Name
        {
            get { return m_name; }
            private set
            {
                if (m_name != value)
                    m_name = value;
            }
        }
    }
}
