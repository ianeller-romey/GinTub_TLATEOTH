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
    class MessageChoiceResult
    {
        int m_id;
        int m_result;
        int m_messageChoice;

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
        public int Result
        {
            get { return m_result; }
            private set
            {
                if (m_result != value)
                    m_result = value;
            }
        }

        [DataMember]
        public int MessageChoice
        {
            get { return m_messageChoice; }
            private set
            {
                if (m_messageChoice != value)
                    m_messageChoice = value;
            }
        }
    }
}
