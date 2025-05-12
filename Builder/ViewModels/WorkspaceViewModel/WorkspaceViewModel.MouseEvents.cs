using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Builder.Enums;
using Builder.ViewModels.WorkspaceElements;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand MouseWheelCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

        private bool isDragDropping = false; //is drag and drop on an item active
        private WorkspaceItemViewModel draggingItem = null;

        private void OnDragOver(DragEventArgs e)
        {
            if (e.Data.GetDataPresent("SelectedItems"))
            {

                var draggedItem = e.Data.GetData(typeof(WorkspaceItemViewModel)) as WorkspaceItemViewModel;
                if (draggedItem == null || !_originalPositions.ContainsKey(draggedItem))
                    return;

                Point dropPosition = e.GetPosition(e.Source as IInputElement);
                dropPosition.X -= mouseOffset.X;
                dropPosition.Y -= mouseOffset.Y;

                Vector moveDelta = new Vector(
                    dropPosition.X - _originalPositions[draggedItem].X,
                    dropPosition.Y - _originalPositions[draggedItem].Y
                );

                foreach (var item in SelectedItems)
                {
                    if (_originalPositions.ContainsKey(item))
                    {
                        Point originalPos = _originalPositions[item];
                        item.Position = new Point(originalPos.X + moveDelta.X, originalPos.Y + moveDelta.Y);
                    }
                }
            }
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            //update connection in progress 
            if (connectionInProgress != null)
                connectionInProgress.CurrentMousePosition = e.GetPosition(e.Source as IInputElement);
            //pan canvas
            if (_isPanning)
            {
                Point currentPosition = e.GetPosition(null);
                Vector delta = currentPosition - _panStart;
                CanvasPanOffset = ClipCanvasPan(new Point(CanvasPanOffset.X + delta.X, CanvasPanOffset.Y + delta.Y));
                _panStart = currentPosition;
            }
            if (IsSelectingMultiple)
            {
                Point currentPos = e.GetPosition(_selectionStartReference);
                UpdateSelectionRectangle(currentPos);
            }
            if (isDragDropping)
            {
                Point currentPos = e.GetPosition(e.Source as IInputElement);
                currentPos.X -= mouseOffset.X;
                currentPos.Y -= mouseOffset.Y;
                if (draggedItem != null)
                    draggedItem.Position = currentPos;
            }
            //if hovering over workspace element and connection mode is active then show markers on possible connection starts or ends
            var data = e.OriginalSource as FrameworkElement;
            if (e.OriginalSource is Image && Builder.isMakingConnection)
            {
                if (data != null && data.DataContext is WorkspaceItemViewModel workspaceItem)
                {
                    workspaceItem.MarkerDirection = FaceDirection.Top;
                    Point relativePoint = e.GetPosition(data);

                    double distanceFromTop = relativePoint.Y;
                    double distanceFromLeft = relativePoint.X;
                    double distanceFromRight = workspaceItem.Width - relativePoint.X;
                    double distanceFromBottom = workspaceItem.Height - relativePoint.Y;

                    double minDistance = Math.Min(Math.Min(distanceFromTop, distanceFromLeft), Math.Min(distanceFromRight, distanceFromBottom));

                    if (minDistance == distanceFromTop)
                        workspaceItem.MarkerDirection = FaceDirection.Top;
                    else if (minDistance == distanceFromLeft)
                        workspaceItem.MarkerDirection = FaceDirection.Left;
                    else if (minDistance == distanceFromRight)
                        workspaceItem.MarkerDirection = FaceDirection.Right;
                    else if (minDistance == distanceFromBottom)
                        workspaceItem.MarkerDirection = FaceDirection.Bottom;
                }
            }
            //clear all markers if not hovering over workspace item
            else
                ClearAllMarkers();
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            Point mousePos;
            if (e.OriginalSource is Canvas)
                mousePos = e.GetPosition(e.OriginalSource as IInputElement);
            else
                mousePos = e.GetPosition(e.Source as IInputElement);

            double oldZoom = ZoomFactor;
            double zoomDelta = e.Delta > 0 ? 0.1 : -0.1;
            double minZoom = Math.Max(0.2, Math.Max(VisibleWidth / Width, VisibleHeight / Height));
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
            if (e.Source is FrameworkElement element)
            {
                Keyboard.Focus(element);
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _isPanning = false;
                IsSelectingMultiple = true;
                var parentGrid = (e.Source as FrameworkElement)?.Parent as FrameworkElement;
                _selectionStartReference = parentGrid;
                _selectionStart = e.GetPosition(parentGrid);
                SelectionRectScreen = new Rect(_selectionStart, new Size(0, 0));
                e.Handled = true;
                return;
            }

            var data = e.OriginalSource as FrameworkElement;
            if (e.OriginalSource is Image)
            {
                SelectedConnection = null;
                if (data != null && data.DataContext is WorkspaceItemViewModel workspaceItem)
                {
                    bool isPartOfMultiSelection = IsMultipleSelectionActive && SelectedItems.Contains(workspaceItem);

                    // jeigu nera is slected group, focusint pasirinkta 
                    if (!isPartOfMultiSelection)
                    {
                        SelectedItem = workspaceItem;
                        Builder.PropertiesWindowViewModel.SelectedWorkspaceItem = workspaceItem;
                    }

                    if (Builder.isMakingConnection)
                    {
                        connectionInProgress = new WSConnectionViewModel();
                        connectionInProgress.Source = workspaceItem;
                        connectionInProgress.CurrentMousePosition = e.GetPosition(e.Source as IInputElement);
                        connectionInProgress.SourceFaceDirection = workspaceItem.MarkerDirection ?? FaceDirection.Right;
                        WorkspaceConnections.Add(connectionInProgress);
                        return;
                    }

                    mouseOffset = e.GetPosition(data);

                    // multi item drag'as 
                    if (isPartOfMultiSelection && e.LeftButton == MouseButtonState.Pressed)
                    {
                        _originalPositions.Clear();
                        foreach (var selectedItem in SelectedItems)
                            _originalPositions[selectedItem] = selectedItem.Position;

                        DataObject dragData = new DataObject();
                        dragData.SetData(typeof(WorkspaceItemViewModel), workspaceItem);
                        dragData.SetData("SelectedItems", true);
                        DragDrop.DoDragDrop(e.OriginalSource as UIElement, dragData, DragDropEffects.Move);
                    }
                    //item drag and drop
                    else if (data != null && e.LeftButton == MouseButtonState.Pressed && data.DataContext is WorkspaceItemViewModel)
                    {
                        Mouse.Capture(data.DataContext as UIElement, CaptureMode.SubTree);
                        mouseOffset = e.GetPosition(data); // mouse position relative to top-left of the rectangle
                        draggedItem = data.DataContext as WorkspaceItemViewModel;
                        isDragDropping = true;
                    }
                }
            }
            else if (data != null && e.OriginalSource is Polyline && data.DataContext is WSConnectionViewModel connection)
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
                ClearAllSelections();
                OnPropertyChanged(nameof(IsMultipleSelectionActive));
            }
        }

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            draggedItem = null;
            _isPanning = false;
            _originalPositions.Clear();

            if (isDragDropping)
            {
                isDragDropping = false;
                draggedItem = null;
            }
            if (IsSelectingMultiple)
            {
                UpdateSelectionRectangle(e.GetPosition(_selectionStartReference));
                IsSelectingMultiple = false;
                _selectionStartReference = null;

                SelectionRectScreen = Rect.Empty;
                OnPropertyChanged(nameof(SelectionRectScreen));

                e.Handled = true;
                return;
            }
            if (Builder.WorkspaceItemSelected.Count > 0)
            {
                IInputElement? mouseEventOriginalSource = e.OriginalSource as IInputElement;
                if (e.OriginalSource is Canvas)
                {
                    Point mousePos = e.GetPosition(e.OriginalSource as IInputElement);
                    Builder.WorkspaceItemSelected[0].Opacity = 1;
                    Builder.WorkspaceItemSelected[0].Position = mousePos;
                    Builder.WorkspaceItemSelected[0].Name = GenerateElementName(Builder.WorkspaceItemSelected[0]);
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
                        connectionInProgress.TargetFaceDirection = workspaceItem.MarkerDirection ?? FaceDirection.Left;
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
    }
}
