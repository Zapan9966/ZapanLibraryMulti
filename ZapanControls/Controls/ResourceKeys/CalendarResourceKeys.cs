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
        public static ComponentResourceKey ControlBorderBrushKey => _ControlBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ControlBorderBrush"); 
        /// <summary>
        /// Control background
        /// </summary>
        public static ComponentResourceKey ControlBackgroundKey => _ControlBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ControlBackground");        
        #endregion

        #region Header
        /// <summary>
        /// Header normal foreground
        /// </summary>
        public static ComponentResourceKey HeaderNormalForegroundKey => _HeaderNormalForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderNormalForeground");
        /// <summary>
        /// Header focused foreground
        /// </summary>
        public static ComponentResourceKey HeaderFocusedForegroundKey => _HeaderFocusedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderFocusedForeground");
        /// <summary>
        /// Header pressed foreground
        /// </summary>
        public static ComponentResourceKey HeaderPressedForegroundKey => _HeaderPressedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderPressedForeground");
        /// <summary>
        /// Header normal border brush
        /// </summary>
        public static ComponentResourceKey HeaderNormalBorderBrushKey => _HeaderNormalBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderNormalBorderBrush");
        /// <summary>
        /// Header focused border brush
        /// </summary>
        public static ComponentResourceKey HeaderFocusedBorderBrushKey => _HeaderFocusedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderFocusedBorderBrush");
        /// <summary>
        /// Header pressed border brush
        /// </summary>
        public static ComponentResourceKey HeaderPressedBorderBrushKey => _HeaderPressedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderPressedBorderBrush");
        /// <summary>
        /// Header normal background brush
        /// </summary>
        public static ComponentResourceKey HeaderNormalBackgroundKey => _HeaderNormalBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderNormalBackground");
        /// <summary>
        /// Header focused background 
        /// </summary>
        public static ComponentResourceKey HeaderFocusedBackgroundKey => _HeaderFocusedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderFocusedBackground");
        /// <summary>
        /// Header pressed background 
        /// </summary>
        public static ComponentResourceKey HeaderPressedBackgroundKey => _HeaderPressedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "HeaderPressedBackground");
        #endregion

        #region Navigation Buttons
        /// <summary>
        /// Direction arrow normal fill
        /// </summary>
        public static ComponentResourceKey ArrowBorderBrushKey => _ArrowBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowBorderBrush");
        /// <summary>
        /// Direction arrow normal fill
        /// </summary>
        public static ComponentResourceKey ArrowNormalFillKey => _ArrowNormalFillKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowNormalFill");
        /// <summary>
        /// Direction arrow focused fill
        /// </summary>
        public static ComponentResourceKey ArrowFocusedFillKey => _ArrowFocusedFillKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowFocusedFill");
        /// <summary>
        /// Direction arrow pressed fill
        /// </summary>
        public static ComponentResourceKey ArrowPressedFillKey => _ArrowPressedFillKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ArrowPressedFill");
        #endregion

        #region Day Column
        /// <summary>
        /// Day names foreground
        /// </summary>
        public static ComponentResourceKey DayNamesForegroundKey => _DayNamesForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "DayNamesForeground");
        /// <summary>
        /// Day names border
        /// </summary>
        public static ComponentResourceKey DayNamesBorderBrushKey => _DayNamesBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "DayNamesBorderBrush");
        /// <summary>
        /// Day names background
        /// </summary>
        public static ComponentResourceKey DayNamesBackgroundKey => _DayNamesBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "DayNamesBackground");
        #endregion

        #region Week Column
        /// <summary>
        /// Week column foreground
        /// </summary>
        public static ComponentResourceKey WeekColumnForegroundKey => _WeekColumnForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "WeekColumnForeground");
        /// <summary>
        /// Week column border
        /// </summary>
        public static ComponentResourceKey WeekColumnBorderBrushKey => _WeekColumnBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "WeekColumnBorderBrush");
        /// <summary>
        /// Week column background
        /// </summary>
        public static ComponentResourceKey WeekColumnBackgroundKey => _WeekColumnBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "WeekColumnBackground");
        #endregion

        #region Button
        #region Normal
        /// <summary>
        /// Button normal foreground
        /// </summary>
        public static ComponentResourceKey ButtonNormalForegroundKey => _ButtonNormalForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonNormalForeground");
        /// <summary>
        /// Button normal border
        /// </summary>
        public static ComponentResourceKey ButtonNormalBorderBrushKey => _ButtonNormalBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonNormalBorderBrush");
        /// <summary>
        /// Button normal background
        /// </summary>
        public static ComponentResourceKey ButtonNormalBackgroundKey => _ButtonNormalBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonNormalBackground");
        #endregion

        #region Focused
        /// <summary>
        /// Button focused foreground
        /// </summary>
        public static ComponentResourceKey ButtonFocusedForegroundKey => _ButtonFocusedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonFocusedForeground");
        /// <summary>
        /// Button focused border
        /// </summary>
        public static ComponentResourceKey ButtonFocusedBorderBrushKey => _ButtonFocusedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonFocusedBorderBrush");
        /// <summary>
        /// Button focused background
        /// </summary>
        public static ComponentResourceKey ButtonFocusedBackgroundKey => _ButtonFocusedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonFocusedBackground");
        #endregion

        #region Selected
        /// <summary>
        /// Button selected foreground
        /// </summary>
        public static ComponentResourceKey ButtonSelectedForegroundKey => _ButtonSelectedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonSelectedForeground");
        /// <summary>
        /// Button selected border
        /// </summary>
        public static ComponentResourceKey ButtonSelectedBorderBrushKey => _ButtonSelectedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonSelectedBorderBrush");
        /// <summary>
        /// Button selected background
        /// </summary>
        public static ComponentResourceKey ButtonSelectedBackgroundKey => _ButtonSelectedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonSelectedBackground");
        #endregion

        #region Defaulted
        /// <summary>
        /// Button defaulted foreground
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedForegroundKey => _ButtonDefaultedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDefaultedForeground");
        /// <summary>
        /// Button defaulted border
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedBorderBrushKey => _ButtonDefaultedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDefaultedBorderBrush");
        /// <summary>
        /// Button selected background
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedBackgroundKey => _ButtonDefaultedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDefaultedBackground");
        #endregion

        #region Pressed
        /// <summary>
        /// Button pressed foreground
        /// </summary>
        public static ComponentResourceKey ButtonPressedForegroundKey => _ButtonPressedForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonPressedForeground");
        /// <summary>
        /// Button pressed border
        /// </summary>
        public static ComponentResourceKey ButtonPressedBorderBrushKey => _ButtonPressedBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonPressedBorderBrush");
        /// <summary>
        /// Button pressed background
        /// </summary>
        public static ComponentResourceKey ButtonPressedBackgroundKey => _ButtonPressedBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonPressedBackground");
        #endregion

        #region Disabled
        /// <summary>
        /// Transparent 
        /// </summary>
        public static ComponentResourceKey ButtonTransparentKey => _ButtonTransparentKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonTransparent");
        /// <summary>
        /// Button disabled foreground
        /// </summary>
        public static ComponentResourceKey ButtonDisabledForegroundKey => _ButtonDisabledForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDisabledForeground");
        /// <summary>
        /// Button disabled border
        /// </summary>
        public static ComponentResourceKey ButtonDisabledBorderBrushKey => _ButtonDisabledBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDisabledBorderBrush");
        /// <summary>
        /// Button disabled background
        /// </summary>
        public static ComponentResourceKey ButtonDisabledBackgroundKey => _ButtonDisabledBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "ButtonDisabledBackground");
        #endregion
        #endregion

        #region Footer
        /// <summary>
        /// Footer foreground
        /// </summary>
        public static ComponentResourceKey FooterForegroundKey => _FooterForegroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "FooterForeground");
        /// <summary>
        /// Footer border
        /// </summary>
        public static ComponentResourceKey FooterBorderBrushKey => _FooterBorderBrushKey.GetRegisteredKey(typeof(CalendarResourceKeys), "FooterBorderBrush");
        /// <summary>
        /// Footer background
        /// </summary>
        public static ComponentResourceKey FooterBackgroundKey => _FooterBackgroundKey.GetRegisteredKey(typeof(CalendarResourceKeys), "FooterBackground");
        #endregion
        #endregion
    }
}
