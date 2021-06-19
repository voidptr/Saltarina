using Gma.System.MouseKeyHook;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saltarina.MouseHook
{
    public class MouseHook : IMouseHook
    {
        private ILogger<MouseHook> _logger;
        private IKeyboardMouseEvents _globalHook;

        public event EventHandler<MouseMoveEventArgs> MouseMove;

        public MouseHook(ILogger<MouseHook> logger,
            IKeyboardMouseEvents globalHook)
        {
            _logger = logger;
            _globalHook = globalHook;

            _globalHook.MouseMove += _globalHook_MouseMove;
        }

        private void _globalHook_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseMove?.Invoke(this, new MouseMoveEventArgs() { X = e.X, Y = e.Y });
        }
    }

    public class MouseMoveEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
