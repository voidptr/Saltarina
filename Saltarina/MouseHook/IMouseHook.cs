using System;

namespace Saltarina.MouseHook
{
    public interface IMouseHook : IDisposable
    {
        event EventHandler<MouseMoveEventArgs> MouseMove;
    }
}