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
    class RoomState
    {
        int m_id;
        int m_room;
        int m_state;
        int m_location;
        TimeSpan m_time;

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
        public int Location
        {
            get { return m_location; }
            private set
            {
                if (m_location != value)
                    m_location = value;
            }
        }

        [DataMember]
        public TimeSpan Time
        {
            get { return m_time; }
            private set
            {
                if (m_time != value)
                    m_time = value;
            }
        }
    }
}
