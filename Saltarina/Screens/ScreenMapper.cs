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

        public ScreenMapper(ILogger<ScreenMapper> logger)
        {
            _logger = logger;

        }

        public void Map()
        {
            var totalBounds = Screen.AllScreens.Select(screen => screen.Bounds)
                .Aggregate(Rectangle.Union);

            _logger.LogInformation($"{totalBounds}");

            var scale = (100 * Screen.PrimaryScreen.Bounds.Width / System.Windows.SystemParameters.PrimaryScreenWidth);

            _logger.LogInformation($"{System.Windows.SystemParameters.PrimaryScreenWidth} {scale}");

            foreach (var scrn in Screen.AllScreens)
            {
                _logger.LogInformation("----------------------------");
                _logger.LogInformation($"{scrn.DeviceName}");
                _logger.LogInformation($"{scrn.Bounds} t{scrn.Bounds.Top} b{scrn.Bounds.Bottom} l{scrn.Bounds.Left} r{scrn.Bounds.Right}");

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

                var LeftwardCandidates = new List<Screen>();
                var RightwardCandidates = new List<Screen>();
                var UpwardCandidates = new List<Screen>();
                var DownwardCandidates = new List<Screen>();

                foreach (var scrn2 in Screen.AllScreens)
                {
                    //_logger.LogDebug($"{scrn2.Bounds} t{scrn2.Bounds.Top} b{scrn2.Bounds.Bottom} l{scrn2.Bounds.Left} r{scrn2.Bounds.Right}");
                    if (scrn.Bounds.Equals(scrn2.Bounds))
                    {
                        //_logger.LogDebug(" Same. Skipping");
                        continue;
                    }

                    bool leftLineup = scrn.Bounds.Left == scrn2.Bounds.Right;
                    bool rightLineup = scrn.Bounds.Right == scrn2.Bounds.Left;
                    bool topLineup = scrn.Bounds.Top == scrn2.Bounds.Bottom;
                    bool bottomLineup = scrn.Bounds.Bottom == scrn2.Bounds.Top;
                    //_logger.LogDebug($" Lineup with l{leftLineup} r{rightLineup} t{topLineup} b{bottomLineup}");


                    bool thisLeftConnect = leftLineup && horizontalOverlap(scrn, scrn2);
                    bool thisRightConnect = (rightLineup && horizontalOverlap(scrn, scrn2));
                    bool thisTopConnect = (topLineup && verticalOverlap(scrn, scrn2));
                    bool thisBottomConnect = (bottomLineup && verticalOverlap(scrn, scrn2));
                    //_logger.LogDebug($" Connects on l{thisLeftConnect} r{thisRightConnect} t{thisTopConnect} b{thisBottomConnect}");

                    leftConnect = leftConnect | thisLeftConnect;
                    rightConnect = rightConnect | thisRightConnect;
                    topConnect = topConnect | thisTopConnect;
                    bottomConnect = bottomConnect | thisBottomConnect;

                    if (scrn.Bounds.Left != scrn2.Bounds.Right && horizontalOverlap(scrn, scrn2) && isLeftward(scrn, scrn2))
                    {
                        LeftwardCandidates.Add(scrn2);
                    }

                    if (scrn.Bounds.Right != scrn2.Bounds.Left && horizontalOverlap(scrn, scrn2) && isRightward(scrn, scrn2))
                    {
                        RightwardCandidates.Add(scrn2);
                    }

                    if (scrn.Bounds.Top != scrn2.Bounds.Bottom && verticalOverlap(scrn, scrn2) && isUpward(scrn, scrn2))
                    {
                        UpwardCandidates.Add(scrn2);
                    }

                    if (scrn.Bounds.Bottom != scrn2.Bounds.Top && verticalOverlap(scrn, scrn2) && isDownward(scrn, scrn2))
                    {
                        DownwardCandidates.Add(scrn2);
                    }

                }

                if (!leftConnect && !leftEdge)
                { 
                    _logger.LogInformation($"Interior Disconnected Left");
                    if (LeftwardCandidates.Count > 0)
                    {
                        var candidate = LeftwardCandidates.OrderBy(x => (scrn.Bounds.Left - x.Bounds.Right)).First();
                        _logger.LogInformation($"Candidate: {candidate.DeviceName} {candidate?.Bounds}");
                    }
                }

                if (!rightConnect && !rightEdge)
                {
                    _logger.LogInformation($"Interior Disconnected Right");
                    if (RightwardCandidates.Count > 0)
                    {
                        var candidate = RightwardCandidates.OrderBy(x => (x.Bounds.Left - scrn.Bounds.Right)).First();
                        _logger.LogInformation($"Candidate: {candidate.DeviceName} {candidate?.Bounds}");
                    }
                }
                
                if (!topConnect && !topEdge)
                {
                    _logger.LogInformation($"Interior Disconnected Top");
                    if (UpwardCandidates.Count > 0)
                    {
                        var candidate = UpwardCandidates.OrderBy(x => (x.Bounds.Bottom - scrn.Bounds.Top)).First();
                        _logger.LogInformation($"Candidate: {candidate.DeviceName} {candidate?.Bounds}");
                    }
                }

                if (!bottomConnect && !bottomEdge)
                {
                    _logger.LogInformation($"Interior Disconnected Bottom");
                    if (DownwardCandidates.Count > 0)
                    {
                        var candidate = DownwardCandidates.OrderBy(x => (scrn.Bounds.Bottom - x.Bounds.Top)).First();
                        _logger.LogInformation($"Candidate: {candidate.DeviceName} {candidate?.Bounds}");
                    }
                }
                
            }
        }

        private bool isRightward(Screen scrn1, Screen scrn2)
        {
            if (scrn1.Bounds.Right < scrn2.Bounds.Left)
                return true;
            return false;
        }

        private bool isLeftward(Screen scrn1, Screen scrn2)
        {
            if (scrn1.Bounds.Left > scrn2.Bounds.Right)
                return true;
            return false;
        }

        private bool isUpward(Screen scrn1, Screen scrn2)
        {
            if (scrn1.Bounds.Top > scrn2.Bounds.Bottom)
                return true;
            return false;
        }

        private bool isDownward(Screen scrn1, Screen scrn2)
        {
            if (scrn1.Bounds.Bottom < scrn2.Bounds.Top)
                return true;
            return false;
        }

        private bool verticalOverlap(Screen scrn1, Screen scrn2)
        {
            return verticalOverlap(scrn1.Bounds.Left, scrn1.Bounds.Right, scrn2.Bounds.Left, scrn2.Bounds.Right);
        }

        private bool verticalOverlap(int l1Left, int l1Right, int l2Left, int l2Right)
        {
            if ((l1Left >= l2Right) || (l1Right <= l2Left))
                return false;

            return true;
        }

        private bool horizontalOverlap(Screen scrn1, Screen scrn2)
        {
            return horizontalOverlap(scrn1.Bounds.Top, scrn1.Bounds.Bottom, scrn2.Bounds.Top, scrn2.Bounds.Bottom);
        }
        private bool horizontalOverlap(int l1Top, int l1Bottom, int l2Top, int l2Bottom)
        {
            if ((l1Top >= l2Bottom) || (l1Bottom <= l2Top))
                return false;

            return true;
        }
    }
}
