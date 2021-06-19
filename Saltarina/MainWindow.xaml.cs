using Microsoft.Extensions.Logging;
using Saltarina.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
