using System.Collections.Generic;
using System.Windows.Media;

namespace ZapanControls.Libraries
{
    public sealed class SolidColorBrushComparer : IEqualityComparer<SolidColorBrush>
    {
        public bool Equals(SolidColorBrush x, SolidColorBrush y)
            => x?.Color == y?.Color && x?.Opacity == y?.Opacity;

        public int GetHashCode(SolidColorBrush obj)
            => new { C = obj?.Color, O = obj?.Opacity }.GetHashCode();
    }
}
