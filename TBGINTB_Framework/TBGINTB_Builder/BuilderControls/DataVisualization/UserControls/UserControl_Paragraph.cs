using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TBGINTB_Builder.BuilderControls;
using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.HelperControls;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class UserControl_Paragraph : UserControl_Gettable, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        TextBox m_textBox_order;

        #endregion


        #region MEMBER PROPERTIES

        public int? ParagraphId { get; private set; }
        public int? ParagraphOrder { get; private set; }
        public int RoomId { get; private set; }
        public int? RoomStateId { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_order
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Paragraph(int? paragraphId, int? paragraphOrder, int roomId, int? roomStateId, bool enableEditing)
        {
            ParagraphId = paragraphId;
            ParagraphOrder = paragraphOrder;
            RoomStateId = roomStateId;
            RoomId = roomId;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.ParagraphModified += GinTubBuilderManager_ParagraphModified;
            GinTubBuilderManager.ParagraphGet += GinTubBuilderManager_ParagraphGet;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.ParagraphModified -= GinTubBuilderManager_ParagraphModified;
            GinTubBuilderManager.ParagraphGet -= GinTubBuilderManager_ParagraphGet;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
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
                    Text = (ParagraphId.HasValue) ? ParagraphId.ToString() : "NewParagraph"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Order Grid
            Grid grid_order = new Grid();
            grid_order.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_order.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_order, 1, 0);

            ////////
            // Order
            m_textBox_order = new TextBox() { VerticalAlignment = VerticalAlignment.Center };
            m_textBox_order.TextChanged += TextBox_Order_TextChanged;
            m_textBox_order.Text = (ParagraphOrder.HasValue) ? ParagraphOrder.ToString() : "0";
            Label label_order = new Label() { Content = "Order:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_order.SetGridRowColumn(m_textBox_order, 0, 1);
            grid_order.SetGridRowColumn(label_order, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_ParagraphModified(object sender, GinTubBuilderManager.ParagraphModifiedEventArgs args)
        {
            if (args.Id == ParagraphId)
            {
                SetParagraphOrder(args.Order);
                RoomStateId = args.RoomState;
                RoomId = args.Room;
            }
        }

        private void GinTubBuilderManager_ParagraphGet(object sender, GinTubBuilderManager.ParagraphGetEventArgs args)
        {
            SetGettableBackground(ParagraphId == args.Id);
        }

        private void SetParagraphOrder(int order)
        {
            ParagraphOrder = order;
            m_textBox_order.Text = ParagraphOrder.ToString();
        }

        private void TextBox_Order_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int newOrder = 0;
            if (tb != null && tb == m_textBox_order && int.TryParse(m_textBox_order.Text, out newOrder))
                ParagraphOrder = newOrder;
        }

        #endregion

        #endregion

    }
}
