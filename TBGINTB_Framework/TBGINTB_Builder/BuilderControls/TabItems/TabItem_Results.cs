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
    public class TabItem_Results : TabItem, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_ResultType m_grid_resultType;
        UserControl_Results m_itemsControl_results;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public TabItem_Results()
        {
            Header = "Results";
            Content = CreateControls();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_grid_resultType.SetActiveAndRegisterForGinTubEvents();
            if(m_itemsControl_results != null)
                m_itemsControl_results.SetActiveAndRegisterForGinTubEvents();

            GinTubBuilderManager.ReadAllResultTypes();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_grid_resultType.SetInactiveAndUnregisterFromGinTubEvents();
            if (m_itemsControl_results != null)
                m_itemsControl_results.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            m_grid_resultType = new UserControl_ResultType(true);

            return m_grid_resultType;
        }

        #endregion

        #endregion

    }
}
