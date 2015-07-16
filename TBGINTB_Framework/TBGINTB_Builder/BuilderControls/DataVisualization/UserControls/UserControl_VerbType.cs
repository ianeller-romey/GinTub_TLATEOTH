using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_VerbType : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Grid m_grid_main;
        ComboBox_VerbType m_comboBox_verbType;
        UserControl_Verbs m_userControl_verb;

        private bool m_enableEditing;

        #endregion


        #region MEMBER PROPERTIES

        public int? SelectedVerbTypeId { get; private set; }
        public string SelectedVerbTypeName { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_verbType,
                    m_userControl_verb
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_VerbType(bool enableEditing)
        {
            m_enableEditing = enableEditing;

            CreateControls();

            m_comboBox_verbType.IsEnabled = m_enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            //m_comboBox_verbType.SetActiveAndRegisterForGinTubEvents();
            //m_itemsControl_verb.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            //m_comboBox_verbType.SetInactiveAndUnregisterFromGinTubEvents();
            //m_itemsControl_verb.SetInactiveAndUnregisterFromGinTubEvents();
        }
        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            m_grid_main = new Grid();
            m_grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50.0, GridUnitType.Star) });
            m_grid_main.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50.0, GridUnitType.Star) });

            m_comboBox_verbType = 
                new ComboBox_VerbType() 
                { 
                    VerticalAlignment = System.Windows.VerticalAlignment.Top 
                };
            m_comboBox_verbType.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_verbType.SelectionChanged += ComboBox_VerbType_SelectionChanged;
            m_grid_main.SetGridRowColumn(m_comboBox_verbType, 0, 0);

            ////////
            // Fin
            Content = m_grid_main;
        }

        private void ComboBox_VerbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_VerbType comboBox = sender as ComboBox_VerbType;
            ComboBox_VerbType.ComboBoxItem_VerbType comboBoxItem = null;
            if (comboBox != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_VerbType.ComboBoxItem_VerbType) != null)
            {
                SelectedVerbTypeId = comboBoxItem.VerbTypeId;
                SelectedVerbTypeName = comboBoxItem.VerbTypeName;

                if (m_userControl_verb != null)
                    m_grid_main.Children.Remove(m_userControl_verb);

                m_userControl_verb = new UserControl_Verbs(SelectedVerbTypeId.Value);
                m_userControl_verb.IsEnabled = m_enableEditing;
                m_userControl_verb.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
                m_grid_main.SetGridRowColumn(m_userControl_verb, 0, 1);

                GinTubBuilderManager.ReadAllVerbsForVerbType(SelectedVerbTypeId.Value);
            }
            else if (comboBoxItem == null)
            {
                SelectedVerbTypeId = null;
                SelectedVerbTypeName = null;
            }
        }

        #endregion

        #endregion
    }
}
