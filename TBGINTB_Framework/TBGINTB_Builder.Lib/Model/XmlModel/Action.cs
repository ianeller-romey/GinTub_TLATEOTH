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
    public class Action
    {
        int m_id;
        int m_verbType;
        ActionResult[] m_actionResults;
        ItemActionRequirement[] m_itemActionRequirements;
        EventActionRequirement[] m_eventActionRequirements;
        CharacterActionRequirement[] m_characterActionRequirements;

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

        [XmlElement("VerbType")]
        public int VerbType
        {
            get { return m_verbType; }
            set
            {
                if (m_verbType != value)
                    m_verbType = value;
            }
        }

        [XmlArray("ActionResults")]
        public ActionResult[] ActionResults
        {
            get { return m_actionResults; }
            set
            {
                if (m_actionResults != value)
                    m_actionResults = value;
            }
        }

        [XmlArray("ItemActionRequirements")]
        public ItemActionRequirement[] ItemActionRequirements
        {
            get { return m_itemActionRequirements; }
            set
            {
                if (m_itemActionRequirements != value)
                    m_itemActionRequirements = value;
            }
        }

        [XmlArray("EventActionRequirements")]
        public EventActionRequirement[] EventActionRequirements
        {
            get { return m_eventActionRequirements; }
            set
            {
                if (m_eventActionRequirements != value)
                    m_eventActionRequirements = value;
            }
        }

        [XmlArray("CharacterActionRequirements")]
        public CharacterActionRequirement[] CharacterActionRequirements
        {
            get { return m_characterActionRequirements; }
            set
            {
                if (m_characterActionRequirements != value)
                    m_characterActionRequirements = value;
            }
        }
    }
}
