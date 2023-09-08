using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace MangaReader.Utilities
{
    public static class ScrollViewerMouseWheelBehavior
    {
        public static readonly DependencyProperty IncreaseScrollSpeedProperty =
            DependencyProperty.RegisterAttached(
                "IncreaseScrollSpeed",
                typeof(bool),
                typeof(ScrollViewerMouseWheelBehavior),
                new PropertyMetadata(false, OnIncreaseScrollSpeedChanged)
            );

        public static bool GetIncreaseScrollSpeed(DependencyObject obj)
        {
            return (bool)obj.GetValue(IncreaseScrollSpeedProperty);
        }

        public static void SetIncreaseScrollSpeed(DependencyObject obj, bool value)
        {
            obj.SetValue(IncreaseScrollSpeedProperty, value);
        }

        private static void OnIncreaseScrollSpeedChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                if ((bool)e.NewValue)
                {
                    scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
                }
                else
                {
                    scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
                }
            }
        }

        private static void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                // Increase the scroll speed by multiplying the delta value
                double scrollDelta = e.Delta * 2; // Adjust the factor as needed

                // Perform the scrolling
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - scrollDelta);

                // Mark the event as handled to prevent standard scrolling behavior
                e.Handled = true;
            }
        }
    }
}
