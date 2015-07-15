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
    public class UserControl_ItemModification : UserControl, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        UserControl_Item m_userControl_item;

        #endregion


        #region MEMBER PROPERTIES

        public int? ItemId { get { return m_userControl_item.ItemId; } }
        public string ItemName { get { return m_userControl_item.ItemName; } }
        public string ItemDescription { get { return m_userControl_item.ItemDescription; } }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ItemModification(int? itemId, string itemName, string itemDescription)
        {
            CreateControls(itemId, itemName, itemDescription);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            m_userControl_item.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            m_userControl_item.SetInactiveAndUnregisterFromGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int? itemId, string itemName, string itemDescription)
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button_modifyItem = new Button() { Content = "Modify Item" };
            button_modifyItem.Click += Button_ModifyItem_Click;
            grid_main.SetGridRowColumn(button_modifyItem, 0, 0);

            m_userControl_item = new UserControl_Item(itemId, itemName, itemDescription, false);
            grid_main.SetGridRowColumn(m_userControl_item, 1, 0);
            m_userControl_item.SetActiveAndRegisterForGinTubEvents();

            Border border = new Border() { Style = new Style_DefaultBorder(), Child = grid_main };
            Content = border;
        }

        private void Button_ModifyItem_Click(object sender, RoutedEventArgs e)
        {
            Window_Item window =
                new Window_Item
                (
                    m_userControl_item.ItemId, 
                    m_userControl_item.ItemName, 
                    m_userControl_item.ItemDescription, 
                    (win) =>
                    {
                        Window_Item wWin = win as Window_Item;
                        if(wWin != null)
                            GinTubBuilderManager.ModifyItem(wWin.ItemId.Value, wWin.ItemName, wWin.ItemDescription);
                    }
                );
            window.Show();
        }

        #endregion

        #endregion
    }
}
