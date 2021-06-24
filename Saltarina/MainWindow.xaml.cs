using Microsoft.Extensions.Logging;
using Saltarina.MouseControl;
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

        public IAboutViewModel AboutViewModel_DataContext { get; set; }

        public MainWindow(ILogger<MainWindow> logger,
            IAboutViewModel aboutViewModel)
        {
            _logger = logger;

            InitializeComponent();
            DataContext = this;

            AboutViewModel_DataContext = aboutViewModel;            
        }
    }
}
