#region Directives
using System.Windows;
#endregion

namespace ZapanControls.Controls.ResourceKeys
{
    public static class CalendarResourceKeys 
    {
        #region Fields
        // Header
        private static readonly ComponentResourceKey _HeaderNormalForegroundKey = null;
        private static readonly ComponentResourceKey _HeaderFocusedForegroundKey = null;
        private static readonly ComponentResourceKey _HeaderPressedForegroundKey = null;

        private static readonly ComponentResourceKey _HeaderNormalBorderBrushKey = null;
        private static readonly ComponentResourceKey _HeaderFocusedBorderBrushKey = null;
        private static readonly ComponentResourceKey _HeaderPressedBorderBrushKey = null;

        private static readonly ComponentResourceKey _HeaderNormalBackgroundKey = null;
        private static readonly ComponentResourceKey _HeaderFocusedBackgroundKey = null;
        private static readonly ComponentResourceKey _HeaderPressedBackgroundKey = null;

        // Direction buttons
        private static readonly ComponentResourceKey _ArrowBorderBrushKey = null;
        private static readonly ComponentResourceKey _ArrowNormalFillKey = null;
        private static readonly ComponentResourceKey _ArrowFocusedFillKey = null;
        private static readonly ComponentResourceKey _ArrowPressedFillKey = null;

        // Date buttons
        // normal
        private static readonly ComponentResourceKey _ButtonNormalForegroundKey = null;
        private static readonly ComponentResourceKey _ButtonNormalBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonNormalBackgroundKey = null;
        // focused
        private static readonly ComponentResourceKey _ButtonFocusedForegroundKey = null;
        private static readonly ComponentResourceKey _ButtonFocusedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonFocusedBackgroundKey = null;
        // selected 
        private static readonly ComponentResourceKey _ButtonSelectedForegroundKey = null;
        private static readonly ComponentResourceKey _ButtonSelectedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonSelectedBackgroundKey = null;
        // pressed
        private static readonly ComponentResourceKey _ButtonPressedForegroundKey = null;
        private static readonly ComponentResourceKey _ButtonPressedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonPressedBackgroundKey = null;
        // defaulted
        private static readonly ComponentResourceKey _ButtonDefaultedForegroundKey = null;
        private static readonly ComponentResourceKey _ButtonDefaultedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonDefaultedBackgroundKey = null;
        // disabled
        private static readonly ComponentResourceKey _ButtonTransparentKey = null;
        private static readonly ComponentResourceKey _ButtonDisabledForegroundKey = null;
        private static readonly ComponentResourceKey _ButtonDisabledBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonDisabledBackgroundKey = null;

        // Week column
        private static readonly ComponentResourceKey _WeekColumnForegroundKey = null;
        private static readonly ComponentResourceKey _WeekColumnBackgroundKey = null;
        private static readonly ComponentResourceKey _WeekColumnBorderBrushKey = null;

        // Footer
        private static readonly ComponentResourceKey _FooterForegroundKey = null;
        private static readonly ComponentResourceKey _FooterBorderBrushKey = null;
        private static readonly ComponentResourceKey _FooterBackgroundKey = null;

        // Day column
        private static readonly ComponentResourceKey _DayNamesForegroundKey = null;
        private static readonly ComponentResourceKey _DayNamesBorderBrushKey = null;
        private static readonly ComponentResourceKey _DayNamesBackgroundKey = null;

        // Control
        private static readonly ComponentResourceKey _ControlBorderBrushKey = null;
        private static readonly ComponentResourceKey _ControlBackgroundKey = null;
        #endregion

