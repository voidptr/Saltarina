using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saltarina.Screens
{
    public class ScreenMapper : IScreenMapper
    {
        private ILogger<ScreenMapper> _logger;

        public ScreenMapper(ILogger<ScreenMapper> logger)
        {
            _logger = logger;

        }

        public void Map()
        {
            var totalBounds = Screen.AllScreens.Select(screen => screen.Bounds)
                .Aggregate(Rectangle.Union);

            _logger.LogInformation($"{totalBounds}");

            foreach (var scrn in Screen.AllScreens)
            {
                _logger.LogInformation("----------------------------");
                _logger.LogInformation($"{scrn.DeviceName}");
                _logger.LogInformation($"{scrn.Bounds}");

                var centerX = scrn.Bounds.Left - scrn.Bounds.Right;
                var centerY = scrn.Bounds.Bottom - scrn.Bounds.Top;

                var outLeft = new Point(scrn.Bounds.Left - 1, centerY);
                var outRight = new Point(scrn.Bounds.Right + 1, centerY);
                var outTop = new Point(centerX, scrn.Bounds.Top - 1);
                var outBottom = new Point(centerX, scrn.Bounds.Bottom + 1);

                bool leftEdge = scrn.Bounds.Left.Equals(totalBounds.Left);
                bool rightEdge = scrn.Bounds.Right.Equals(totalBounds.Right);
                bool topEdge = scrn.Bounds.Top.Equals(totalBounds.Top);
                bool bottomEdge = scrn.Bounds.Bottom.Equals(totalBounds.Bottom);

                if (leftEdge)
                    _logger.LogInformation($"Left Edge!");
                if (rightEdge)
                    _logger.LogInformation($"Right Edge!");
                if (topEdge)
                    _logger.LogInformation($"Top Edge!");
                if (bottomEdge)
                    _logger.LogInformation($"Bottom Edge!");

                bool leftConnect = false;
                bool rightConnect = false;
                bool topConnect = false;
                bool bottomConnect = false;
                foreach (var scrn2 in Screen.AllScreens)
                {
                    leftConnect = leftConnect | scrn2.Bounds.Contains(outLeft);
                    rightConnect = rightConnect | scrn2.Bounds.Contains(outRight);
                    topConnect = topConnect | scrn2.Bounds.Contains(outTop);
                    bottomConnect = bottomConnect | scrn2.Bounds.Contains(outTop);
                }

                if (!leftConnect && !leftEdge)
                    _logger.LogInformation($"Interior Disconnected Left");
                if (!rightConnect && !rightEdge)
                    _logger.LogInformation($"Interior Disconnected Right");
                if (!topConnect && !topEdge)
                    _logger.LogInformation($"Interior Disconnected Top");
                if (!bottomConnect && !bottomEdge)
                    _logger.LogInformation($"Interior Disconnected Bottom");
            }
        }
    }
}
