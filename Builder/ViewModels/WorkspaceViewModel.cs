﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Builder.Commands;
using Builder.ViewModels.WorkspaceElements;
using Shared.ViewModels;
using Shared.Commands;



namespace Builder.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase
    {
        public BuilderViewModel Builder { get; set; }

        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeaveCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand MouseWheelCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public ICommand RenderSizeChangedCommand { get; }

        private double _visibleWidth; //Border actual width
        public double VisibleWidth
        {
            get => _visibleWidth;
            set { _visibleWidth = value; OnPropertyChanged(nameof(VisibleWidth)); }
        }

        private double _visibleHeight; //Border actual height
        public double VisibleHeight
        {
            get => _visibleHeight;
            set { _visibleHeight = value; OnPropertyChanged(nameof(VisibleHeight)); }
        }

        private int _width = 10000; //Canvas width
        public int Width
        {
            get => _width;
            set { _width = value; OnPropertyChanged(nameof(Width)); }
        }

        private int _height = 10000; //Canvas height
        public int Height
        {
            get => _height;
            set { _height = value; OnPropertyChanged(nameof(Height)); }
        }

        private double _zoomFactor = 1.0;
        public double ZoomFactor
        {
            get => _zoomFactor;
            set { _zoomFactor = value; OnPropertyChanged(nameof(ZoomFactor)); }
        }

        private Point _canvasPanOffset;
        public Point CanvasPanOffset
        {
            get => _canvasPanOffset;
            set { _canvasPanOffset = value; OnPropertyChanged(nameof(CanvasPanOffset)); }
        }

        private Point mouseOffset; // The offset of the mouse to the top left corner of the rectangle

        //For panning
        private bool _isPanning;
        private Point _panStart;

        public ObservableCollection<WorkspaceItemViewModel> WorkspaceItems { get; set; } = new ObservableCollection<WorkspaceItemViewModel>();
        public ObservableCollection<WSConnectionViewModel> WorkspaceConnections { get; set; } = new ObservableCollection<WSConnectionViewModel>();

        private WorkspaceItemViewModel? _selectedItem;
        public WorkspaceItemViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != null)
                    _selectedItem.IsSelected = false;

                _selectedItem = value;

                if (_selectedItem != null)
                    _selectedItem.IsSelected = true;

                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        
		private WSConnectionViewModel? _selectedConnection;
		public WSConnectionViewModel? SelectedConnection
		{
			get => _selectedConnection;
			set
			{
				if (_selectedConnection != null)
					_selectedConnection.IsSelected = false;

				_selectedConnection = value;

				if (_selectedConnection != null)
				{
					_selectedConnection.IsSelected = true;
				}

				OnPropertyChanged(nameof(SelectedConnection));
			}
		}

        private WSConnectionViewModel? connectionInProgress;

		public WorkspaceViewModel(BuilderViewModel builderViewModel)
        {
            Builder = builderViewModel;

            CanvasPanOffset = new Point(0, 0);

            WorkspaceItems.Add(new WorkspaceItemViewModel(0, 0, 60, 60));
            WorkspaceItems.Add(new WorkspaceItemViewModel(105, 123, 50, 50));
            WorkspaceItems.Add(new WorkspaceItemViewModel(220, 330, 75, 75));

			WorkspaceConnections.Add(new WSConnectionViewModel(WorkspaceItems[0], WorkspaceItems[1]));
			WorkspaceConnections.Add(new WSConnectionViewModel(WorkspaceItems[1], WorkspaceItems[2]));

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            MouseLeaveCommand = new RelayCommand<MouseEventArgs>(e => _isPanning = false);
            DragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);

            MouseWheelCommand = new RelayCommand<MouseWheelEventArgs>(OnMouseWheel);
            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);

            RenderSizeChangedCommand = new RelayCommand<SizeChangedEventArgs>(OnRenderSizeChanged);
        }

        private void OnDragOver(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(WorkspaceItemViewModel)))
            {
                var draggedItem = e.Data.GetData(typeof(WorkspaceItemViewModel)) as WorkspaceItemViewModel;
                Point dropPosition = e.GetPosition(e.Source as IInputElement);
                if (draggedItem != null)
                {
                    dropPosition.X -= mouseOffset.X;
                    dropPosition.Y -= mouseOffset.Y;
                    draggedItem.Position = dropPosition;
                }
            }
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            Point mousePos;
            if (e.OriginalSource is Rectangle)
                mousePos = e.GetPosition(e.Source as IInputElement);
            else
                mousePos = e.GetPosition(e.OriginalSource as IInputElement);

            double oldZoom = ZoomFactor;
            double zoomDelta = e.Delta > 0 ? 0.1 : -0.1;

            double minZoom = Math.Max(VisibleWidth / Width, VisibleHeight / Height);
            double maxZoom = 3.0;

            double newZoom = Math.Max(minZoom, Math.Min(maxZoom, ZoomFactor + zoomDelta));
            ZoomFactor = newZoom;

            CanvasPanOffset = new Point(
                CanvasPanOffset.X - mousePos.X * (newZoom - oldZoom),
                CanvasPanOffset.Y - mousePos.Y * (newZoom - oldZoom)
            );

            CanvasPanOffset = ClipCanvasPan(CanvasPanOffset);
        }

        private void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
			var data = e.OriginalSource as FrameworkElement;
			if (e.OriginalSource is Image)
            {
		        SelectedConnection = null;
                if (data != null && data.DataContext is WorkspaceItemViewModel workspaceItem)
                {
                    SelectedItem = workspaceItem;
                    Builder.PropertiesWindowViewModel.SelectedWorkspaceItem = workspaceItem;

                    if (Builder.isMakingConnection)
                    {
                        connectionInProgress = new WSConnectionViewModel();
                        connectionInProgress.Source = workspaceItem;
                        connectionInProgress.CurrentMousePosition = e.GetPosition(e.Source as IInputElement);
                        WorkspaceConnections.Add(connectionInProgress);
                        return;
                    }

                }
                if (data != null && e.LeftButton == MouseButtonState.Pressed && data.DataContext is WorkspaceItemViewModel)
                {
                    mouseOffset = e.GetPosition(data); // mouse position relative to top-left of the rectangle
                    DragDrop.DoDragDrop(e.OriginalSource as UIElement, new DataObject(typeof(WorkspaceItemViewModel), data.DataContext), DragDropEffects.Move);
                }

            }
            else if(data != null && e.OriginalSource is Line && data.DataContext is WSConnectionViewModel connection)
            {
                
                SelectedItem = null;
                SelectedConnection = connection; 
                return;
                
			}
             else
             {
                _isPanning = true;
                _panStart = e.GetPosition(null);
                SelectedItem = null;
				SelectedConnection = null;
			}
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            if (connectionInProgress != null)
                connectionInProgress.CurrentMousePosition = e.GetPosition(e.Source as IInputElement);
            if (_isPanning)
            {
                Point currentPosition = e.GetPosition(null);
                Vector delta = currentPosition - _panStart;
                CanvasPanOffset = ClipCanvasPan(new Point(CanvasPanOffset.X + delta.X, CanvasPanOffset.Y + delta.Y));
                _panStart = currentPosition;
            }
        }

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _isPanning = false;

            if (Builder.WorkspaceItemSelected.Count > 0)
            {
                IInputElement? mouseEventOriginalSource = e.OriginalSource as IInputElement;
                if (e.OriginalSource is Canvas)
                {
                    Point mousePos = e.GetPosition(e.OriginalSource as IInputElement);
                    Builder.WorkspaceItemSelected[0].Opacity = 1;
                    Builder.WorkspaceItemSelected[0].Position = mousePos;
                    WorkspaceItems.Add(Builder.WorkspaceItemSelected[0]);
                }
            }
            if (connectionInProgress != null)
            {
                if (e.OriginalSource is Image)
                {
                    var data = e.OriginalSource as FrameworkElement;

                    if (data != null && data.DataContext is WorkspaceItemViewModel workspaceItem)
                    {
                        connectionInProgress.Target = workspaceItem;
                        WorkspaceConnections.Remove(connectionInProgress);
                        AddConnectionIfNotExists(connectionInProgress);
                        connectionInProgress = null;
                        return;
                    }
                }
                else
                    CancelConnectionInProgress(); 
            }
        }

        private void OnRenderSizeChanged(SizeChangedEventArgs e)
        {
            VisibleWidth = e.NewSize.Width;
            VisibleHeight = e.NewSize.Height;
            CanvasPanOffset = ClipCanvasPan(CanvasPanOffset);
        }

        private Point ClipCanvasPan(Point point)
        {
            double allowedX = Width * ZoomFactor - VisibleWidth;
            double allowedY = Height * ZoomFactor - VisibleHeight;

            double clampedX = Math.Max(Math.Min(point.X, 0), -allowedX);
            double clampedY = Math.Max(Math.Min(point.Y, 0), -allowedY);

            return new Point(clampedX, clampedY);
        }

        private void CancelConnectionInProgress()
        {
            if (connectionInProgress != null)
            {
                WorkspaceConnections.Remove(connectionInProgress);
                connectionInProgress = null;
            }
        }

        /// <summary>
        /// does deep copy of connection, checks if no such connections already exist or if the connection source and target is the same
        /// </summary>
        /// <param name="connection"></param>
        private void AddConnectionIfNotExists(WSConnectionViewModel connection)
        {
            if (connection.Source == connection.Target || connection == null)
                return;
            WSConnectionViewModel newConnection = new WSConnectionViewModel(connection.Source, connection.Target);
            if (WorkspaceConnections.Count == 0)
                WorkspaceConnections.Add(newConnection);
            else
            {
                foreach (var existingConnection in WorkspaceConnections)
                    if (newConnection.Equals(existingConnection))
                        return;

                WorkspaceConnections.Add(newConnection);
            }
        }

    }
}
