using Avalonia.Controls;

namespace XpathRunner.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var screen = Screens.Primary;
            // Set width and height as percentages of the screen
            if (screen != null)
            {
                Width = screen.WorkingArea.Width * 0.5;
                Height = screen.WorkingArea.Height * 0.6;
            }
        }
        

    }
}