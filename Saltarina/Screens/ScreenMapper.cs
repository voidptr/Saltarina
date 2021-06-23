using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Saltarina.Screens
{
    public class ScreenMapper : IScreenMapper
    {
        private ILogger<ScreenMapper> _logger;
        private Func<ScreenModel> _screenModelFactory;

        public Dictionary<string, ScreenModel> Screens = new Dictionary<string, ScreenModel>();

        public ScreenMapper(ILogger<ScreenMapper> logger,
            Func<ScreenModel> screenModelFactory)
        {
            _logger = logger;
            _screenModelFactory = screenModelFactory;
        }
        private Rectangle _totalBounds;
        public Rectangle TotalBounds
        {
            get {
                if (_totalBounds == Rectangle.Empty)
                {
                    _totalBounds = Screen.AllScreens.Select(screen => screen.Bounds)
                   .Aggregate(Rectangle.Union);
                }
                return _totalBounds;
            }
            private set
            {
                _totalBounds = value;
            }
        }

        public void Map()
        {
            _logger.LogInformation($"{TotalBounds}");

            Screens.Clear();
            foreach (var scrn in Screen.AllScreens)
            {
                var model = _screenModelFactory();
                model.Screen = scrn;
                model.Map();
                Screens.Add(scrn.DeviceName, model);

                //break;
            }
        }        
    }
}
