using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saltarina.Screens
{    
    public enum Direction
    {
        Leftward,
        Rightward,
        Upward,
        Downward
    }
    public class ScreenModel
    {        
        public Screen Screen { get; set; }

        public Dictionary<Direction, Screen> Candidates = new Dictionary<Direction, Screen>()
            { { Direction.Leftward, null }, { Direction.Rightward, null }, { Direction.Upward, null }, { Direction.Downward, null }  };
        public Dictionary<Direction, bool> IsEdges = new Dictionary<Direction, bool>()
            { { Direction.Leftward, false }, { Direction.Rightward, false }, { Direction.Upward, false }, { Direction.Downward, false }  };
        public Dictionary<Direction, bool> Connected = new Dictionary<Direction, bool>()
            { { Direction.Leftward, false }, { Direction.Rightward, false }, { Direction.Upward, false }, { Direction.Downward, false }  };
        public Dictionary<Direction, bool> InteriorDisconnected = new Dictionary<Direction, bool>()
            { { Direction.Leftward, false }, { Direction.Rightward, false }, { Direction.Upward, false }, { Direction.Downward, false }  };

        private ILogger<ScreenModel> _logger;
        private IScreenMapper _screenMapper;

        public ScreenModel(ILogger<ScreenModel> logger, IScreenMapper screenMapper)
        {
            _screenMapper = screenMapper;
            _logger = logger;
        }

        public void Map()
        {
            if (Screen == null)
                throw new InvalidProgramException("Somehow, we called map without setting Screen :|");

            _logger.LogInformation("---------------------------------------------------------------");
            _logger.LogInformation($"{Screen.DeviceName}");
            _logger.LogInformation($"{Screen.ExtendedScreenBounds()}");

            var totalBounds = _screenMapper.TotalBounds;

            IsEdges[Direction.Leftward] = Screen.Bounds.Left.Equals(totalBounds.Left);
            IsEdges[Direction.Rightward] = Screen.Bounds.Right.Equals(totalBounds.Right);
            IsEdges[Direction.Upward] = Screen.Bounds.Top.Equals(totalBounds.Top);
            IsEdges[Direction.Downward] = Screen.Bounds.Bottom.Equals(totalBounds.Bottom);

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (IsEdges[direction])
                    _logger.LogInformation($"Is {direction} Edge!");
            }


            foreach (var candidate in Screen.AllScreens)
            {
                if (Screen.Bounds.Equals(candidate.Bounds))
                {
                    _logger.LogDebug(" It's me. Skipping.");
                    continue;
                }

                _logger.LogDebug($">> Candidate {candidate.DeviceName}");
                _logger.LogDebug($"   {candidate.ExtendedScreenBounds()}");

                // check each direction
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    _logger.LogDebug($"   Checking {direction}");
                    if (!IsEdges[direction] && !Connected[direction])
                    {
                        if (Screen.Connects(direction, candidate))
                        {
                            Connected[direction] = true;
                            _logger.LogDebug($"     I'm connected to you!");
                        }
                        else
                        {
                            if (Screen.isInDirection(direction, candidate) && Screen.Overlaps(direction, candidate))
                            {
                                if (Candidates[direction] == null)
                                {
                                    Candidates[direction] = candidate;
                                    _logger.LogDebug($"     First Potential candidate! {candidate.DeviceName}");
                                }
                                else if (Screen.Distance(direction, candidate) < Screen.Distance(direction, Candidates[direction]))
                                {
                                    Candidates[direction] = candidate;
                                    _logger.LogDebug($"     Candidate superceded! {candidate.DeviceName}");
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogDebug($"     I'm either an edge or already connected in this direction, skipping.");
                    }
                }
            }

            _logger.LogInformation($"Summary");
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (IsEdges[direction])
                    _logger.LogInformation($" Is Edge in {direction}");
                if (Connected[direction])
                    _logger.LogInformation($" Connected in {direction}");
                if (!IsEdges[direction] && !Connected[direction])
                {
                    InteriorDisconnected[direction] = true;
                    _logger.LogInformation($" Interior Disconnected {direction}!");
                    if (Candidates[direction] != null)
                    {
                        _logger.LogInformation($"  {direction} Candidate Found: {Candidates[direction].DeviceName}!");
                    }
                    else
                    {
                        _logger.LogInformation($"  No {direction} Candidate Found.");
                    }
                }
            }
        }
    }
    public static class ScreenExtensions
    {    
        public static string ExtendedScreenBounds(this Screen screen)
        {
            return $"{screen.Bounds} t{screen.Bounds.Top} b{screen.Bounds.Bottom} l{screen.Bounds.Left} r{screen.Bounds.Right}";
        }

        public static int Distance(this Screen screen, Direction direction, Screen candidate)
        {
            switch (direction)
            {
                case Direction.Leftward:
                    return screen.Bounds.Left - candidate.Bounds.Right;
                case Direction.Rightward:
                    return candidate.Bounds.Left - screen.Bounds.Right;
                case Direction.Upward:
                    return candidate.Bounds.Bottom - screen.Bounds.Top;
                case Direction.Downward:
                    return screen.Bounds.Bottom - candidate.Bounds.Top;
                default:
                    throw new InvalidProgramException("Somehow, we bugged on the Direction!");
            }
        }

        public static bool Connects(this Screen screen, Direction direction, Screen candidate)
        {
            switch (direction)
            {
                case Direction.Leftward:
                    return screen.Bounds.Left == candidate.Bounds.Right && screen.OverlapsHorizontally(candidate);
                case Direction.Rightward:
                    return screen.Bounds.Right == candidate.Bounds.Left && screen.OverlapsHorizontally(candidate);
                case Direction.Upward:
                    return screen.Bounds.Top == candidate.Bounds.Bottom && screen.OverlapsVertically(candidate);
                case Direction.Downward:
                    return screen.Bounds.Bottom == candidate.Bounds.Top && screen.OverlapsVertically(candidate);
                default:
                    throw new InvalidProgramException("Somehow, we bugged on the Direction!");
            }
        }

        public static bool isInDirection(this Screen screen, Direction direction, Screen candidate)
        {
            switch (direction)
            {
                case Direction.Leftward:
                    return screen.Bounds.Left > candidate.Bounds.Right;
                case Direction.Rightward:
                    return screen.Bounds.Right < candidate.Bounds.Left;
                case Direction.Upward:
                    return screen.Bounds.Top > candidate.Bounds.Bottom;
                case Direction.Downward:
                    return screen.Bounds.Bottom < candidate.Bounds.Top;
                default:
                    throw new InvalidProgramException("Somehow, we bugged on the Direction!");
            }
        }

        public static bool Overlaps(this Screen screen, Direction direction, Screen candidate)
        {
            switch (direction)
            {
                case Direction.Leftward:
                case Direction.Rightward:
                    return OverlapsHorizontally(screen, candidate);
                case Direction.Upward:
                case Direction.Downward:
                    return OverlapsVertically(screen, candidate);
                default:
                    throw new InvalidProgramException("Somehow, we bugged on the Direction!");
            }
        }

        /// <summary>
        /// Reject all screens that don't overlap vertically
        /// line1 ----- 
        /// line2        -----  
        ///       or
        /// line1        -----
        /// line2 -----
        /// </summary>
        /// <param name="candidate">source of line2</param>
        /// <returns></returns>
        private static bool OverlapsVertically(this Screen screen, Screen candidate)
        {
            return Overlaps(screen.Bounds.Left, screen.Bounds.Right, candidate.Bounds.Left, candidate.Bounds.Right);
        }
        /// <summary>
        /// Reject all screens that don't overlap horizontally
        /// line1  line2
        ///   |
        ///   |
        ///    
        ///          |
        ///          |
        /// or viceversa
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private static bool OverlapsHorizontally(this Screen screen, Screen candidate)
        {
            return Overlaps(screen.Bounds.Top, screen.Bounds.Bottom, candidate.Bounds.Top, candidate.Bounds.Bottom);
        }

        /// <summary>
        /// The logic works the same for either horizontal or vertical lines as 
        /// long as the coordinate system is 0,0 (or negatives) to the top/left, such that
        /// begin -> left/top and end -> right/bottom. That is left/top is the lower number,
        /// and right/bottom is the higher number.
        /// </summary>
        /// <param name="l1Begin">Left or Top</param>
        /// <param name="l1End">Right or Bottom</param>
        /// <param name="l2Begin">Left or Top</param>
        /// <param name="l2End">Right or Bottom</param>
        /// <returns></returns>
        private static bool Overlaps(int l1Begin, int l1End, int l2Begin, int l2End)
        {
            return !((l1Begin >= l2End) || (l1End <= l2Begin));                
        }
    }
}
