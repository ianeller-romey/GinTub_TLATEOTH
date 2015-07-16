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
    public class UserControl_ResultTypeJSONPropertyModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_ResultTypeJSONProperty m_userControl_resultTypeJSONProperty;

        #endregion


        #region MEMBER PROPERTIES

        public int? ResultTypeJSONPropertyId { get { return m_userControl_resultTypeJSONProperty.ResultTypeJSONPropertyId; } }
        public string ResultTypeJSONPropertyJSONProperty { get { return m_userControl_resultTypeJSONProperty.ResultTypeJSONPropertyJSONProperty; } }
        public int? ResultTypeJSONPropertyDataType { get { return m_userControl_resultTypeJSONProperty.ResultTypeJSONPropertyDataType; } }
        public int ResultTypeId { get { return m_userControl_resultTypeJSONProperty.ResultTypeId; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ResultTypeJSONPropertyModification
        (
            int? resultTypeJSONPropertyId, 
            string resultTypeJSONPropertyJSONProperty,
            int? resultTypeJSONPropertyDataType,
            int resultTypeId
        )
        {
            CreateControls(resultTypeJSONPropertyId, resultTypeJSONPropertyJSONProperty, resultTypeJSONPropertyDataType, resultTypeId);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_resultTypeJSONProperty.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_resultTypeJSONProperty.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls
        (
            int? resultTypeJSONPropertyId, 
            string resultTypeJSONPropertyJSONProperty, 
            int? resultTypeJSONPropertyDataType,
            int resultTypeId
        )
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyResultTypeJSONProperty = new Button() { Content = "Modify ResultTypeJSONProperty" };
            button_modifyResultTypeJSONProperty.Click += Button_UpdateResultTypeJSONProperty_Click;
            grid_main.SetGridRowColumn(button_modifyResultTypeJSONProperty, 0, 0);

            m_userControl_resultTypeJSONProperty = 
                new UserControl_ResultTypeJSONProperty
                (
                    resultTypeJSONPropertyId,
                    resultTypeJSONPropertyJSONProperty, 
                    resultTypeJSONPropertyDataType, 
                    resultTypeId, 
                    false
                );
            grid_main.SetGridRowColumn(m_userControl_resultTypeJSONProperty, 1, 0);
            m_userControl_resultTypeJSONProperty.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_UpdateResultTypeJSONProperty_Click(object sender, RoutedEventArgs e)
        {
            Window_ResultTypeJSONProperty window =
                new Window_ResultTypeJSONProperty
                (
                    m_userControl_resultTypeJSONProperty.ResultTypeJSONPropertyId, 
                    m_userControl_resultTypeJSONProperty.ResultTypeJSONPropertyJSONProperty,
                    m_userControl_resultTypeJSONProperty.ResultTypeJSONPropertyDataType,
                    m_userControl_resultTypeJSONProperty.ResultTypeId,
                    (win) =>
                    {
                        Window_ResultTypeJSONProperty wWin = win as Window_ResultTypeJSONProperty;
                        if (wWin != null)
                            GinTubBuilderManager.UpdateResultTypeJSONProperty
                            (
                                wWin.ResultTypeJSONPropertyId.Value,
                                wWin.ResultTypeJSONPropertyJSONProperty,
                                wWin.ResultTypeJSONPropertyDataType.Value,
                                wWin.ResultTypeId
                            );
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
