using Microsoft.Extensions.Logging;
using Saltarina.MouseControl;
using Saltarina.Screens;

namespace Saltarina
{
    public class Service
    {
        private ILogger<Service> _logger;
        private IDisplayManager _screenManager;
        private IMouseControl _mouseControl;

        public Service(ILogger<Service> logger,
            IDisplayManager screenManager,
            IMouseControl mouseControl)
        {
            _logger = logger;
            _screenManager = screenManager;
            _mouseControl = mouseControl;
        }
    }
}
