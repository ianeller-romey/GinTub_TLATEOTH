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
    class Area
    {
        int m_id;
        string m_name;
        bool m_displayTime;
        int? m_audio;
        int m_maxX, m_minX;
        int m_maxY, m_minY;
        int m_maxZ, m_minZ;
        int m_numRooms;

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
        public bool DisplayTime
        {
            get { return m_displayTime; }
            private set
            {
                if (m_displayTime != value)
                    m_displayTime = value;
            }
        }

        [DataMember]
        public int? Audio
        {
            get { return m_audio; }
            private set
            {
                if (m_audio != value)
                    m_audio = value;
            }
        }

        public int MaxX
        {
            get { return m_maxX; }
            private set
            {
                if (m_maxX != value)
                    m_maxX = value;
            }
        }

        public int MinX
        {
            get { return m_minX; }
            private set
            {
                if (m_minX != value)
                    m_minX = value;
            }
        }

        public int MaxY
        {
            get { return m_maxY; }
            private set
            {
                if (m_maxY != value)
                    m_maxY = value;
            }
        }

        public int MinY
        {
            get { return m_minY; }
            private set
            {
                if (m_minY != value)
                    m_minY = value;
            }
        }

        public int MaxZ
        {
            get { return m_maxZ; }
            private set
            {
                if (m_maxZ != value)
                    m_maxZ = value;
            }
        }

        public int MinZ
        {
            get { return m_minZ; }
            private set
            {
                if (m_minZ != value)
                    m_minZ = value;
            }
        }

        public int NumRooms
        {
            get { return m_numRooms; }
            private set
            {
                if (m_numRooms != value)
                    m_numRooms = value;
            }
        }

    }
}
