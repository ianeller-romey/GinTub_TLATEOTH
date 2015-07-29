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
    public class UserControl_Bordered_Action : UserControl_Action, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS
        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Bordered_Action(int? actionId, int? actionVerbType, int? actionNoun, int paragraphStateId, bool enableEditing, bool enableSelecting) :
            base(actionId, actionVerbType, actionNoun, paragraphStateId, enableEditing, enableSelecting)
        {
            CreateControls();
        }

        public new void SetActiveAndRegisterForGinTubEvents()
        {
            base.SetActiveAndRegisterForGinTubEvents();
        }

        public new void SetInactiveAndUnregisterFromGinTubEvents()
        {
            base.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality
        
        private void CreateControls()
        {
            var content = Content;
            Content = null;
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = content as UIElement };
            Content = border;
        }

        #endregion

        #endregion
    }
}
