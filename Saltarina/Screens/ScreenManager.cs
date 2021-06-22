using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Saltarina.Screens
{
    public class ScreenManager : IScreenManager
    {
        private ILogger<ScreenManager> _logger;
        private IScreenMapper _screenMapper;
        private bool disposedValue;

        public ScreenManager(ILogger<ScreenManager> logger,
            IScreenMapper screenMapper)
        {
            _logger = logger;
            _screenMapper = screenMapper;

            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

            _screenMapper.Map();
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
