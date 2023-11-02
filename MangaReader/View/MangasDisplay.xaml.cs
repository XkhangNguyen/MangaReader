using MangaReader.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MangaReader.View
{
    /// <summary>
    /// Interaction logic for MangasDisplay.xaml
    /// </summary>
    public partial class MangasDisplay : UserControl
    {
        ScrollViewer? currentScrollViewer;

        Button? currentButton;

        public MangasDisplay()
        {
            InitializeComponent();
            Loaded += MangasDisplay_Loaded;
            SizeChanged += MangasDisplay_SizeChanged;
        }

        private void OuterScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Check if the mouse wheel event should be passed to the hovered ScrollViewer
            if (IsMouseOverScrollViewer(e))
            {
                e.Handled = true; // Prevent the outer ScrollViewer from scrolling
                ScrollScrollViewer(e.Delta); // Scroll the hovered ScrollViewer
            }
        }



        private bool IsMouseOverScrollViewer(MouseWheelEventArgs e)
        {
            if (currentScrollViewer != null)
            {
                // Check if the mouse pointer is over the hovered ScrollViewer
                Point position = e.GetPosition(currentScrollViewer);
                Rect bounds = new Rect(0, 0, currentScrollViewer.ActualWidth, currentScrollViewer.ActualHeight);
                return bounds.Contains(position);
            }

            return false;
        }

        private void ScrollScrollViewer(double delta)
        {
            // Scroll the hovered ScrollViewer based on the mouse wheel delta
            currentScrollViewer!.ScrollToVerticalOffset(currentScrollViewer.VerticalOffset - delta);
        }


        private void ScrollViewer_MouseEnter(object sender, MouseEventArgs e)
        {
            currentScrollViewer = sender as ScrollViewer;
        }

        private void ScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            currentScrollViewer = null;
        }

        private void MangasDisplay_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Update MaxHeight when the screen size changes
            UpdateMaxHeight();
        }

        private void MangasDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            // Initial setup, set the MaxHeight on load
            UpdateMaxHeight();
        }

        private void UpdateMaxHeight()
        {
            if (leftScrollViewer != null && rightScrollViewer != null)
            {
                double screenHeight = ActualHeight - 65; // Adjust padding as needed
                leftScrollViewer.MaxHeight = screenHeight;
                rightScrollViewer.MaxHeight = screenHeight;
                outerScrollViewer.MaxHeight = ActualHeight;
            }
        }

        private void UpdateHeightScrollViewer()
        {
            if(items_control.ActualHeight < ActualHeight)
                outerScrollViewer.MaxHeight = items_control.ActualHeight;
            else
                outerScrollViewer.MaxHeight = ActualHeight;
        }

        private void GenreButton_Click(object sender, RoutedEventArgs e)
        {

            if(currentButton != null && currentButton == sender && DataContext is MangasDisplayVM viewModel)
            {
                viewModel.ReloadAllManga();
                currentButton = null;
            }
            else
            {
                currentButton = (Button)sender;

                UpdateHeightScrollViewer();
            }
        }

        private void SearchBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            searchBox.Text = "";
            searchIcon.Visibility = Visibility.Hidden;
            searchBox.Foreground = Brushes.Black;
        }

        private void SearchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            searchBox.Text = "Search for manga";
            searchIcon.Visibility = Visibility.Visible;
            searchBox.Foreground = Brushes.Gray;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchBox.Text = "Search for manga";
            searchIcon.Visibility = Visibility.Visible;
            searchBox.Foreground = Brushes.Gray;
        }
    }
}
