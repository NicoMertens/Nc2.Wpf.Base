using System.Windows;

namespace Nc2.Wpf.Base.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DeferredCommandViewModel = new DeferredCommandViewModel();
            DataContext = this;
            InitializeComponent();
        }

        public DeferredCommandViewModel DeferredCommandViewModel { get; }
    }
}
