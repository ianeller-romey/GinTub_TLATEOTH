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
    public class UserControl_ResultModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Result m_userControl_result;

        #endregion


        #region MEMBER PROPERTIES

        public int? ResultId { get { return m_userControl_result.ResultId; } }
        public string ResultName { get { return m_userControl_result.ResultName; } }
        public string ResultJSONData { get { return m_userControl_result.ResultJSONData; } }
        public int ResultTypeId { get { return m_userControl_result.ResultTypeId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ResultModification(int? resultId, string resultName, string resultJSONData, int resultTypeId)
        {
            CreateControls(resultId, resultName, resultJSONData, resultTypeId);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_result.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_result.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? resultId, string resultName, string resultJSONData, int resultTypeId)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyResult = new Button() { Content = "Modify Result" };
            button_modifyResult.Click += Button_ModifyResult_Click;
            grid_main.SetGridRowColumn(button_modifyResult, 0, 0);

            m_userControl_result = new UserControl_Result(resultId, resultName, resultJSONData, resultTypeId, false);
            grid_main.SetGridRowColumn(m_userControl_result, 1, 0);
            m_userControl_result.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyResult_Click(object sender, RoutedEventArgs e)
        {
            Window_Result window =
                new Window_Result
                (
                    m_userControl_result.ResultId, 
                    m_userControl_result.ResultName, 
                    m_userControl_result.ResultJSONData,
                    m_userControl_result.ResultTypeId,
                    (win) =>
                    {
                        Window_Result wWin = win as Window_Result;
                        if (wWin != null)
                            GinTubBuilderManager.ModifyResult(wWin.ResultId.Value, wWin.ResultName, wWin.ResultJSONData, wWin.ResultTypeId);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
