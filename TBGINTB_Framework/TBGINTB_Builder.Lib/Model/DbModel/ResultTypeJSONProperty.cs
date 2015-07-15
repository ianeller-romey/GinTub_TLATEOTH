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
    class ResultTypeJSONProperty
    {
        int m_id;
        string m_jsonProperty;
        int m_dataType;
        int m_resultType;

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
        public string JSONProperty
        {
            get { return m_jsonProperty; }
            private set
            {
                if (m_jsonProperty != value)
                    m_jsonProperty = value;
            }
        }

        [DataMember]
        public int DataType
        {
            get { return m_dataType; }
            private set
            {
                if (m_dataType != value)
                    m_dataType = value;
            }
        }

        [DataMember]
        public int ResultType
        {
            get { return m_resultType; }
            private set
            {
                if (m_resultType != value)
                    m_resultType = value;
            }
        }
    }
}
