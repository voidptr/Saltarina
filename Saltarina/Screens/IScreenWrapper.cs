using System.Drawing;
using System.Windows.Forms;

namespace Saltarina.Screens
{
    public interface IScreenWrapper
    {
        Screen[] AllScreens { get; }
        Rectangle TotalBounds { get; }

        void Reset();
    }
}