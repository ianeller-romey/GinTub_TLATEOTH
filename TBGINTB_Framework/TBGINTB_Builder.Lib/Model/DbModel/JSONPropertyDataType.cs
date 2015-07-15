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
    class JSONPropertyDataType
    {
        int m_id;
        string m_dataType;

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
        public string DataType
        {
            get { return m_dataType; }
            private set
            {
                if (m_dataType != value)
                    m_dataType = value;
            }
        }
    }
}
