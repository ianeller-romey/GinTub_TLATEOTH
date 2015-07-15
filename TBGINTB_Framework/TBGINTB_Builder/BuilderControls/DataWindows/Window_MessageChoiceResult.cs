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
    public class Window_MessageChoiceResult : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_MessageChoiceResult m_userControl_messageChoiceResult;

        #endregion


        #region MEMBER PROPERTIES

        public int? MessageChoiceResultId { get { return m_userControl_messageChoiceResult.MessageChoiceResultId; } }
        public int? MessageChoiceResultResult { get { return m_userControl_messageChoiceResult.MessageChoiceResultResult; } }
        public int? MessageChoiceResultMessageChoice { get { return m_userControl_messageChoiceResult.MessageChoiceResultMessageChoice; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_MessageChoiceResult(int? messageChoiceResultId, int? messageChoiceResultResult, int? messageChoiceResultMessageChoice, int messageId, int resultTypeId, TaskOnAccept task) :
            base("Message Choice Result Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(messageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice, messageId);
            m_userControl_messageChoiceResult.SetActiveAndRegisterForGinTubEvents(); // needed for possible results, messageChoices
            GinTubBuilderManager.LoadAllResultsForResultType(resultTypeId);
            GinTubBuilderManager.LoadAllMessageChoicesForMessage(messageId);
        }

        public Window_MessageChoiceResult(int? messageChoiceResultId, int? messageChoiceResultResult, int? messageChoiceResultMessageChoice, int messageId, TaskOnAccept task) :
            base("Message Choice Result Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(messageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice, messageId);
            m_userControl_messageChoiceResult.SetActiveAndRegisterForGinTubEvents(); // needed for possible results, messageChoices
            if (messageChoiceResultMessageChoice.HasValue)
                GinTubBuilderManager.LoadAllResultsForMessageChoiceResultType(messageChoiceResultMessageChoice.Value);
            GinTubBuilderManager.LoadAllMessageChoicesForMessage(messageId);
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? messageChoiceResultId, int? messageChoiceResultResult, int? messageChoiceResultMessageChoice, int messageId)
        {
            m_userControl_messageChoiceResult = new UserControl_MessageChoiceResult(messageChoiceResultId, messageChoiceResultResult, messageChoiceResultMessageChoice, messageId, true);
            return m_userControl_messageChoiceResult;
        }

        #endregion

        #endregion
    }
}
