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
    class EventActionRequirement
    {
        int m_id;
        int m_event;
        int m_action;

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
        public int Event
        {
            get { return m_event; }
            private set
            {
                if (m_event != value)
                    m_event = value;
            }
        }

        [DataMember]
        public int Action
        {
            get { return m_action; }
            private set
            {
                if (m_action != value)
                    m_action = value;
            }
        }
    }
}
