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

        bool isEnterHit = false;

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
            if (leftScrollViewer != null)
            {
                double screenHeight = ActualHeight - 65; // Adjust padding as needed
                leftScrollViewer.MaxHeight = screenHeight;
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
            if(searchBox.Text == "Search for manga")
            {
                searchBox.Text = "";
                searchIcon.Visibility = Visibility.Hidden;
            }
        }

        private void SearchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(!isEnterHit)
            {
                searchBox.Text = "Search for manga";
            }
            searchIcon.Visibility = Visibility.Visible;
            isEnterHit = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isEnterHit)
            {
                searchBox.Text = "Search for manga";
            }
            searchIcon.Visibility = Visibility.Visible;
            isEnterHit = false;
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = searchBox.Text;

                PerformSearch(searchText);

                isEnterHit = true;

                outerScrollViewer.Focus();

            }
        }

        private void PerformSearch(string searchText)
        {
            if(DataContext is MangasDisplayVM viewModel)
            {
                if (searchText.Length > 0)
                {
                    viewModel.SearchMangas(searchText);
                }
                else
                {
                    searchBox.Text = "Search for manga";

                    viewModel.ReloadAllManga();
                }
            }
        }
    }
}
