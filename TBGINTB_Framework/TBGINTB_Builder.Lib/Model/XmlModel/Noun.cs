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
    public class Noun
    {
        int m_id;
        string m_text;
        Action[] m_actions;

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

        [XmlElement("Text")]
        public string Text
        {
            get { return m_text; }
            set
            {
                if (m_text != value)
                    m_text = value;
            }
        }

        [XmlArray("Actions")]
        public Action[] Actions
        {
            get { return m_actions; }
            set
            {
                if (m_actions != value)
                    m_actions = value;
            }
        }
    }
}
