using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saltarina.Screens
{
    /// <summary>
    /// Non-static wrapper for the Screen.AllScreens static method that is more
    /// friendly for testing and DI
    /// </summary>
    public class ScreenWrapper : IScreenWrapper
    {
        public Screen[] AllScreens => Screen.AllScreens;

        private Rectangle _totalBounds;
        public Rectangle TotalBounds
        {
            get
            {
                if (_totalBounds == Rectangle.Empty)
                {
                    _totalBounds = AllScreens.Select(screen => screen.Bounds)
                   .Aggregate(Rectangle.Union);
                }
                return _totalBounds;
            }
            private set
            {
                _totalBounds = value;
            }
        }

        public void Reset()
        {
            TotalBounds = Rectangle.Empty;
        }
    }
}
