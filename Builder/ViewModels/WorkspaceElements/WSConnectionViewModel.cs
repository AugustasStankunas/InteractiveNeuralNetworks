using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using Shared.ViewModels;
using System.Windows.Media;
using Builder.Enums;


namespace Builder.ViewModels.WorkspaceElements
{

    public class WSConnectionViewModel : ViewModelBase
    {
        public WSConnectionViewModel(WorkspaceItemViewModel source, WorkspaceItemViewModel target, FaceDirection sourceFaceDirection = FaceDirection.Right, 
                                     FaceDirection targetFaceDirection = FaceDirection.Left)
        {
            Source = source;
            Target = target;
            SourceFaceDirection = sourceFaceDirection;
            TargetFaceDirection = targetFaceDirection;
            UpdatePolyline();
        }

        public WSConnectionViewModel() { }

        private WorkspaceItemViewModel? _source;
        public WorkspaceItemViewModel? Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    if (_source != null)
                        _source.PropertyChanged -= OnSourcePropertyChanged;
                    _source = value;
                    if (_source != null)
                        _source.PropertyChanged += OnSourcePropertyChanged;

                    OnPropertyChanged(nameof(Source));
                    UpdatePolyline();
                }
            }
        }
        private WorkspaceItemViewModel? _target;
        public WorkspaceItemViewModel? Target
        {
            get => _target;
            set
            {
                if (_target != value)
                {
                    if (_target != null)
                        _target.PropertyChanged -= OnTargetPropertyChanged;
                    _target = value;
                    if (_target != null)
                        _target.PropertyChanged += OnTargetPropertyChanged;
                    OnPropertyChanged(nameof(Target));
                    OnPropertyChanged(nameof(TargetPoint));
                    UpdatePolyline();
                }
            }
        }

        private FaceDirection _sourceFaceDirection;
        public FaceDirection SourceFaceDirection
        {
            get => _sourceFaceDirection;
            set
            {
                _sourceFaceDirection = value;
                OnPropertyChanged(nameof(SourceFaceDirection));
                UpdatePolyline();
            }
        }

        private FaceDirection _targetFaceDirection;
        public FaceDirection TargetFaceDirection
        {
            get => _targetFaceDirection;
            set
            {
                _targetFaceDirection = value;
                OnPropertyChanged(nameof(TargetFaceDirection));
                UpdatePolyline();
            }
        }

        private Point _currentMousePosition;
        [JsonIgnore]
        public Point CurrentMousePosition
        {
            get => _currentMousePosition;
            set
            {
                _currentMousePosition = value;
                OnPropertyChanged(nameof(CurrentMousePosition));
                OnPropertyChanged(nameof(TargetPoint));
                UpdatePolyline();
            }
        }
        [JsonIgnore]
        public Point TargetPoint => Target != null
            ? new Point(Target.Position.X + Target.Width / 2, Target.Position.Y + Target.Height / 2)
            : CurrentMousePosition;

        private bool _IsSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                OnPropertyChanged(nameof(IsSelected));

                if (value)
                    Mouse.AddMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
                else
                    Mouse.RemoveMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
            }
        }

        private PointCollection _points;
        [JsonIgnore]
        public PointCollection Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged(nameof(Points));
            }
        }

        /// <summary>
        /// Offset from the item to the connection line when wrapping around corners etc
        /// </summary>
        private int itemConnectionOffset = 10;

        public void GlobalMouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var element = e.OriginalSource as FrameworkElement;
                if (element?.DataContext != this)
                {
                    IsSelected = false;
                }
            }
        }

        private void OnTargetPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WorkspaceItemViewModel.Position) ||
                e.PropertyName == nameof(WorkspaceItemViewModel.Width) ||
                e.PropertyName == nameof(WorkspaceItemViewModel.Height))
            {
                OnPropertyChanged(nameof(TargetPoint));
                UpdatePolyline();
            }
        }

        private void OnSourcePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WorkspaceItemViewModel.Position) ||
                e.PropertyName == nameof(WorkspaceItemViewModel.Width) ||
                e.PropertyName == nameof(WorkspaceItemViewModel.Height))
            {
                OnPropertyChanged(nameof(Source));
                UpdatePolyline();
            }
        }

        private void UpdatePolyline()
        {
            //initialize variables for later use

            Point sourcePos = new Point(Source.Position.X + Source.Width / 2, Source.Position.Y + Source.Height / 2);
            Point targetPos = new Point(TargetPoint.X, TargetPoint.Y);
            PointCollection points = new PointCollection();

            Point pointAfterStart = GetPointNearItem(Source, SourceFaceDirection);

            Point pointBeforeEnd;
            if (Target != null)
                pointBeforeEnd = GetPointNearItem(Target, TargetFaceDirection);
            else
                pointBeforeEnd = new Point(TargetPoint.X, TargetPoint.Y);

            
            // Add starting items from the source 
            points.Add(sourcePos);
            points.Add(pointAfterStart);

            PointCollection startWrapped = wrapAroundItem(SourceFaceDirection, Source, pointAfterStart, targetPos);

            foreach (Point p in startWrapped)
            {
                points.Add(p);
            }

            // the whole logic of pathfinding

            Point midpoint = new Point(points[points.Count - 1].X, pointBeforeEnd.Y);

            points.Add(midpoint);

            // add last points
            if (Target != null)
            {
                PointCollection endWrapped = wrapAroundItem(TargetFaceDirection, Target, pointBeforeEnd, midpoint);

                foreach (Point p in endWrapped.Reverse())
                {
                    points.Add(p);
                }
            }
            

            points.Add(pointBeforeEnd);

            points.Add(targetPos);

            Points = points;
        }

        private PointCollection wrapAroundItem(FaceDirection directionFromItem, WorkspaceItemViewModel item, Point startPoint, Point endPoint)
        {
            PointCollection points = new PointCollection();

            switch (directionFromItem)
            {
                case FaceDirection.Top:
                    if (endPoint.Y > startPoint.Y)
                    {
                        int xSign = endPoint.X > startPoint.X ? 1 : -1;
                        double x = startPoint.X + xSign * (item.Width / 2 + itemConnectionOffset);
                        double lastPointY = Math.Min(endPoint.Y, startPoint.Y + 2 * itemConnectionOffset + item.Height);

                        points.Add(new Point(x, startPoint.Y));
                        points.Add(new Point(x, lastPointY));
                    }
                    break;
                case FaceDirection.Bottom:
                    if (endPoint.Y < startPoint.Y)
                    {
                        int xSign = endPoint.X > startPoint.X ? 1 : -1;
                        double x = startPoint.X + xSign * (item.Width / 2 + itemConnectionOffset);
                        double lastPointY = Math.Max(endPoint.Y, startPoint.Y - 2 * itemConnectionOffset - item.Height);
                        points.Add(new Point(x, startPoint.Y));
                        points.Add(new Point(x, lastPointY));
                    }
                    break;
                case FaceDirection.Left:
                    if (endPoint.X > startPoint.X)
                    {
                        int ySign = endPoint.Y > startPoint.Y ? 1 : -1;
                        double y = startPoint.Y + ySign * (item.Height / 2 + itemConnectionOffset);
                        double lastPointX = Math.Min(endPoint.X, startPoint.X + 2 * itemConnectionOffset + item.Width);
                        points.Add(new Point(startPoint.X, y));
                        points.Add(new Point(lastPointX, y));
                    }
                    break;
                case FaceDirection.Right:
                    if (endPoint.X < startPoint.X)
                    {
                        int ySign = endPoint.Y > startPoint.Y ? 1 : -1;
                        double y = startPoint.Y + ySign * (item.Height / 2 + itemConnectionOffset);
                        double lastPointX = Math.Max(endPoint.X, startPoint.X - 2 * itemConnectionOffset - item.Width);
                        points.Add(new Point(startPoint.X, y));
                        points.Add(new Point(lastPointX, y));
                    }
                    break;
            }

            return points;
        }

        /// <summary>
        /// Looks at face direction of the item and draws a straight line in that direction
        /// </summary>
        /// <param name="item"></param>
        /// <param name="offset">how many pixels to draw from the image</param>
        /// <returns></returns>
        private Point GetPointNearItem(WorkspaceItemViewModel item, FaceDirection faceDirection)
        {
            Point point = new Point(item.Position.X + item.Width / 2, item.Position.Y + item.Height / 2);
            switch (faceDirection)
            {
                case FaceDirection.Top:
                    point.Y -= item.Height / 2 + itemConnectionOffset;
                    break;
                case FaceDirection.Bottom:
                    point.Y += item.Height / 2 + itemConnectionOffset;
                    break;
                case FaceDirection.Left:
                    point.X -= item.Width / 2 + itemConnectionOffset;
                    break;
                case FaceDirection.Right:
                    point.X += item.Width / 2 + itemConnectionOffset;
                    break;
            }
            return point;
        }

        public override bool Equals(object? obj)
        {
            WSConnectionViewModel? other = obj as WSConnectionViewModel;

            if (other == null)
                return false;
            else
                return Source == other.Source && Target == other.Target;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
