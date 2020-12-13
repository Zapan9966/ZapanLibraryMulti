// Copyright (C) Josh Smith - January 2007 -w/ mods ju
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ZapanControls.Controls.CalendarPicker
{
    /// <summary>
    /// Renders a visual which can follow the mouse cursor, 
    /// such as during a drag-and-drop operation.
    /// </summary>
    public class DragAdorner : Adorner
    {
        #region Fields
        private readonly Rectangle _rcChild = null;
        private Point _ptOffset = new Point(0,0);
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of DragVisualAdorner.
        /// </summary>
        /// <param name="adornedElement">The element being adorned.</param>
        /// <param name="size">The size of the adorner.</param>
        /// <param name="brush">A brush to with which to paint the adorner.</param>
        public DragAdorner(UIElement adornedElement, Size size, Brush brush)
            : base(adornedElement)
        {
            Rectangle rect = new Rectangle
            {
                Fill = brush,
                Width = size.Width,
                Height = size.Height,
                IsHitTestVisible = false
            };
            _rcChild = rect;
        }
        #endregion

        #region Public Interface
        #region GetDesiredTransform
        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(Offset.X, Offset.Y));
            return result;
        }
        #endregion

        #region Offset
        public Point Offset
        {
            get { return _ptOffset; }
            set
            {
                _ptOffset = value;
                UpdateLocation();
            }
        }
        #endregion
        #endregion

        #region Protected Overrides
        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            _rcChild.Measure(constraint);
            return _rcChild.DesiredSize;
        }

        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _rcChild.Arrange(new Rect(finalSize));
            return finalSize;
        }

        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            return _rcChild;
        }

        /// <summary>
        /// Override.  Always returns 1.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        #endregion

        #region Private Helpers
        private void UpdateLocation()
        {
            if (Parent is AdornerLayer adornerLayer)
                adornerLayer.Update(AdornedElement);
        }
        #endregion
    }
}
