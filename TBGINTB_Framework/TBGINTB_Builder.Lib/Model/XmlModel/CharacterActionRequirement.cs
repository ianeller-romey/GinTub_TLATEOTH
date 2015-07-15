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
    public class CharacterActionRequirement
    {
        int m_id;
        int m_character;

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

        [XmlElement("Character")]
        public int Character
        {
            get { return m_character; }
            set
            {
                if (m_character != value)
                    m_character = value;
            }
        }
    }
}
