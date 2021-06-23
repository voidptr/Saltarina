using Microsoft.Extensions.Logging;
using Saltarina.MouseHook;
using Saltarina.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saltarina.MouseControl
{
    public class MouseControl : IMouseControl
    {
        private ILogger<MouseControl> _logger;
        private IMouseHook _mouseHook;
        private IScreenMapper _screenMapper;

        private Screen _currentScreen;
        private bool disposedValue;

        /// <summary>
        /// Watches for hits to the screen boundary, and redirects the mouse as appropriate.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mouseHook"></param>
        /// <param name="screenMapper"></param>
        public MouseControl(ILogger<MouseControl> logger,
            IMouseHook mouseHook, 
            IScreenMapper screenMapper)
        {
            _logger = logger;
            _mouseHook = mouseHook;
            _screenMapper = screenMapper;

            _mouseHook.MouseMove += _mouseHook_MouseMove;
        }

        private void _mouseHook_MouseMove(object sender, MouseMoveEventArgs e)
        {
            var mouse = new System.Drawing.Point(e.X, e.Y);
            var currScreen = Screen.FromPoint(mouse);

            if (!currScreen.Bounds.Contains(mouse))
            {
                _logger.LogTrace($"HITTING Boundary {e.X},{e.Y}, trying to leave {_currentScreen?.DeviceName}");

                Direction? headed = currScreen.IsOutside(mouse);
                if (headed.HasValue)
                {
                    Screen candidate = _screenMapper.Screens[currScreen.DeviceName].Candidates[headed.Value];
                    if (candidate != null)
                    {
                        _logger.LogTrace($"Trying to go: {candidate.DeviceName}");
                        switch (headed.Value)
                        {
                            case Direction.Leftward:
                                MouseMove(candidate.Bounds.Right-1, e.Y);
                                break;
                            case Direction.Rightward:
                                MouseMove(candidate.Bounds.Left, e.Y);
                                break;
                            case Direction.Upward:
                                MouseMove(e.X, candidate.Bounds.Bottom-1);
                                break;
                            case Direction.Downward:
                                MouseMove(e.X, candidate.Bounds.Top);
                                break;
                        }
                    }
                    else
                    {
                        _logger.LogTrace("No candidate");

                    }
                }
            }
            else if (currScreen?.DeviceName != _currentScreen?.DeviceName)
            {
                _logger.LogTrace($"Boundary {e.X},{e.Y}, leaving {_currentScreen?.DeviceName}, entering {currScreen?.DeviceName}");
            }
            _currentScreen = currScreen;
        }

        public void MouseMove(int x, int y)
        {
            MouseControlExtern.MouseMove(x, y);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _mouseHook.MouseMove -= _mouseHook_MouseMove;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public static class MouseControlExtern
    {
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        public static void MouseMove(int x, int y)
        {
            SetCursorPos(x, y);
        }
    }
}
