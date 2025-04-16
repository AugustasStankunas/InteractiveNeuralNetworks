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
            Point sourcePos = new Point(Source.Position.X + Source.Width / 2, Source.Position.Y + Source.Height / 2);
            Point targetPos = new Point(TargetPoint.X, TargetPoint.Y);
            PointCollection points = new PointCollection();

            Point pointAfterStart = GetPointNearItem(Source, SourceFaceDirection, 10);

            Point pointBeforeEnd;
            if (Target != null)
                pointBeforeEnd = GetPointNearItem(Target, TargetFaceDirection, 10);
            else
                pointBeforeEnd = new Point(TargetPoint.X, TargetPoint.Y);

            Point midpoint = new Point(pointAfterStart.X, pointBeforeEnd.Y);
            //if (pointBeforeEnd.Y > pointAfterStart.Y)
            //    midpoint = new Point(pointAfterStart.X, pointBeforeEnd.Y);
            //else
            //    midpoint = new Point(pointBeforeEnd.X, pointAfterStart.Y);

            points.Add(sourcePos);
            points.Add(pointAfterStart);

            points.Add(midpoint);

            points.Add(pointBeforeEnd);

            points.Add(targetPos);

            Points = points;
        }

        /// <summary>
        /// Looks at face direction of the item and draws a straight line in that direction
        /// </summary>
        /// <param name="item"></param>
        /// <param name="offset">how many pixels to draw from the image</param>
        /// <returns></returns>
        private Point GetPointNearItem(WorkspaceItemViewModel item, FaceDirection faceDirection, int offset)
        {
            Point point = new Point(item.Position.X + item.Width / 2, item.Position.Y + item.Height / 2);
            switch (faceDirection)
            {
                case FaceDirection.Top:
                    point.Y -= item.Height / 2 + offset;
                    break;
                case FaceDirection.Bottom:
                    point.Y += item.Height / 2 + offset;
                    break;
                case FaceDirection.Left:
                    point.X -= item.Width / 2 + offset;
                    break;
                case FaceDirection.Right:
                    point.X += item.Width / 2 + offset;
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
