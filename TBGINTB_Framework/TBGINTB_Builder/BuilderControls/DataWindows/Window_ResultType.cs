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
    public class Window_ResultType : Window_TaskOnAccept
    {
        #region MEMBER FIELDS

        UserControl_ResultType m_userControl_resultType;

        #endregion


        #region MEMBER PROPERTIES

        public int? ResultTypeId { get { return m_userControl_resultType.SelectedResultTypeId; } }
        public string ResultTypeName { get { return m_userControl_resultType.SelectedResultTypeName; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Window_ResultType(int? resultTypeId, string resultTypeName, TaskOnAccept task) :
            base("Result Type Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(resultTypeId, resultTypeName);
            m_userControl_resultType.SetActiveAndRegisterForGinTubEvents(); // needed for possible nouns
            GinTubBuilderManager.LoadAllResultTypes();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? resultTypeId, string resultTypeName)
        {
            m_userControl_resultType = new UserControl_ResultType(true);
            return m_userControl_resultType;
        }

        #endregion

        #endregion
    }
}
