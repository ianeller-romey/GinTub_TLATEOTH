using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Bordered_Location : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Location m_userControl_location;

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Location(bool enableEditing)
        {
            CreateControls(enableEditing);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_location.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_location.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(bool enableEditing)
        {
            m_userControl_location = new UserControl_Location(enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_location };
            Content = border;
        }

        #endregion

        #endregion
    }
}
