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
    public class VerbType
    {
        int m_id;
        string m_name;
        Verb[] m_verbs;

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

        [XmlArray("Verbs")]
        public Verb[] Verbs
        {
            get { return m_verbs; }
            set
            {
                if (m_verbs != value)
                    m_verbs = value;
            }
        }
    }
}
