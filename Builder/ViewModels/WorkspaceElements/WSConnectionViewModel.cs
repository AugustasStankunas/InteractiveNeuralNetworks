using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using Shared.ViewModels;
 using System.Windows.Media;


namespace Builder.ViewModels.WorkspaceElements
{

    public class WSConnectionViewModel : ViewModelBase
    {
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

        public WSConnectionViewModel(WorkspaceItemViewModel source, WorkspaceItemViewModel target)
        {
            Source = source;
            Target = target;
            UpdatePolyline();
        }

        public WSConnectionViewModel() { }

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

            points.Add(sourcePos);

            Point midpoint = new Point(targetPos.X, sourcePos.Y);
            points.Add(midpoint);

            points.Add(targetPos);

            Points = points;
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
