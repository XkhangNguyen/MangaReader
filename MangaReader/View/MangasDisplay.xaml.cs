using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace MangaReader.View
{
    /// <summary>
    /// Interaction logic for MangasDisplay.xaml
    /// </summary>
    public partial class MangasDisplay : UserControl
    {
        ScrollViewer? innerScrollViewer;

        public MangasDisplay()
        {
            InitializeComponent();
        }

        private void OuterScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Check if the mouse wheel event should be passed to the inner ScrollViewer
            if (IsMouseOverInnerScrollViewer(e))
            {
                e.Handled = true; // Prevent the outer ScrollViewer from scrolling
                ScrollInnerScrollViewer(e.Delta); // Scroll the inner ScrollViewer
            }
        }



        private bool IsMouseOverInnerScrollViewer(MouseWheelEventArgs e)
        {
            if (innerScrollViewer != null)
            {
                // Check if the mouse pointer is over the inner ScrollViewer
                Point position = e.GetPosition(innerScrollViewer);
                Rect bounds = new Rect(0, 0, innerScrollViewer.ActualWidth, innerScrollViewer.ActualHeight);
                return bounds.Contains(position);
            }

            return false;
        }

        private void ScrollInnerScrollViewer(double delta)
        {
            // Scroll the inner ScrollViewer based on the mouse wheel delta
            innerScrollViewer!.ScrollToVerticalOffset(innerScrollViewer.VerticalOffset - delta);
        }


        private void InnerScrollViewer_MouseEnter(object sender, MouseEventArgs e)
        {
            innerScrollViewer = sender as ScrollViewer;
        }

        private void InnerScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            innerScrollViewer = null;
        }
    }
}
