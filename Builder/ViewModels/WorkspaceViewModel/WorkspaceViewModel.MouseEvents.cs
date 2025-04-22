using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Builder.ViewModels.WorkspaceElements;
using System.Windows.Shapes;
using Shared.ViewModels;
using Builder.Enums;


namespace Builder.ViewModels
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeaveCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand MouseWheelCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

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
            if (e.OriginalSource is Rectangle)
                mousePos = e.GetPosition(e.Source as IInputElement);
            else
                mousePos = e.GetPosition(e.OriginalSource as IInputElement);

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
                    SelectedItem = workspaceItem;
                    Builder.PropertiesWindowViewModel.SelectedWorkspaceItem = workspaceItem;

                    if (Builder.isMakingConnection)
                    {
                        connectionInProgress = new WSConnectionViewModel();
                        connectionInProgress.Source = workspaceItem;
                        connectionInProgress.CurrentMousePosition = e.GetPosition(e.Source as IInputElement);
                        connectionInProgress.SourceFaceDirection = workspaceItem.MarkerDirection ?? FaceDirection.Right;
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
            }
        }

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _isPanning = false;

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
