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
    class Result
    {
        int m_id;
        string m_name;
        string m_jsonData;
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
        public string JSONData
        {
            get { return m_jsonData; }
            private set
            {
                if (m_jsonData != value)
                    m_jsonData = value;
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
