using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_MessageTreeMessage : UserControl_Message, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS
        #endregion


        #region MEMBER PROPERTIES

        public int? MessageParentMessageChoiceId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_MessageTreeMessage(int? messageId, string messageName, string messageText, int? messageParentMessageChoiceId, bool enableEditing, bool enableSelecting) :
            base(messageId, messageName, messageText, enableEditing, enableSelecting)
        {
            MessageParentMessageChoiceId = messageParentMessageChoiceId;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
        }

        #endregion


        #region Private Functionality
        #endregion

        #endregion
    }
}
