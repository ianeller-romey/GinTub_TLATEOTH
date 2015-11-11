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
    public class UserControl_Area : UserControl
    {
        #region MEMBER FIELDS

        TextBox m_textBox_name;
        CheckBox m_checkBox_displayTime;
        TextBox m_textBox_audio;

        #endregion


        #region MEMBER PROPERTIES

        public int? AreaId { get; private set; }
        public string AreaName { get; private set; }
        public bool AreaDisplayTime { get; private set; }
        public int? AreaAudio { get; private set; }

        public List<UIElement> EditingControls
        {
            get
            {
                return new List<UIElement>
                {
                    m_textBox_name,
                    m_checkBox_displayTime,
                    m_textBox_audio
                };
            }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_Area(int? areaId, string areaName, bool? areaDisplayTime, int? areaAudio, bool enableEditing)
        {
            AreaId = areaId;
            AreaName = areaName;
            AreaDisplayTime = (areaDisplayTime.HasValue) ? areaDisplayTime.Value : true; // default to displaying time
            AreaAudio = areaAudio;

            CreateControls();

            foreach (var e in EditingControls)
                e.IsEnabled = enableEditing;
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.AreaUpdated += GinTubBuilderManager_AreaUpdated;
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.AreaUpdated -= GinTubBuilderManager_AreaUpdated;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_main = new Grid();
            grid_main.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
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
                    Text = (AreaId.HasValue) ? AreaId.ToString() : "NewArea"
                };
            Label label_id = new Label() { Content = "Id:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_id.SetGridRowColumn(textBlock_id, 0, 1);
            grid_id.SetGridRowColumn(label_id, 0, 0);

            ////////
            // Name Grid
            Grid grid_name = new Grid();
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_name.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_name, 1, 0);

            ////////
            // Name
            m_textBox_name = new TextBox();
            m_textBox_name.TextChanged += TextBox_Name_TextChanged;
            Label label_name = new Label() { Content = "Name:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_name.SetGridRowColumn(m_textBox_name, 1, 0);
            grid_name.SetGridRowColumn(label_name, 0, 0);

            ////////
            // DisplayTime Grid
            Grid grid_displayTime = new Grid();
            grid_displayTime.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_displayTime.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_main.SetGridRowColumn(grid_displayTime, 2, 0);

            ////////
            // DisplayTime
            m_checkBox_displayTime = new CheckBox() { IsChecked = AreaDisplayTime, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            m_checkBox_displayTime.Checked += CheckBox_DisplayTime_Checked;
            Label label_displayTime = new Label() { Content = "Display Time?", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_displayTime.SetGridRowColumn(m_checkBox_displayTime, 0, 1);
            grid_displayTime.SetGridRowColumn(label_displayTime, 0, 0);

            ////////
            // Audio Grid
            Grid grid_audio = new Grid();
            grid_audio.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_audio.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100.0, GridUnitType.Star) });
            grid_main.SetGridRowColumn(grid_audio, 3, 0);

            ////////
            // Audio
            m_textBox_audio = new TextBox();
            m_textBox_audio.TextChanged += TextBox_Name_TextChanged;
            Label label_audio = new Label() { Content = "Audio:", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            grid_audio.SetGridRowColumn(m_textBox_audio, 1, 0);
            grid_audio.SetGridRowColumn(label_audio, 0, 0);

            ////////
            // Fin
            Content = grid_main;
        }

        private void GinTubBuilderManager_AreaUpdated(object sender, GinTubBuilderManager.AreaUpdatedEventArgs args)
        {
            if (AreaId == args.Id)
            {
                SetAreaName(args.Name);
                SetAreaDisplayTime(args.DisplayTime);
                SetAreaAudio(args.Audio);
            }
        }

        private void SetAreaName(string areaName)
        {
            m_textBox_name.Text = areaName;
            if (!m_textBox_name.IsEnabled)
                TextBox_Name_TextChanged(m_textBox_name, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void SetAreaDisplayTime(bool areaDisplayTime)
        {
            m_checkBox_displayTime.IsChecked = areaDisplayTime;
            if (!m_checkBox_displayTime.IsEnabled)
                CheckBox_DisplayTime_Checked(m_checkBox_displayTime, new RoutedEventArgs(CheckBox.CheckedEvent));
        }

        private void SetAreaAudio(int? areaAudio)
        {
            m_textBox_audio.Text = areaAudio.ToString();
            if (!m_textBox_audio.IsEnabled)
                TextBox_Audio_TextChanged(m_textBox_audio, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Undo));
        }

        private void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_name)
                AreaName = m_textBox_name.Text;
        }

        private void CheckBox_DisplayTime_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb != null && cb == m_checkBox_displayTime)
            {
                AreaDisplayTime = m_checkBox_displayTime.IsChecked.Value;
            }
        }

        private void TextBox_Audio_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb == m_textBox_audio)
            {
                if (string.IsNullOrWhiteSpace(m_textBox_audio.Text))
                    AreaAudio = null;
                else
                {
                    int audioId;
                    if (int.TryParse(m_textBox_audio.Text, out audioId))
                        AreaAudio = audioId;
                }
            }
        }

        #endregion

        #endregion
    }
}
