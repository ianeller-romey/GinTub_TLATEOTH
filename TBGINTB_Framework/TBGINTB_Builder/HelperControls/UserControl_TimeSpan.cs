using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TBGINTB_Builder.Extensions;


namespace TBGINTB_Builder.HelperControls
{
    public class UserControl_TimeSpan : UserControl
    {
        #region MEMBER FIELDS

        private const int c_minuteIncr = 5;

        ComboBox
            m_comboBox_time_hour,
            m_comboBox_time_minute;

        #endregion


        #region MEMBER EVENTS

        public delegate void TimeChangedEventHandler(TimeSpan time);

        public event TimeChangedEventHandler TimeChangedEvent;

        #endregion


        #region MEMBER PROPERTIES

        public int? Hour { get; private set; }
        public int? Minute { get; private set; }
        public TimeSpan? Time
        {
            get { return (Hour.HasValue && Minute.HasValue) ? new TimeSpan?(new TimeSpan(Hour.Value, Minute.Value, 0)) : null; }
        }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public UserControl_TimeSpan(TimeSpan? time)
        {
            CreateControls();

            SetTime(time);
        }

        public void SetTime(TimeSpan? time)
        {
            if (time.HasValue)
                SetTime(time.Value.Hours, time.Value.Minutes);
            else
                ResetTime();
        }

        public void SetTime(int hour, int minute)
        {
            m_comboBox_time_hour.SelectedItem = m_comboBox_time_hour.Items.OfType<string>().SingleOrDefault(h => int.Parse(h) == hour);
            m_comboBox_time_minute.SelectedItem = m_comboBox_time_minute.Items.OfType<string>().SingleOrDefault(m => int.Parse(m) == minute);
        }

        public void ResetTime()
        {
            m_comboBox_time_hour.SelectedItem = null;
            m_comboBox_time_minute.SelectedItem = null;
        }

        #endregion


        #region Private Functionality

        private void CreateControls()
        {
            Grid grid_time = new Grid() { HorizontalAlignment = HorizontalAlignment.Center };
            grid_time.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_time.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_time.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_time.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid_time.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            Label label_time = new Label() { Content = "Time: ", FontWeight = FontWeights.Bold };
            Grid.SetColumnSpan(label_time, 3);
            grid_time.SetGridRowColumn(label_time, 0, 0);

            m_comboBox_time_hour = new ComboBox();
            for (int i = 0; i <= 24; ++i)
                m_comboBox_time_hour.Items.Add(string.Format("{0:00}", i));
            grid_time.SetGridRowColumn(m_comboBox_time_hour, 1, 0);

            Label label_colon = new Label() { Content = " : " };
            grid_time.SetGridRowColumn(label_colon, 1, 1);

            m_comboBox_time_minute = new ComboBox();
            for (int i = 0; i < 60; i += c_minuteIncr)
                m_comboBox_time_minute.Items.Add(string.Format("{0:00}", i));
            grid_time.SetGridRowColumn(m_comboBox_time_minute, 1, 2);

            Content = grid_time;
        }

        private void RaiseTimeChangedEvent()
        {
            var time = Time;
            if (TimeChangedEvent != null && time.HasValue)
                TimeChangedEvent(time.Value);
        }

        private void ComboBox_Time_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (sender == m_comboBox_time_hour || sender == m_comboBox_time_minute)
            {
                Hour = (m_comboBox_time_minute.SelectedItem != null) ? new Nullable<int>(int.Parse(m_comboBox_time_hour.SelectedItem.ToString())) : null;
                Minute = (m_comboBox_time_minute.SelectedItem != null) ? new Nullable<int>(int.Parse(m_comboBox_time_minute.SelectedItem.ToString())) : null;
                RaiseTimeChangedEvent();
            }
        }

        #endregion

        #endregion
    }
}
