using Microsoft.Extensions.Logging;
using Saltarina.MouseHook;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;

namespace Saltarina.ViewModels
{
    public class AboutViewModel : IAboutViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _aboutBoxText;// = "Failed to load About content.";
        public string AboutBoxText
        {
            get { 
                if (_aboutBoxText == null)
                {
                    try
                    {
                        string readme = _filesystem.File.ReadAllText(@"README.md");
                        string license = _filesystem.File.ReadAllText(@"LICENSE.md");
                        _aboutBoxText = $"{readme}\n{license}";
                    }
                    catch
                    {
                        _logger.LogError("Unable to read README or LICENSE content.");
                        _aboutBoxText = "Saltarina!";
                    }            
                }
                return _aboutBoxText; 
            }
            set
            {
                _aboutBoxText = value;
                OnPropertyChanged("AboutBoxText");
            }
        }

        //public string _aboutBoxMouseDemoText = "heyo2";
        //public string AboutBoxMouseDemoText
        //{
        //    get { return _aboutBoxMouseDemoText; }
        //    set
        //    {
        //        _aboutBoxMouseDemoText = value;
        //        OnPropertyChanged("AboutBoxMouseDemoText");
        //    }
        //}

        private ILogger<AboutViewModel> _logger;
        private IMouseHook _mouseHook;
        private IFileSystem _filesystem;
        public AboutViewModel(ILogger<AboutViewModel> logger,
            IMouseHook mouseHook,
            IFileSystem filesystem)
        {
            _logger = logger;
            _mouseHook = mouseHook;
            _filesystem = filesystem;

            //_mouseHook.MouseMove += _mouseHook_MouseMove;

          


        }

        //private void _mouseHook_MouseMove(object sender, MouseMoveEventArgs e)
        //{
        //    AboutBoxMouseDemoText = $"X:{e.X}, Y:{e.Y}";
        //}

        /// <summary>
        /// Raise the PropertyChanged event to notify the window UI elements that they should update their content.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
