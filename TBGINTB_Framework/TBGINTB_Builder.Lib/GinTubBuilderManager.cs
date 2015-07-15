using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TBGINTB_Builder.Lib.Repository;


namespace TBGINTB_Builder.Lib
{
    public static partial class GinTubBuilderManager
    {
        #region MEMBER FIELDS

        private static GinTubEntities m_entities = null;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public static void Initialize()
        {
            InitializeSprocsToDbModelMap();
            InitializeDbModelToXmlModelMap();

            m_entities = new GinTubEntities();
            m_entities.Configuration.AutoDetectChangesEnabled = false;
        }

        #endregion


        #region Private Functionality
        #endregion

        #endregion

    }
}
