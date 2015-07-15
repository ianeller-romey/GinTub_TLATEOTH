using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_ItemActionRequirement : UserControl
    {
        #region MEMBER FIELDS

        ComboBox_Item m_comboBox_item;
        ComboBox_Action m_comboBox_action;

        #endregion


        #region MEMBER PROPERTIES

        public int? ItemActionRequirementId { get; private set; }
        public int? ItemActionRequirementItem { get; private set; }
        public int? ItemActionRequirementAction { get; private set; }
        private int NounId { get; set; }
        private int ParagraphStateId { get; set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_comboBox_item,
                    m_comboBox_action
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_ItemActionRequirement(int? itemActionRequirementId, int? itemActionRequirementItem, int? itemActionRequirementAction, int nounId, int paragraphStateId, bool enableEditing)
        {
            ItemActionRequirementId = itemActionRequirementId;
            ItemActionRequirementItem = itemActionRequirementItem;
            ItemActionRequirementAction = itemActionRequirementAction;
            NounId = nounId;
            ParagraphStateId = paragraphStateId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;

            GinTubBuilderManager.ItemAdded += GinTubBuilderManager_ItemAdded;
            GinTubBuilderManager.ActionAdded += GinTubBuilderManager_ActionAdded;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ItemActionRequirementModified += GinTubBuilderManager_ItemActionRequirementModified;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ItemActionRequirementModified -= GinTubBuilderManager_ItemActionRequirementModified;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            
            ////////
            // Id Grid
            Grid grid_id = new Grid();
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_id.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_id, 0, 0);

            ////////
            // Id
            TextBlock textBlock_id =
                new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = (ItemActionRequirementId.HasValue) ? ItemActionRequirementId.ToString() : "NewItemActionRequirement"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Item Grid
            Grid grid_item = new Grid();
            grid_item.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_item.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_item, 1, 0);

            ////////
            // Item
            m_comboBox_item = new ComboBox_Item();
            m_comboBox_item.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_item.SelectionChanged += ComboBox_Item_SelectionChanged;
            Label label_item = new Label() { Content = "Item:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_item.SetGridRowColumn(m_comboBox_item, 1, 0);
            grid_item.SetGridRowColumn(label_item, 0, 0);

            ////////
            // Action Grid
            Grid grid_action = new Grid();
            grid_action.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_action.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_action, 2, 0);

            ////////
            // Action
            m_comboBox_action = new ComboBox_Action(NounId, ParagraphStateId);
            m_comboBox_action.SetActiveAndRegisterForGinTubEvents(); // never unregister; we want updates no matter where we are
            m_comboBox_action.SelectionChanged += ComboBox_Action_SelectionChanged;
            Label label_action = new Label() { Content = "Action:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_action.SetGridRowColumn(m_comboBox_action, 1, 0);
            grid_action.SetGridRowColumn(label_action, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        void GinTubBuilderManager_ItemActionRequirementModified(object sender, GinTubBuilderManager.ItemActionRequirementModifiedEventArgs args)
        {
            if(ItemActionRequirementId == args.Id)
            {
                SetItemActionRequirementItem(args.Item);
                SetItemActionRequirementAction(args.Action);
            }
        }

        void GinTubBuilderManager_ItemAdded(object sender, GinTubBuilderManager.ItemAddedEventArgs args)
        {
            ResetItemActionRequirementItem(args.Id);
        }
        
        void GinTubBuilderManager_ActionAdded(object sender, GinTubBuilderManager.ActionAddedEventArgs args)
        {
            if (NounId == args.Noun)
                ResetItemActionRequirementAction(args.Id);
        }

        private void SetItemActionRequirementItem(int itemActionRequirementItem)
        {
            ComboBox_Item.ComboBoxItem_Item item =
                m_comboBox_item.Items.OfType<ComboBox_Item.ComboBoxItem_Item>().
                SingleOrDefault(i => i.ItemId == itemActionRequirementItem);
            if (item != null)
                m_comboBox_item.SelectedItem = item;
        }

        private void SetItemActionRequirementAction(int itemActionRequirementAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => i.ActionId == itemActionRequirementAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        private void ResetItemActionRequirementItem(int itemActionRequirementItem)
        {
            ComboBox_Item.ComboBoxItem_Item item =
                m_comboBox_item.Items.OfType<ComboBox_Item.ComboBoxItem_Item>().
                SingleOrDefault(i => ItemActionRequirementItem.HasValue && ItemActionRequirementItem.Value == itemActionRequirementItem && i.ItemId == itemActionRequirementItem);
            if (item != null)
                m_comboBox_item.SelectedItem = item;
        }

        private void ResetItemActionRequirementAction(int itemActionRequirementAction)
        {
            ComboBox_Action.ComboBoxItem_Action item = m_comboBox_action.Items.OfType<ComboBox_Action.ComboBoxItem_Action>().
                SingleOrDefault(i => ItemActionRequirementAction.HasValue && ItemActionRequirementAction.Value == itemActionRequirementAction && i.ActionId == itemActionRequirementAction);
            if (item != null)
                m_comboBox_action.SelectedItem = item;
        }

        void ComboBox_Item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Item.ComboBoxItem_Item item;
            if (m_comboBox_item.SelectedItem != null && (item = m_comboBox_item.SelectedItem as ComboBox_Item.ComboBoxItem_Item) != null)
                ItemActionRequirementItem = item.ItemId;
        }

        void ComboBox_Action_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Action.ComboBoxItem_Action item;
            if (m_comboBox_action.SelectedItem != null && (item = m_comboBox_action.SelectedItem as ComboBox_Action.ComboBoxItem_Action) != null)
                ItemActionRequirementAction = item.ActionId;
        }

        #endregion

        #endregion
    }
}
