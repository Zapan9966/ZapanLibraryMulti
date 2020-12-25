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
            get { return GetRegisteredKey(_ControlBorderBrushKey, "ControlBorderBrush"); }
        }

        /// <summary>
        /// Control background
        /// </summary>
        public static ComponentResourceKey ControlBackgroundKey
        {
            get { return GetRegisteredKey(_ControlBackgroundKey, "ControlBackground"); }
        }
        #endregion

        #region Header
        /// <summary>
        /// Header normal foreground
        /// </summary>
        public static ComponentResourceKey HeaderNormalForegroundKey
        {
            get { return GetRegisteredKey(_HeaderNormalForegroundKey, "HeaderNormalForeground"); }
        }

        /// <summary>
        /// Header focused foreground
        /// </summary>
        public static ComponentResourceKey HeaderFocusedForegroundKey
        {
            get { return GetRegisteredKey(_HeaderFocusedForegroundKey, "HeaderFocusedForeground"); }
        }

        /// <summary>
        /// Header pressed foreground
        /// </summary>
        public static ComponentResourceKey HeaderPressedForegroundKey
        {
            get { return GetRegisteredKey(_HeaderPressedForegroundKey, "HeaderPressedForeground"); }
        }

        /// <summary>
        /// Header normal border brush
        /// </summary>
        public static ComponentResourceKey HeaderNormalBorderBrushKey
        {
            get { return GetRegisteredKey(_HeaderNormalBorderBrushKey, "HeaderNormalBorderBrush"); }
        }

        /// <summary>
        /// Header focused border brush
        /// </summary>
        public static ComponentResourceKey HeaderFocusedBorderBrushKey
        {
            get { return GetRegisteredKey(_HeaderFocusedBorderBrushKey, "HeaderFocusedBorderBrush"); }
        }

        /// <summary>
        /// Header pressed border brush
        /// </summary>
        public static ComponentResourceKey HeaderPressedBorderBrushKey
        {
            get { return GetRegisteredKey(_HeaderPressedBorderBrushKey, "HeaderPressedBorderBrush"); }
        }

        /// <summary>
        /// Header normal background brush
        /// </summary>
        public static ComponentResourceKey HeaderNormalBackgroundKey
        {
            get { return GetRegisteredKey(_HeaderNormalBackgroundKey, "HeaderNormalBackground"); }
        }

        /// <summary>
        /// Header focused background 
        /// </summary>
        public static ComponentResourceKey HeaderFocusedBackgroundKey
        {
            get { return GetRegisteredKey(_HeaderFocusedBackgroundKey, "HeaderFocusedBackground"); }
        }

        /// <summary>
        /// Header pressed background 
        /// </summary>
        public static ComponentResourceKey HeaderPressedBackgroundKey
        {
            get { return GetRegisteredKey(_HeaderPressedBackgroundKey, "HeaderPressedBackground"); }
        }
        #endregion

        #region Navigation Buttons
        /// <summary>
        /// Direction arrow normal fill
        /// </summary>
        public static ComponentResourceKey ArrowBorderBrushKey
        {
            get { return GetRegisteredKey(_ArrowBorderBrushKey, "ArrowBorderBrush"); }
        }

        /// <summary>
        /// Direction arrow normal fill
        /// </summary>
        public static ComponentResourceKey ArrowNormalFillKey
        {
            get { return GetRegisteredKey(_ArrowNormalFillKey, "ArrowNormalFill"); }
        }

        /// <summary>
        /// Direction arrow focused fill
        /// </summary>
        public static ComponentResourceKey ArrowFocusedFillKey
        {
            get { return GetRegisteredKey(_ArrowFocusedFillKey, "ArrowFocusedFill"); }
        }

        /// <summary>
        /// Direction arrow pressed fill
        /// </summary>
        public static ComponentResourceKey ArrowPressedFillKey
        {
            get { return GetRegisteredKey(_ArrowPressedFillKey, "ArrowPressedFill"); }
        }
        #endregion

        #region Day Column
        /// <summary>
        /// Day names foreground
        /// </summary>
        public static ComponentResourceKey DayNamesForegroundKey
        {
            get { return GetRegisteredKey(_DayNamesForegroundKey, "DayNamesForeground"); }
        }

        /// <summary>
        /// Day names border
        /// </summary>
        public static ComponentResourceKey DayNamesBorderBrushKey
        {
            get { return GetRegisteredKey(_DayNamesBorderBrushKey, "DayNamesBorderBrush"); }
        }

        /// <summary>
        /// Day names background
        /// </summary>
        public static ComponentResourceKey DayNamesBackgroundKey
        {
            get { return GetRegisteredKey(_DayNamesBackgroundKey, "DayNamesBackground"); }
        }
        #endregion

        #region Week Column
        /// <summary>
        /// Week column foreground
        /// </summary>
        public static ComponentResourceKey WeekColumnForegroundKey
        {
            get { return GetRegisteredKey(_WeekColumnForegroundKey, "WeekColumnForeground"); }
        }

        /// <summary>
        /// Week column border
        /// </summary>
        public static ComponentResourceKey WeekColumnBorderBrushKey
        {
            get { return GetRegisteredKey(_WeekColumnBorderBrushKey, "WeekColumnBorderBrush"); }
        }

        /// <summary>
        /// Week column background
        /// </summary>
        public static ComponentResourceKey WeekColumnBackgroundKey
        {
            get { return GetRegisteredKey(_WeekColumnBackgroundKey, "WeekColumnBackground"); }
        }
        #endregion

        #region Button
        #region Normal
        /// <summary>
        /// Button normal foreground
        /// </summary>
        public static ComponentResourceKey ButtonNormalForegroundKey
        {
            get { return GetRegisteredKey(_ButtonNormalForegroundKey, "ButtonNormalForeground"); }
        }

        /// <summary>
        /// Button normal border
        /// </summary>
        public static ComponentResourceKey ButtonNormalBorderBrushKey
        {
            get { return GetRegisteredKey(_ButtonNormalBorderBrushKey, "ButtonNormalBorderBrush"); }
        }

        /// <summary>
        /// Button normal background
        /// </summary>
        public static ComponentResourceKey ButtonNormalBackgroundKey
        {
            get { return GetRegisteredKey(_ButtonNormalBackgroundKey, "ButtonNormalBackground"); }
        }
        #endregion

        #region Focused
        /// <summary>
        /// Button focused foreground
        /// </summary>
        public static ComponentResourceKey ButtonFocusedForegroundKey
        {
            get { return GetRegisteredKey(_ButtonFocusedForegroundKey, "ButtonFocusedForeground"); }
        }

        /// <summary>
        /// Button focused border
        /// </summary>
        public static ComponentResourceKey ButtonFocusedBorderBrushKey
        {
            get { return GetRegisteredKey(_ButtonFocusedBorderBrushKey, "ButtonFocusedBorderBrush"); }
        }

        /// <summary>
        /// Button focused background
        /// </summary>
        public static ComponentResourceKey ButtonFocusedBackgroundKey
        {
            get { return GetRegisteredKey(_ButtonFocusedBackgroundKey, "ButtonFocusedBackground"); }
        }
        #endregion

        #region Selected
        /// <summary>
        /// Button selected foreground
        /// </summary>
        public static ComponentResourceKey ButtonSelectedForegroundKey
        {
            get { return GetRegisteredKey(_ButtonSelectedForegroundKey, "ButtonSelectedForeground"); }
        }

        /// <summary>
        /// Button selected border
        /// </summary>
        public static ComponentResourceKey ButtonSelectedBorderBrushKey
        {
            get { return GetRegisteredKey(_ButtonSelectedBorderBrushKey, "ButtonSelectedBorderBrush"); }
        }

        /// <summary>
        /// Button selected background
        /// </summary>
        public static ComponentResourceKey ButtonSelectedBackgroundKey
        {
            get { return GetRegisteredKey(_ButtonSelectedBackgroundKey, "ButtonSelectedBackground"); }
        }
        #endregion

        #region Defaulted
        /// <summary>
        /// Button defaulted foreground
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedForegroundKey
        {
            get { return GetRegisteredKey(_ButtonDefaultedForegroundKey, "ButtonDefaultedForeground"); }
        }

        /// <summary>
        /// Button defaulted border
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedBorderBrushKey
        {
            get { return GetRegisteredKey(_ButtonDefaultedBorderBrushKey, "ButtonDefaultedBorderBrush"); }
        }

        /// <summary>
        /// Button selected background
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedBackgroundKey
        {
            get { return GetRegisteredKey(_ButtonDefaultedBackgroundKey, "ButtonDefaultedBackground"); }
        }
        #endregion

        #region Pressed
        /// <summary>
        /// Button pressed foreground
        /// </summary>
        public static ComponentResourceKey ButtonPressedForegroundKey
        {
            get { return GetRegisteredKey(_ButtonPressedForegroundKey, "ButtonPressedForeground"); }
        }

        /// <summary>
        /// Button pressed border
        /// </summary>
        public static ComponentResourceKey ButtonPressedBorderBrushKey
        {
            get { return GetRegisteredKey(_ButtonPressedBorderBrushKey, "ButtonPressedBorderBrush"); }
        }

        /// <summary>
        /// Button pressed background
        /// </summary>
        public static ComponentResourceKey ButtonPressedBackgroundKey
        {
            get { return GetRegisteredKey(_ButtonPressedBackgroundKey, "ButtonPressedBackground"); }
        }
        #endregion

        #region Disabled
        /// <summary>
        /// Transparent 
        /// </summary>
        public static ComponentResourceKey ButtonTransparentKey
        {
            get { return GetRegisteredKey(_ButtonTransparentKey, "ButtonTransparent"); }
        }

        /// <summary>
        /// Button disabled foreground
        /// </summary>
        public static ComponentResourceKey ButtonDisabledForegroundKey
        {
            get { return GetRegisteredKey(_ButtonDisabledForegroundKey, "ButtonDisabledForeground"); }
        }

        /// <summary>
        /// Button disabled border
        /// </summary>
        public static ComponentResourceKey ButtonDisabledBorderBrushKey
        {
            get { return GetRegisteredKey(_ButtonDisabledBorderBrushKey, "ButtonDisabledBorderBrush"); }
        }

        /// <summary>
        /// Button disabled background
        /// </summary>
        public static ComponentResourceKey ButtonDisabledBackgroundKey
        {
            get { return GetRegisteredKey(_ButtonDisabledBackgroundKey, "ButtonDisabledBackground"); }
        }
        #endregion
        #endregion

        #region Footer
        /// <summary>
        /// Footer foreground
        /// </summary>
        public static ComponentResourceKey FooterForegroundKey
        {
            get { return GetRegisteredKey(_FooterForegroundKey, "FooterForeground"); }
        }

        /// <summary>
        /// Footer border
        /// </summary>
        public static ComponentResourceKey FooterBorderBrushKey
        {
            get { return GetRegisteredKey(_FooterBorderBrushKey, "FooterBorderBrush"); }
        }

        /// <summary>
        /// Footer background
        /// </summary>
        public static ComponentResourceKey FooterBackgroundKey
        {
            get { return GetRegisteredKey(_FooterBackgroundKey, "FooterBackground"); }
        }
        #endregion
        #endregion

        #region Helper
        /// <summary>
        /// Return ResourceKey from resourceId
        /// </summary>
        /// <param name="resKey"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private static ComponentResourceKey GetRegisteredKey(ComponentResourceKey resKey, string resourceId)
        {
            if (resKey == null)
            {
                return new ComponentResourceKey(typeof(CalendarResourceKeys), resourceId);
            }
            else
                return resKey;
        }
        #endregion
    }
}
