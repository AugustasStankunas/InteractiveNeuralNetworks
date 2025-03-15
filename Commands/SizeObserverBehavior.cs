using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteractiveNeuralNetworks.ViewModels;
using System.Windows;

namespace InteractiveNeuralNetworks.Commands
{
    class SizeObserverBehavior
    {
        public static readonly DependencyProperty ObserveProperty =
            DependencyProperty.RegisterAttached(
                "Observe",
                typeof(bool),
                typeof(SizeObserverBehavior),
                new PropertyMetadata(false, OnObserveChanged));

        public static bool GetObserve(DependencyObject obj) => (bool)obj.GetValue(ObserveProperty);
        public static void SetObserve(DependencyObject obj, bool value) => obj.SetValue(ObserveProperty, value);

        private static void OnObserveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                if ((bool)e.NewValue)
                    fe.SizeChanged += Fe_SizeChanged;
                else
                    fe.SizeChanged -= Fe_SizeChanged;
            }
        }

        private static void Fe_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is WorkspaceViewModel vm)
            {
                vm.VisibleWidth = fe.ActualWidth;
                vm.VisibleHeight = fe.ActualHeight;
            }
        }
    }
}
