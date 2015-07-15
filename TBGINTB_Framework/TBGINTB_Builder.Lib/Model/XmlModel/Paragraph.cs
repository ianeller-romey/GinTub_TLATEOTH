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
    public class Paragraph
    {
        int m_id;
        int m_order;
        ParagraphState[] m_paragraphStates;

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

        [XmlElement("Order")]
        public int Order
        {
            get { return m_order; }
            set
            {
                if (m_order != value)
                    m_order = value;
            }
        }

        [XmlArray("ParagraphStates")]
        public ParagraphState[] ParagraphStates
        {
            get { return m_paragraphStates; }
            set
            {
                if (m_paragraphStates != value)
                    m_paragraphStates = value;
            }
        }
    }
}
