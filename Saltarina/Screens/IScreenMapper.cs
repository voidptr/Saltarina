using System.Drawing;

namespace Saltarina.Screens
{
    public interface IScreenMapper
    {
        Rectangle TotalBounds { get; }
        void Map();
    }
}