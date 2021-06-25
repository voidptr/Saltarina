using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

using System.Windows.Input;

namespace Saltarina.ViewModels
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    public class NotifyIconViewModel : INotifyIconViewModel
    {
        private ILogger<NotifyIconViewModel> _logger;
        private Func<MainWindow> _mainWindowFactory;

        public NotifyIconViewModel(ILogger<NotifyIconViewModel> logger,
            Func<MainWindow> mainWindowFactory)
        {
            _logger = logger;
            _mainWindowFactory = mainWindowFactory;

            _autoStartIsChecked = (Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)
                            .GetValue(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductName) != null);
        }

        private bool _autoStartIsChecked;
        public bool AutoStartIsChecked
        {
            get
            {                
                return _autoStartIsChecked;
            }
            set
            {
                if (value)
                {
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)
                            .SetValue(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductName,
                                System.Windows.Forms.Application.ExecutablePath);
                }
                else
                {
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)
                            .DeleteValue(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductName, false);
                }

                _autoStartIsChecked = value;
            }
        }

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow == null,
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow = _mainWindowFactory();
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => Application.Current.MainWindow.Close(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }


        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }

        //public ICommand ToggleAutoStart
        //{
        //    get
        //    {
        //        if (AutoStartIsChecked)
        //        {
        //            _logger.LogDebug($"{nameof(ToggleAutoStart)} checked");
        //            return new DelegateCommand
        //            {
        //                CommandAction = () =>
                            
        //            };
        //        }
        //        else
        //        {
        //            _logger.LogDebug($"{nameof(ToggleAutoStart)} notchecked");
        //            return new DelegateCommand
        //            {
        //                CommandAction = () =>
                            
        //            };
        //        }

                
        //    }
        //}
    }
}
