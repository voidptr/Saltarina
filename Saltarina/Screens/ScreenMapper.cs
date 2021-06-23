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
        private IScreenWrapper _screenWrapper;

        public Dictionary<string, ScreenModel> Screens { get; } = new Dictionary<string, ScreenModel>();

        public bool IsMapped { get; private set; }

        /// <summary>
        /// Builds a map of the connected displays.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="screenModelFactory"></param>
        /// <param name="totalScreenBounds"></param>
        /// <param name="screenWrapper"></param>
        public ScreenMapper(ILogger<ScreenMapper> logger,
            Func<ScreenModel> screenModelFactory,
            IScreenWrapper screenWrapper)
        {
            _logger = logger;
            _screenModelFactory = screenModelFactory;
            _screenWrapper = screenWrapper;

            Map();
        }

        public void Map()
        {
            IsMapped = false;

            _screenWrapper.Reset();
            _logger.LogInformation($"{_screenWrapper.TotalBounds}");

            Screens.Clear();
            foreach (var scrn in _screenWrapper.AllScreens)
            {
                var model = _screenModelFactory();
                model.Screen = scrn;
                model.Map();
                Screens.Add(scrn.DeviceName, model);
            }

            IsMapped = true;
        }
    }
}
