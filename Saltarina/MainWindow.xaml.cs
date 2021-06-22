using Microsoft.Extensions.Logging;
using Saltarina.Screens;
using Saltarina.ViewModels;
using System.Windows;

namespace Saltarina
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger<MainWindow> _logger;
        private IScreenManager _screenManager;

        public IAboutViewModel AboutViewModel_DataContext { get; set; }

        public MainWindow(ILogger<MainWindow> logger,
            IAboutViewModel aboutViewModel,
            IScreenManager screenManager)
        {
            _logger = logger;
            _screenManager = screenManager;

            InitializeComponent();
            DataContext = this;

            AboutViewModel_DataContext = aboutViewModel;

            
        }
    }
}
