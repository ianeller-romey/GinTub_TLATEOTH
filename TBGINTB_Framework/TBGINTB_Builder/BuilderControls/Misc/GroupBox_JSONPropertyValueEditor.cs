using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class GroupBox_JSONPropertyValueEditor : GroupBox
    {
        #region MEMBER FIELDS

        TextBox m_textBox_propertyValue;

        #endregion


        #region MEMBER PROPERTIES

        public string JSONPropertyName { get; private set; }
        public string JSONPropertyValue { get; private set; }
        public int JSONPropertyDataTypeId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public GroupBox_JSONPropertyValueEditor(string jsonPropertyName, string jsonPropertyValue, int jsonPropertyDataTypeId)
        {
            JSONPropertyName = jsonPropertyName;
            JSONPropertyValue = jsonPropertyValue;
            JSONPropertyDataTypeId = jsonPropertyDataTypeId;

            CreateControls();
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Header = JSONPropertyName;

            m_textBox_propertyValue = new TextBox();
            m_textBox_propertyValue.TextChanged += TextBox_PropertyValue_TextChanged;
            m_textBox_propertyValue.Text = JSONPropertyValue;
            Content = m_textBox_propertyValue;
        }

        void TextBox_PropertyValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox == m_textBox_propertyValue)
                JSONPropertyValue = m_textBox_propertyValue.Text;
        }

        #endregion

        #endregion

    }
}
