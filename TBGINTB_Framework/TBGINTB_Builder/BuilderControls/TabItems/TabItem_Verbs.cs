using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class TabItem_Verbs : TabItem, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_VerbType m_grid_verbType;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public TabItem_Verbs()
        {
            Header = "Verbs";
            Content = CreateControls();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_grid_verbType.SetActiveAndRegisterForGinTubEvents();

            GinTubBuilderManager.LoadAllVerbTypes();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_grid_verbType.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            m_grid_verbType = new UserControl_VerbType(true);

            return m_grid_verbType;
        }

        #endregion

        #endregion

    }
}
