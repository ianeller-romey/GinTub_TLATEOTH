using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace TBGINTB_Builder.Lib.Model.XmlModel
{
    public class Audio
    {
        int m_id;
        string m_name;
        string m_audioFile;
        bool m_isLooped;

        [XmlAttribute("Id")]
        public int Id
        {
            get { return m_id; }
            set
            {
                if (m_id != value)
                    m_id = value;
            }
        }

        [XmlElement("Name")]
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name != value)
                    m_name = value;
            }
        }

        [XmlElement("AudioFile")]
        public string AudioFile
        {
            get { return m_audioFile; }
            set
            {
                if (m_audioFile != value)
                    m_audioFile = value;
            }
        }

        [XmlElement("IsLooped")]
        public bool IsLooped
        {
            get { return m_isLooped; }
            set
            {
                if (m_isLooped != value)
                    m_isLooped = value;
            }
        }
    }
}
