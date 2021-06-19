using System;

namespace Saltarina.MouseHook
{
    public interface IMouseHook
    {
        event EventHandler<MouseMoveEventArgs> MouseMove;
    }
}