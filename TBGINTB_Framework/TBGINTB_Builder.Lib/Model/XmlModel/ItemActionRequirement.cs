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
    public class ItemActionRequirement
    {
        int m_id;
        int m_item;

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

        [XmlElement("Item")]
        public int Item
        {
            get { return m_item; }
            set
            {
                if (m_item != value)
                    m_item = value;
            }
        }
    }
}
