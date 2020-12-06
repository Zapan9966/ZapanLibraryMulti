using System.Collections.Generic;
using System.Windows.Media;

namespace ZapanControls.Libraries
{
    public class SolidColorBrushComparer : IEqualityComparer<SolidColorBrush>
    {
        public bool Equals(SolidColorBrush x, SolidColorBrush y)
        {
            return x?.Color == y?.Color && x?.Opacity == y?.Opacity;
        }

        public int GetHashCode(SolidColorBrush obj)
        {
            return new { C = obj?.Color, O = obj?.Opacity }.GetHashCode();
        }
    }
}
