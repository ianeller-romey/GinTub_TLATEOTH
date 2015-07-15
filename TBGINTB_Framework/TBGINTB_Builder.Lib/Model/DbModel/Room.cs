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
    class Room
    {
        int m_id;
        string m_name;
        int m_x;
        int m_y;
        int m_z;
        int m_area;

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
        public int X
        {
            get { return m_x; }
            private set
            {
                if (m_x != value)
                    m_x = value;
            }
        }

        [DataMember]
        public int Y
        {
            get { return m_y; }
            private set
            {
                if (m_y != value)
                    m_y = value;
            }
        }

        [DataMember]
        public int Z
        {
            get { return m_z; }
            private set
            {
                if (m_z != value)
                    m_z = value;
            }
        }

        [DataMember]
        public int Area
        {
            get { return m_area; }
            private set
            {
                if (m_area != value)
                    m_area = value;
            }
        }
    }
}