        #region Resource Keys
        #region Control
        /// <summary>
        /// Control border
        /// </summary>
        public static ComponentResourceKey ControlBorderBrushKey
        {
            get { return _ControlBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ControlBorderBrush"); }
        }

        /// <summary>
        /// Control background
        /// </summary>
        public static ComponentResourceKey ControlBackgroundKey
        {
            get { return _ControlBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ControlBackground"); }
        }
        #endregion

        #region Header
        /// <summary>
        /// Header normal foreground
        /// </summary>
        public static ComponentResourceKey HeaderNormalForegroundKey
        {
            get { return _HeaderNormalForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderNormalForeground"); }
        }

        /// <summary>
        /// Header focused foreground
        /// </summary>
        public static ComponentResourceKey HeaderFocusedForegroundKey
        {
            get { return _HeaderFocusedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderFocusedForeground"); }
        }

        /// <summary>
        /// Header pressed foreground
        /// </summary>
        public static ComponentResourceKey HeaderPressedForegroundKey
        {
            get { return _HeaderPressedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderPressedForeground"); }
        }

        /// <summary>
        /// Header normal border brush
        /// </summary>
        public static ComponentResourceKey HeaderNormalBorderBrushKey
        {
            get { return _HeaderNormalBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderNormalBorderBrush"); }
        }

        /// <summary>
        /// Header focused border brush
        /// </summary>
        public static ComponentResourceKey HeaderFocusedBorderBrushKey
        {
            get { return _HeaderFocusedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderFocusedBorderBrush"); }
        }

        /// <summary>
        /// Header pressed border brush
        /// </summary>
        public static ComponentResourceKey HeaderPressedBorderBrushKey
        {
            get { return _HeaderPressedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderPressedBorderBrush"); }
        }

        /// <summary>
        /// Header normal background brush
        /// </summary>
        public static ComponentResourceKey HeaderNormalBackgroundKey
        {
            get { return _HeaderNormalBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderNormalBackground"); }
        }

        /// <summary>
        /// Header focused background 
        /// </summary>
        public static ComponentResourceKey HeaderFocusedBackgroundKey
        {
            get { return _HeaderFocusedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderFocusedBackground"); }
        }

        /// <summary>
        /// Header pressed background 
        /// </summary>
        public static ComponentResourceKey HeaderPressedBackgroundKey
        {
            get { return _HeaderPressedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderPressedBackground"); }
        }
        #endregion

        #region Navigation Buttons
        /// <summary>
        /// Direction arrow normal fill
        /// </summary>
        public static ComponentResourceKey ArrowBorderBrushKey
        {
            get { return _ArrowBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowBorderBrush"); }
        }

        /// <summary>
        /// Direction arrow normal fill
        /// </summary>
        public static ComponentResourceKey ArrowNormalFillKey
        {
            get { return _ArrowNormalFillKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowNormalFill"); }
        }

        /// <summary>
        /// Direction arrow focused fill
        /// </summary>
        public static ComponentResourceKey ArrowFocusedFillKey
        {
            get { return _ArrowFocusedFillKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowFocusedFill"); }
        }

        /// <summary>
        /// Direction arrow pressed fill
        /// </summary>
        public static ComponentResourceKey ArrowPressedFillKey
        {
            get { return _ArrowPressedFillKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowPressedFill"); }
        }
        #endregion

        #region Day Column
        /// <summary>
        /// Day names foreground
        /// </summary>
        public static ComponentResourceKey DayNamesForegroundKey
        {
            get { return _DayNamesForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "DayNamesForeground"); }
        }

        /// <summary>
        /// Day names border
        /// </summary>
        public static ComponentResourceKey DayNamesBorderBrushKey
        {
            get { return _DayNamesBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "DayNamesBorderBrush"); }
        }

        /// <summary>
        /// Day names background
        /// </summary>
        public static ComponentResourceKey DayNamesBackgroundKey
        {
            get { return _DayNamesBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "DayNamesBackground"); }
        }
        #endregion

        #region Week Column
        /// <summary>
        /// Week column foreground
        /// </summary>
        public static ComponentResourceKey WeekColumnForegroundKey
        {
            get { return _WeekColumnForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "WeekColumnForeground"); }
        }

        /// <summary>
        /// Week column border
        /// </summary>
        public static ComponentResourceKey WeekColumnBorderBrushKey
        {
            get { return _WeekColumnBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "WeekColumnBorderBrush"); }
        }

        /// <summary>
        /// Week column background
        /// </summary>
        public static ComponentResourceKey WeekColumnBackgroundKey
        {
            get { return _WeekColumnBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "WeekColumnBackground"); }
        }
        #endregion

        #region Button
        #region Normal
        /// <summary>
        /// Button normal foreground
        /// </summary>
        public static ComponentResourceKey ButtonNormalForegroundKey
        {
            get { return _ButtonNormalForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonNormalForeground"); }
        }

        /// <summary>
        /// Button normal border
        /// </summary>
        public static ComponentResourceKey ButtonNormalBorderBrushKey
        {
            get { return _ButtonNormalBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonNormalBorderBrush"); }
        }

        /// <summary>
        /// Button normal background
        /// </summary>
        public static ComponentResourceKey ButtonNormalBackgroundKey
        {
            get { return _ButtonNormalBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonNormalBackground"); }
        }
        #endregion

        #region Focused
        /// <summary>
        /// Button focused foreground
        /// </summary>
        public static ComponentResourceKey ButtonFocusedForegroundKey
        {
            get { return _ButtonFocusedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonFocusedForeground"); }
        }

        /// <summary>
        /// Button focused border
        /// </summary>
        public static ComponentResourceKey ButtonFocusedBorderBrushKey
        {
            get { return _ButtonFocusedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonFocusedBorderBrush"); }
        }

        /// <summary>
        /// Button focused background
        /// </summary>
        public static ComponentResourceKey ButtonFocusedBackgroundKey
        {
            get { return _ButtonFocusedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonFocusedBackground"); }
        }
        #endregion

        #region Selected
        /// <summary>
        /// Button selected foreground
        /// </summary>
        public static ComponentResourceKey ButtonSelectedForegroundKey
        {
            get { return _ButtonSelectedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonSelectedForeground"); }
        }

        /// <summary>
        /// Button selected border
        /// </summary>
        public static ComponentResourceKey ButtonSelectedBorderBrushKey
        {
            get { return _ButtonSelectedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonSelectedBorderBrush"); }
        }

        /// <summary>
        /// Button selected background
        /// </summary>
        public static ComponentResourceKey ButtonSelectedBackgroundKey
        {
            get { return _ButtonSelectedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonSelectedBackground"); }
        }
        #endregion

        #region Defaulted
        /// <summary>
        /// Button defaulted foreground
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedForegroundKey
        {
            get { return _ButtonDefaultedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDefaultedForeground"); }
        }

        /// <summary>
        /// Button defaulted border
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedBorderBrushKey
        {
            get { return _ButtonDefaultedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDefaultedBorderBrush"); }
        }

        /// <summary>
        /// Button selected background
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedBackgroundKey
        {
            get { return _ButtonDefaultedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDefaultedBackground"); }
        }
        #endregion

        #region Pressed
        /// <summary>
        /// Button pressed foreground
        /// </summary>
        public static ComponentResourceKey ButtonPressedForegroundKey
        {
            get { return _ButtonPressedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonPressedForeground"); }
        }

        /// <summary>
        /// Button pressed border
        /// </summary>
        public static ComponentResourceKey ButtonPressedBorderBrushKey
        {
            get { return _ButtonPressedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonPressedBorderBrush"); }
        }

        /// <summary>
        /// Button pressed background
        /// </summary>
        public static ComponentResourceKey ButtonPressedBackgroundKey
        {
            get { return _ButtonPressedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonPressedBackground"); }
        }
        #endregion

        #region Disabled
        /// <summary>
        /// Transparent 
        /// </summary>
        public static ComponentResourceKey ButtonTransparentKey
        {
            get { return _ButtonTransparentKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonTransparent"); }
        }

        /// <summary>
        /// Button disabled foreground
        /// </summary>
        public static ComponentResourceKey ButtonDisabledForegroundKey
        {
            get { return _ButtonDisabledForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDisabledForeground"); }
        }

        /// <summary>
        /// Button disabled border
        /// </summary>
        public static ComponentResourceKey ButtonDisabledBorderBrushKey
        {
            get { return _ButtonDisabledBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDisabledBorderBrush"); }
        }

        /// <summary>
        /// Button disabled background
        /// </summary>
        public static ComponentResourceKey ButtonDisabledBackgroundKey
        {
            get { return _ButtonDisabledBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDisabledBackground"); }
        }
        #endregion
        #endregion

        #region Footer
        /// <summary>
        /// Footer foreground
        /// </summary>
        public static ComponentResourceKey FooterForegroundKey
        {
            get { return _FooterForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "FooterForeground"); }
        }

        /// <summary>
        /// Footer border
        /// </summary>
        public static ComponentResourceKey FooterBorderBrushKey
        {
            get { return _FooterBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "FooterBorderBrush"); }
        }

        /// <summary>
        /// Footer background
        /// </summary>
        public static ComponentResourceKey FooterBackgroundKey
        {
            get { return _FooterBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "FooterBackground"); }
        }
        #endregion
        #endregion
    }
}
