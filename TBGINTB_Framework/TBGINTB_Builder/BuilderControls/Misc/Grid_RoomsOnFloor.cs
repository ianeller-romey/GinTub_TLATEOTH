using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using TBGINTB_Builder.Extensions;
using TBGINTB_Builder.Lib;


namespace TBGINTB_Builder.BuilderControls
{
    public class Grid_RoomsOnFloor : Grid, IRegisterGinTubEventsOnlyWhenActive
    {
        #region MEMBER FIELDS

        private const int c_defaultRowsCols = 10;
        private const int c_defaultRowsColsModifier = c_defaultRowsCols / 2;
        private const double c_defaultRowColWH = 40.0;
        private static readonly GridLength s_defaultRowColGridLength = new GridLength(c_defaultRowColWH, GridUnitType.Pixel);

        #endregion


        #region MEMBER PROPERTIES

        public int RoomsMaxX { get; private set; }
        public int RoomsMaxY { get; private set; }
        public int RoomsZ { get; private set; }
        public int AreaId { get; private set; }

        #endregion


        #region MEMBER METHODS

        #region Public Functionality

        public Grid_RoomsOnFloor(int maxX, int maxY, int roomsZ, int areaId)
        {
            RoomsMaxX = maxX;
            RoomsMaxY = maxY;
            AreaId = areaId;
            CreateControls(RoomsMaxX, RoomsMaxY);

            SetFloor(roomsZ);
        }

        public void SetActiveAndRegisterForGinTubEvents()
        {
            GinTubBuilderManager.RoomAdded += GinTubBuilderManager_RoomAdded;

            foreach (var i in Children.OfType<IRegisterGinTubEventsOnlyWhenActive>())
                i.SetActiveAndRegisterForGinTubEvents();
        }

        public void SetInactiveAndUnregisterFromGinTubEvents()
        {
            GinTubBuilderManager.RoomAdded -= GinTubBuilderManager_RoomAdded;

            foreach (var i in Children.OfType<IRegisterGinTubEventsOnlyWhenActive>())
                i.SetInactiveAndUnregisterFromGinTubEvents();
        }

        public void SetFloor(int z)
        {
            RoomsZ = z;
            foreach (var button in Children.OfType<Button_RoomOnFloor>())
            {
                button.HasNoRoom();
                button.SetFloor(RoomsZ);
            }
            GinTubBuilderManager.LoadAllRoomsInAreaOnFloor(AreaId, RoomsZ);
        }

        #endregion


        #region Private Functionality

        private void CreateControls(int maxX, int maxY)
        {
            for (int x = 0, xx = maxX + c_defaultRowsColsModifier; x < xx; ++x)
                AddColumn(false);
            for (int y = 0, yy = maxY + c_defaultRowsColsModifier; y < yy; ++y)
                AddRow(true);
        }

        private void AddRow(bool addCell)
        {
            RowDefinitions.Add(new RowDefinition() { Height = s_defaultRowColGridLength });
            if (addCell)
            {
                for (int i = 0; i < ColumnDefinitions.Count; ++i)
                    AddCell(RowDefinitions.Count - 1, i);
            }
        }

        private void AddColumn(bool addCell)
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = s_defaultRowColGridLength });
            if (addCell)
            {
                for (int i = 0; i < RowDefinitions.Count; ++i)
                    AddCell(i, ColumnDefinitions.Count - 1);
            }
        }

        private void AddCell(int row, int column)
        {
            Rectangle rect = new Rectangle() { Stroke = Brushes.Black, StrokeThickness = 1.0 };
            this.SetGridRowColumn(rect, row, column);

            Button_RoomOnFloor button = new Button_RoomOnFloor(column, row, RoomsZ, AreaId);
            this.SetGridRowColumn(button, row, column);
            button.SetActiveAndRegisterForGinTubEvents();
        }

        private void AddRoom(int roomX, int roomY)
        {
            for(int xPlusModifier = roomX + c_defaultRowsColsModifier; RoomsMaxX + c_defaultRowsColsModifier < xPlusModifier; RoomsMaxX = RoomsMaxX + 1)
                AddColumn(true);
            for (int yPlusModifier = roomY + c_defaultRowsColsModifier; RoomsMaxY + c_defaultRowsColsModifier < yPlusModifier; RoomsMaxY = RoomsMaxY + 1)
                AddRow(true);
        }

        private void GinTubBuilderManager_RoomAdded(object sender, GinTubBuilderManager.RoomAddedEventArgs args)
        {
            if (args.Area == AreaId && args.Z == RoomsZ)
                AddRoom(args.X, args.Y);
        }

        #endregion

        #endregion
    }
}
