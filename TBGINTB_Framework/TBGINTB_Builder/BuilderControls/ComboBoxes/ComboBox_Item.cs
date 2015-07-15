using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class ComboBox_Item : ComboBox, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private readonly ComboBoxItem c_comboBoxItem_newItem = new ComboBoxItem() { Content = "New Item ..." };

        #endregion


        #region MEMBER PROPERTIES
        #endregion


        #region MEMBER CLASSES
    
        public class ComboBoxItem_Item : ComboBoxItem
        {
            #region MEMBER PROPERTIES

            public int ItemId { get; private set; }
            public string ItemName { get; private set; }
            public string ItemDescription { get; private set; }

            #endregion


            #region MEMBER METHODS

            #region Public Functionality

            public ComboBoxItem_Item(int itemId, string itemName, string itemDescription)
            {
                ItemId = itemId;
                SetItemName(itemName);
                SetItemDescription(itemDescription);
            }

            public void SetItemName(string itemName)
            {
                ItemName = itemName;
                Content = ItemName;
            }

            public void SetItemDescription(string itemDescription)
            {
                ItemDescription = itemDescription;
            }

            #endregion

            #endregion
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public ComboBox_Item()
        {
            Items.Add(c_comboBoxItem_newItem);

            SelectionChanged += ComboBox_Item_SelectionChanged;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ItemAdded += GinTubBuilderManager_ItemAdded;
            GinTubBuilderManager.ItemModified += GinTubBuilderManager_ItemModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ItemAdded -= GinTubBuilderManager_ItemAdded;
            GinTubBuilderManager.ItemModified -= GinTubBuilderManager_ItemModified;
        }

        #endregion


        #region Private Functionality

        private void GinTubBuilderManager_ItemAdded(object sender, GinTubBuilderManager.ItemAddedEventArgs args)
        {
            if (!Items.OfType<ComboBoxItem_Item>().Any(i => i.ItemId == args.Id))
                Items.Add(new ComboBoxItem_Item(args.Id, args.Name, args.Description));
        }

        private void GinTubBuilderManager_ItemModified(object sender, GinTubBuilderManager.ItemModifiedEventArgs args)
        {
            ComboBoxItem_Item item = Items.OfType<ComboBoxItem_Item>().SingleOrDefault(i => i.ItemId == args.Id);
            if (item != null)
            {
                item.SetItemName(args.Name);
                item.SetItemDescription(args.Description);
            }
        }

        private void NewItemDialog()
        {
            Window_Item window = 
                new Window_Item
                (
                    null, 
                    null,
                    null,
                    (win) =>
                    {
                        Window_Item wWin = win as Window_Item;
                        if (wWin != null)
                            GinTubBuilderManager.AddItem(wWin.ItemName, wWin.ItemDescription);
                    }
                );
            window.Show();
        }

        private void ComboBox_Item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = null;
            if ((item = SelectedItem as ComboBoxItem) != null)
            {
                if (item == c_comboBoxItem_newItem)
                    NewItemDialog();
            }
        }

        #endregion

        #endregion
    }
}
