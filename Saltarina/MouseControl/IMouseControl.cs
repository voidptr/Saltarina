using System;

namespace Saltarina.MouseControl
{
    public interface IMouseControl : IDisposable
    {
        void MouseMove(int x, int y);
    }
}