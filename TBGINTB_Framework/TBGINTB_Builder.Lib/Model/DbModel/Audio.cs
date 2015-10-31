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
    class Audio
    {
        int m_id;
        string m_name;
        string m_audioFile;
        bool m_isLooped;

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
        public string AudioFile
        {
            get { return m_audioFile; }
            private set
            {
                if (m_audioFile != value)
                    m_audioFile = value;
            }
        }

        [DataMember]
        public bool IsLooped
        {
            get { return m_isLooped; }
            private set
            {
                if (m_isLooped != value)
                    m_isLooped = value;
            }
        }
    }
}
