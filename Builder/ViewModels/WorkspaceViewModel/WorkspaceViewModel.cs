using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Builder.ViewModels.WorkspaceElements;
using Shared.Commands;
using Shared.ViewModels;



namespace Builder.ViewModels
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
        public WorkspaceViewModel(BuilderViewModel builderViewModel)
        {
            Builder = builderViewModel;

            WorkspaceItems.Add(new WSConvolutionViewModel(3, 64, 3, 2, x: 50020, y: 50200, name: "conv1"));
            WorkspaceItems.Add(new WSPoolingViewModel(3, 2, x: 50120, y: 50200, name: "pool1"));
            WorkspaceItems.Add(new WSFullyConnectedViewModel(256, 512, x: 50220, y: 50200, name: "fc1"));

            WorkspaceConnections.Add(new WSConnectionViewModel(WorkspaceItems[0], WorkspaceItems[1]));
            WorkspaceConnections.Add(new WSConnectionViewModel(WorkspaceItems[1], WorkspaceItems[2]));

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            DragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);

            MouseWheelCommand = new RelayCommand<MouseWheelEventArgs>(OnMouseWheel);
            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);

            RenderSizeChangedCommand = new RelayCommand<SizeChangedEventArgs>(OnRenderSizeChanged);
            DeleteKeyDownCommand = new RelayCommand<KeyEventArgs>(OnDeleteKeyDown);
            ControlKeyDownCommand = new RelayCommand<KeyEventArgs>(ControlKeyDown);
            CanvasPanOffset = new Point(-Width / 2, -Height / 2);
        }
        public WorkspaceViewModel() { }

        public BuilderViewModel Builder { get; set; }


        public IInputElement _selectionStartReference;
        public ICommand RenderSizeChangedCommand { get; }
        public ICommand DeleteKeyDownCommand { get; }
        public ICommand ControlKeyDownCommand { get; }

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

        private int _width = 100000; //Canvas width
        public int Width
        {
            get => _width;
            set { _width = value; OnPropertyChanged(nameof(Width)); }
        }

        private int _height = 100000; //Canvas height
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

        //for drag and drop
        private WorkspaceItemViewModel? draggedItem;

        //For panning
        private bool _isPanning;
        private Point _panStart;

        public ObservableCollection<WorkspaceItemViewModel> WorkspaceItems { get; set; } = new ObservableCollection<WorkspaceItemViewModel>();
        public ObservableCollection<WSConnectionViewModel> WorkspaceConnections { get; set; } = new ObservableCollection<WSConnectionViewModel>();
        public ObservableCollection<WorkspaceItemViewModel> SelectedItems { get; } = new ObservableCollection<WorkspaceItemViewModel>();
        private Dictionary<WorkspaceItemViewModel, Point> _originalPositions = new Dictionary<WorkspaceItemViewModel, Point>();
        private List<WorkspaceItemViewModel> _clipboardItems = new List<WorkspaceItemViewModel>();
        private List<WSConnectionViewModel> _clipboardConnections = new List<WSConnectionViewModel>();
        private WorkspaceItemViewModel? _selectedItem;
        public WorkspaceItemViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                bool isCtrlPressed = Keyboard.IsKeyDown(Key.LeftCtrl);

                if (!isCtrlPressed && _selectedItem != value)
                {
                    ClearAllSelections();
                }

                if (_selectedItem != null && _selectedItem != value)
                    _selectedItem.IsSelected = false;

                _selectedItem = value;

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;

                    if (!SelectedItems.Contains(_selectedItem))
                    {
                        SelectedItems.Add(_selectedItem);
                    }
                    else if (isCtrlPressed)
                    {
                        _selectedItem.IsSelected = false;
                        SelectedItems.Remove(_selectedItem);
                        _selectedItem = null;
                    }
                }

                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(IsMultipleSelectionActive));
            }
        }

        private bool _isSelecting;
        private Point _selectionStart;
        private Rect _selectionRect;

        public Rect SelectionRect
        {
            get => _selectionRect;
            set { _selectionRect = value; OnPropertyChanged(nameof(SelectionRect)); }
        }
		// Update the properties window with selection information
		private void UpdatePropertiesWindowViewModel()
		{
			Builder.PropertiesWindowViewModel.IsMultipleSelectionActive = IsMultipleSelectionActive;
			if (SelectedItems.Count == 0)
			{
				Builder.PropertiesWindowViewModel.Properties = null;
			}
		}
        // Add property to check if multiple selection is active
        public bool IsMultipleSelectionActive => SelectedItems.Count > 1;

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
        public bool IsSelectingMultiple
        {
            get => _isSelecting;
            set
            {
                _isSelecting = value;
                OnPropertyChanged(nameof(IsSelectingMultiple));
            }
        }

        private Rect _selectionRectScreen;
        public Rect SelectionRectScreen
        {
            get => _selectionRectScreen;
            set
            {
                _selectionRectScreen = value;
                OnPropertyChanged(nameof(SelectionRectScreen));
            }
        }

        private WSConnectionViewModel? connectionInProgress;



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
            WSConnectionViewModel newConnection = new WSConnectionViewModel(connection.Source, connection.Target, connection.SourceFaceDirection, connection.TargetFaceDirection);
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

        private void OnDeleteKeyDown(KeyEventArgs e)
        {

            if (SelectedConnection != null && e.Key == Key.Delete)
            {
                WorkspaceConnections.Remove(SelectedConnection);
                SelectedConnection = null;
                return;
            }
            else if (e.Key == Key.Delete)
            {
                if (SelectedItems.Count > 0)
                {

                    var itemsToRemove = SelectedItems.ToList();
                    foreach (var item in itemsToRemove)
                    {
                        RemoveElementConnections(item);
                        WorkspaceItems.Remove(item);
                    }

                    ClearAllSelections();
                    _selectedItem = null;
                }
                else if (SelectedItem != null)
                {
                    RemoveElementConnections(SelectedItem);
                    WorkspaceItems.Remove(SelectedItem);
                    SelectedItem = null;
                }
                return;
            }
        }

        private void RemoveElementConnections(WorkspaceItemViewModel SelectedItem)
        {

            for (int i = 0; i < WorkspaceConnections.Count; i++)
            {
                if (WorkspaceConnections[i].Source == SelectedItem || WorkspaceConnections[i].Target == SelectedItem)
                {
                    WorkspaceConnections.Remove(WorkspaceConnections[i--]);
                }
            }
            return;

        }
        public string GenerateElementName(WorkspaceItemViewModel item)
        {
            string itemClass = item.GetType().Name;
            string acronym = itemClass.Substring(2, 4);

            int maxval = 0;
            foreach (var existingItem in WorkspaceItems)
            {
                if (existingItem.Name.StartsWith(acronym))
                    if (int.TryParse(existingItem.Name.Substring(acronym.Length), out int number))
                        maxval = Math.Max(maxval, number);
            }
            return $"{acronym}{maxval + 1}";
        }

        /// <summary>
        /// Clears current workspace items and connections and adds new ones
        /// </summary>
        public void UpdateItemsAndConnections(ObservableCollection<WorkspaceItemViewModel> items, ObservableCollection<WSConnectionViewModel> connections)
        {
            WorkspaceItems.Clear();
            WorkspaceConnections.Clear();

            foreach (var item in items)
            {
                WorkspaceItems.Add(item);
            }

            foreach (var connection in connections)
            {
                WorkspaceItemViewModel source = WorkspaceItems.First(item => item.Equals(connection.Source));
                WorkspaceItemViewModel target = WorkspaceItems.First(item => item.Equals(connection.Target));
                WorkspaceConnections.Add(new WSConnectionViewModel(source, target));
            }
        }

        private void ClearAllMarkers()
        {
            foreach (var item in WorkspaceItems)
            {
                item.MarkerDirection = null;
            }
        }
        private WorkspaceItemViewModel CloneWorkspaceItem(WorkspaceItemViewModel original)
        {
            if (original is WSConvolutionViewModel convItem)
            {
                return new WSConvolutionViewModel(
                    convItem.InputChannels,
                    convItem.OutputChannels,
                    convItem.KernelSize,
                    convItem.Stride,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSPoolingViewModel poolItem)
            {
                return new WSPoolingViewModel(
                    poolItem.KernelSize,
                    poolItem.Stride,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSFullyConnectedViewModel fcItem)
            {
                return new WSFullyConnectedViewModel(
                    fcItem.InputNeurons,
                    fcItem.OutputNeurons,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSInputViewModel inputItem)
            {
                return new WSInputViewModel(
                    inputItem.ImageHeight,
                    inputItem.ImageWidth,
                    inputItem.NumChannels,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSOutputViewModel outputItem)
            {
                return new WSOutputViewModel(
                    outputItem.NumOutputs,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSBatchNormViewModel bnItem)
            {
                return new WSBatchNormViewModel(
                    bnItem.Momentum,
                    bnItem.Epsilon,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSFlattenViewModel flattenItem)
            {
                return new WSFlattenViewModel(
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSDropoutViewModel dropoutItem)
            {
                return new WSDropoutViewModel(
                    dropoutItem.Rate,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else if (original is WSAddViewModel addItem)
            {
                return new WSAddViewModel(
                    addItem.NumInputs,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    original.Name
                );
            }
            else
            {
                Type itemType = original.GetType();
                return (WorkspaceItemViewModel)Activator.CreateInstance(
                    itemType,
                    original.Position.X,
                    original.Position.Y,
                    original.Width,
                    original.Height,
                    original.Opacity,
                    "FAILED_CLONE");
            }
        }
        public void ControlKeyDown(KeyEventArgs e)
        {
            // Copy Ctrl+C
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                if (SelectedItems.Count > 0)
                {
                    CopySelectedItems();
                    e.Handled = true;
                }
            }
            // Paste Ctrl+V
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.V)
            {
                PasteItems();
                e.Handled = true;
            }
            // Cut Ctrl+X
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X)
            {
                if (SelectedItems.Count > 0)
                {
                    CutSelectedItems();
                    e.Handled = true;
                }
            }
        }
        private void ClearAllSelections()
        {
            foreach (var item in SelectedItems)
            {
                item.IsSelected = false;
            }
            SelectedItems.Clear();
            OnPropertyChanged(nameof(IsMultipleSelectionActive));
            UpdatePropertiesWindowViewModel(); 
        }
        private void UpdateSelectionRectangle(Point currentPos)
        {
            if (!IsSelectingMultiple)
                return;

            double left = Math.Min(_selectionStart.X, currentPos.X);
            double top = Math.Min(_selectionStart.Y, currentPos.Y);
            double width = Math.Abs(_selectionStart.X - currentPos.X);
            double height = Math.Abs(_selectionStart.Y - currentPos.Y);

            SelectionRectScreen = new Rect(left, top, width, height);

            Rect selectionRectCanvas = new Rect(
                (left - CanvasPanOffset.X) / ZoomFactor,
                (top - CanvasPanOffset.Y) / ZoomFactor,
                width / ZoomFactor,
                height / ZoomFactor
            );

            bool isCtrlPressed = Keyboard.IsKeyDown(Key.LeftCtrl);

            if (!isCtrlPressed || width < 5 || height < 5)
            {
                ClearAllSelections();
            }

            foreach (var item in WorkspaceItems)
            {
                Rect itemRect = new Rect(item.Position.X, item.Position.Y, item.Width, item.Height);

                if (selectionRectCanvas.IntersectsWith(itemRect))
                {
                    if (!SelectedItems.Contains(item))
                    {
                        item.IsSelected = true;
                        SelectedItems.Add(item);

                        if (_selectedItem == null)
                        {
                            _selectedItem = item;
                            OnPropertyChanged(nameof(SelectedItem));
                        }
                    }
                }
                else if (!isCtrlPressed)
                {
                    item.IsSelected = false;
                    if (SelectedItems.Contains(item))
                    {
                        SelectedItems.Remove(item);
                    }
                }
            }

            OnPropertyChanged(nameof(IsMultipleSelectionActive));
            UpdatePropertiesWindowViewModel();
        }
        private void CopySelectedItems()
        {
            if (SelectedItems.Count == 0)
                return;

            _clipboardItems.Clear();
            _clipboardConnections.Clear();


            Dictionary<WorkspaceItemViewModel, WorkspaceItemViewModel> originalToCopyMap = new Dictionary<WorkspaceItemViewModel, WorkspaceItemViewModel>();

            // Copy selected items
            foreach (var original in SelectedItems)
            {
                WorkspaceItemViewModel copy = CloneWorkspaceItem(original);
                _clipboardItems.Add(copy);
                originalToCopyMap[original] = copy;
            }

            // Copy items' connections
            foreach (var connection in WorkspaceConnections)
            {
                if (originalToCopyMap.ContainsKey(connection.Source) &&
                    originalToCopyMap.ContainsKey(connection.Target))
                {
                    WSConnectionViewModel copyConn = new WSConnectionViewModel(
                        originalToCopyMap[connection.Source],
                        originalToCopyMap[connection.Target],
                        connection.SourceFaceDirection,
                        connection.TargetFaceDirection
                    );

                    _clipboardConnections.Add(copyConn);
                }
            }
        }
        private void CutSelectedItems()
        {
            if (SelectedItems.Count == 0)
                return;

            // Copy 
            CopySelectedItems();

            // Delete
            var itemsToRemove = SelectedItems.ToList();
            foreach (var item in itemsToRemove)
            {
                RemoveElementConnections(item);
                WorkspaceItems.Remove(item);
            }

            ClearAllSelections();
            _selectedItem = null;
        }
        private void PasteItems()
        {
            if (_clipboardItems.Count == 0)
                return;

            ClearAllSelections();

            Point screenCenter = new Point(
                VisibleWidth / 2,
                VisibleHeight / 2
            );

            //  Screen center to canvas coordinates
            Point canvasCenterPosition = new Point(
                (screenCenter.X - CanvasPanOffset.X) / ZoomFactor,
                (screenCenter.Y - CanvasPanOffset.Y) / ZoomFactor
            );

            // Center for clipboard items
            double itemsCenterX = 0;
            double itemsCenterY = 0;

            foreach (var item in _clipboardItems)
            {
                itemsCenterX += item.Position.X;
                itemsCenterY += item.Position.Y;
            }

            itemsCenterX /= _clipboardItems.Count;
            itemsCenterY /= _clipboardItems.Count;

            Vector offsetVector = new Vector(
                canvasCenterPosition.X - itemsCenterX,
                canvasCenterPosition.Y - itemsCenterY
            );

            Dictionary<WorkspaceItemViewModel, WorkspaceItemViewModel> oldToNewMap =
                new Dictionary<WorkspaceItemViewModel, WorkspaceItemViewModel>();

            foreach (var clipboardItem in _clipboardItems)
            {
                WorkspaceItemViewModel newItem = CloneWorkspaceItem(clipboardItem);

                newItem.Position = new Point(
                    clipboardItem.Position.X + offsetVector.X,
                    clipboardItem.Position.Y + offsetVector.Y
                );

                // Generate new name
                newItem.Name = GenerateElementName(newItem);

                // Paste selected item
                WorkspaceItems.Add(newItem);
                newItem.IsSelected = true; // "highlight" for pasted items
                SelectedItems.Add(newItem);
                _selectedItem = newItem;
                oldToNewMap[clipboardItem] = newItem;

            }

            // Re-Connecting pasted items
            foreach (var conn in _clipboardConnections)
            {
                if (oldToNewMap.ContainsKey(conn.Source) && oldToNewMap.ContainsKey(conn.Target))
                {
                    WSConnectionViewModel newConn = new WSConnectionViewModel(
                        oldToNewMap[conn.Source],
                        oldToNewMap[conn.Target],
                        conn.SourceFaceDirection,
                        conn.TargetFaceDirection
                    );

                    WorkspaceConnections.Add(newConn);
                }
            }
            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(IsMultipleSelectionActive));
            UpdatePropertiesWindowViewModel();
        }
    }
}
