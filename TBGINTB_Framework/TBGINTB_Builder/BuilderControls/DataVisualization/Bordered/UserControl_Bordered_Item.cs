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
    public class UserControl_Bordered_Item : UserControl, IRegisterGinTubEventsOnlyWhenActive
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

        public UserControl_Bordered_Item(int? itemId, string itemName, string itemDescription, bool enableEditing)
        {
            CreateControls(itemId, itemName, itemDescription, enableEditing);
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

        private void CreateControls(int? itemId, string itemName, string itemDescription, bool enableEditing)
        {
            m_userControl_item = new UserControl_Item(itemId, itemName, itemDescription, enableEditing);
            Border border = new Border() { Style = new Style_DefaultBorder(), Child = m_userControl_item };
            Content = border;
        }

        #endregion

        #endregion
    }
}
