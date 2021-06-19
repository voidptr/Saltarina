using Microsoft.Extensions.Logging;
using Saltarina.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Saltarina.ViewModels
{
    public class AboutViewModel : IAboutViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _aboutBoxText = "heyo";
        public string AboutBoxText
        {
            get { return _aboutBoxText; }
            set
            {
                _aboutBoxText = value;
                OnPropertyChanged("AboutBoxText");
            }
        }

        public string _aboutBoxMouseDemoText = "heyo2";
        public string AboutBoxMouseDemoText
        {
            get { return _aboutBoxMouseDemoText; }
            set
            {
                _aboutBoxMouseDemoText = value;
                OnPropertyChanged("AboutBoxMouseDemoText");
            }
        }

        private ILogger<AboutViewModel> _logger;
        public AboutViewModel(ILogger<AboutViewModel> logger)
        {
            _logger = logger;
            AboutBoxText = "YAOOOOO";


        }

        /// <summary>
        /// Raise the PropertyChanged event to notify the window UI elements that they should update their content.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
