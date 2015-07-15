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
    public class UserControl_Bordered_ResultTypeJSONProperty : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_ResultTypeJSONProperty
        (
            int? resultTypeJSONPropertyId, 
            string resultTypeJSONPropertyJSONProperty, 
            int? resultTypeJSONPropertyDataType,
            int resultTypeId, 
            bool enableEditing
        )
        {
            CreateControls(resultTypeJSONPropertyId, resultTypeJSONPropertyJSONProperty, resultTypeJSONPropertyDataType, resultTypeId, enableEditing);
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
            int resultTypeId, 
            bool enableEditing
        )
        {
            m_userControl_resultTypeJSONProperty = 
                new UserControl_ResultTypeJSONProperty
                (
                    resultTypeJSONPropertyId, 
                    resultTypeJSONPropertyJSONProperty,
                    resultTypeJSONPropertyDataType,
                    resultTypeId, 
                    enableEditing
                );
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_resultTypeJSONProperty };
            Content = border;
        }

        #endregion

        #endregion
    }
}
