using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saltarina.MouseControl
{
    public class MouseControl : IMouseControl
    {
        private ILogger<MouseControl> _logger;
        public MouseControl(ILogger<MouseControl> logger)
        {
            _logger = logger;
        }

        public void MouseMove(int x, int y)
        {
            MouseControlExtern.MouseMove(x, y);
        }
    }

    public static class MouseControlExtern
    {
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        /// <summary>
        /// moves the mouse
        /// </summary>
        /// <param name="x">x position to move to</param>
        /// <param name="y">y position to move to</param>
        public static void MouseMove(int x, int y)
        {
            SetCursorPos(x, y);
        }
    }
}
