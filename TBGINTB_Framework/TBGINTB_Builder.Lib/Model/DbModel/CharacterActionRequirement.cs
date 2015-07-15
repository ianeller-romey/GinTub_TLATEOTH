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
    class CharacterActionRequirement
    {
        int m_id;
        int m_character;
        int m_action;

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
        public int Character
        {
            get { return m_character; }
            private set
            {
                if (m_character != value)
                    m_character = value;
            }
        }

        [DataMember]
        public int Action
        {
            get { return m_action; }
            private set
            {
                if (m_action != value)
                    m_action = value;
            }
        }
    }
}
