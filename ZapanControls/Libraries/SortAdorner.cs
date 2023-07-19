using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ZapanControls.Libraries
{
    public sealed class SortAdorner : Adorner
    {
        private static readonly Geometry _ascGeometry = Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");
        private static readonly Geometry _descGeometry = Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext?.PushTransform(transform);

            Geometry geometry = _ascGeometry;
            if (Direction == ListSortDirection.Descending)
            {
                geometry = _descGeometry;
            }
            drawingContext?.DrawGeometry(Brushes.Black, null, geometry);
            drawingContext?.Pop();
        }
    }

}
