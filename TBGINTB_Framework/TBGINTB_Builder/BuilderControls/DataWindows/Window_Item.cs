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
    public class Window_Item : Window_TaskOnAccept
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

        public Window_Item(int? itemId, string itemName, string itemDescription, TaskOnAccept task) :
            base("Item Data", task)
        {
            Width = 300;
            Height = 300;
            Content = CreateControls(itemId, itemName, itemDescription);
            m_userControl_item.SetActiveAndRegisterForGinTubEvents();
        }

        #endregion


        #region Private Functionality

        private UIElement CreateControls(int? itemId, string itemName, string itemDescription)
        {
            m_userControl_item = new UserControl_Item(itemId, itemName, itemDescription, true);
            return m_userControl_item;
        }

        #endregion

        #endregion
    }
}
