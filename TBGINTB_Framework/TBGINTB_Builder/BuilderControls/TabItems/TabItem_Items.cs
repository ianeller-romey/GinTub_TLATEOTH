using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class TabItem_Items : TabItem, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        Grid m_grid_main;
        ComboBox_Item m_comboBox_item;
        UserControl_ItemModification m_userControl_item;

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public TabItem_Items()
        {
            Header = "Items";
            Content = CreateControls();

            GinTubBuilderManager.ReadAllItems();
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            if(m_userControl_item != null)
                m_userControl_item.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            if (m_userControl_item != null)
                m_userControl_item.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls()
        {
            m_grid_main = new Grid();
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            m_grid_main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });

            m_comboBox_item = new ComboBox_Item();
            m_comboBox_item.SetActiveAndRegisterForGinTubEvents();
            m_comboBox_item.SelectionChanged += ComboBox_Item_SelectionChanged;
            m_grid_main.SetGridRowColumn(m_comboBox_item, 0, 0);

            return m_grid_main;
        }

        private void ComboBox_Item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Item comboBox = sender as ComboBox_Item;
            ComboBox_Item.ComboBoxItem_Item comboBoxItem;
            if(comboBox.SelectedItem != null && (comboBoxItem = comboBox.SelectedItem as ComboBox_Item.ComboBoxItem_Item) != null)
            {
                if (m_userControl_item != null)
                    m_grid_main.Children.Remove(m_userControl_item);
                m_userControl_item = new UserControl_ItemModification(comboBoxItem.ItemId, comboBoxItem.ItemName, comboBoxItem.ItemDescription);
                m_userControl_item.SetActiveAndRegisterForGinTubEvents();
                m_grid_main.SetGridRowColumn(m_userControl_item, 1, 0);
            }
        }

        #endregion

        #endregion

    }
}
