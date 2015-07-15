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
    public class JSONPropertyDataType
    {
        int m_id;
        string m_dataType;

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

        [XmlElement("DataType")]
        public string DataType
        {
            get { return m_dataType; }
            set
            {
                if (m_dataType != value)
                    m_dataType = value;
            }
        }
    }
}
