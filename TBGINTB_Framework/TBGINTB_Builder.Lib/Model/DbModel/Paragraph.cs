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
    class Paragraph
    {
        int m_id;
        int m_order;
        int m_room;

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
        public int Order
        {
            get { return m_order; }
            private set
            {
                if (m_order != value)
                    m_order = value;
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
    }
}
