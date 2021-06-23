using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Saltarina.Screens
{
    public class DisplayManager : IDisplayManager
    {
        private ILogger<DisplayManager> _logger;
        private IScreenMapper _screenMapper;
        private bool disposedValue;

        /// <summary>
        /// Listens for changes to the display settings and prompts a re-mapping
        /// </summary>
        public DisplayManager(ILogger<DisplayManager> logger,
            IScreenMapper screenMapper)
        {
            _logger = logger;
            _screenMapper = screenMapper;

            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, System.EventArgs e)
        {
            _logger.LogInformation("Screen Settings Changed!");
            _screenMapper.Map();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
