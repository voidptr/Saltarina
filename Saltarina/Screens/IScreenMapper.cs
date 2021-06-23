using System.Collections.Generic;
using System.Drawing;

namespace Saltarina.Screens
{
    public interface IScreenMapper
    {
        bool IsMapped { get; }
        Dictionary<string, ScreenModel> Screens { get; }

        void Map();
    }
}