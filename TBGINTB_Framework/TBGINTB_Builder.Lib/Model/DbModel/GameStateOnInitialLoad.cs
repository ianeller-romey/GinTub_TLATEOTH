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
    class GameStateOnInitialLoad
    {
        int? m_area;
        int? m_room;
        TimeSpan? m_time;

        [DataMember]
        public int? Area
        {
            get { return m_area; }
            private set
            {
                if (m_area != value)
                    m_area = value;
            }
        }

        [DataMember]
        public int? Room
        {
            get { return m_room; }
            private set
            {
                if (m_room != value)
                    m_room = value;
            }
        }

        [DataMember]
        public TimeSpan? Time
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
