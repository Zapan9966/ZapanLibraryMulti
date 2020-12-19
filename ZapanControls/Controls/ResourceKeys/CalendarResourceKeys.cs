#region Directives
using System.Windows;
#endregion

namespace ZapanControls.Controls.ResourceKeys
{
    public static class CalendarResourceKeys
    {
        #region Fields
        // Header
        private static readonly ComponentResourceKey _HeaderNormalForegroundBrushKey = null;
        private static readonly ComponentResourceKey _HeaderFocusedForegroundBrushKey = null;
        private static readonly ComponentResourceKey _HeaderPressedForegroundBrushKey = null;

        private static readonly ComponentResourceKey _HeaderNormalBorderBrushKey = null;
        private static readonly ComponentResourceKey _HeaderFocusedBorderBrushKey = null;
        private static readonly ComponentResourceKey _HeaderPressedBorderBrushKey = null;

        private static readonly ComponentResourceKey _HeaderNormalBackgroundBrushKey = null;
        private static readonly ComponentResourceKey _HeaderFocusedBackgroundBrushKey = null;
        private static readonly ComponentResourceKey _HeaderPressedBackgroundBrushKey = null;

        // Direction buttons
        private static readonly ComponentResourceKey _ArrowBorderBrushKey = null;
        private static readonly ComponentResourceKey _ArrowNormalFillBrushKey = null;
        private static readonly ComponentResourceKey _ArrowFocusedFillBrushKey = null;
        private static readonly ComponentResourceKey _ArrowPressedFillBrushKey = null;

        // Date buttons
        // normal
        private static readonly ComponentResourceKey _ButtonNormalForegroundBrushKey = null;
        private static readonly ComponentResourceKey _ButtonNormalBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonNormalBackgroundBrushKey = null;
        // focused
        private static readonly ComponentResourceKey _ButtonFocusedForegroundBrushKey = null;
        private static readonly ComponentResourceKey _ButtonFocusedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonFocusedBackgroundBrushKey = null;
        // selected 
        private static readonly ComponentResourceKey _ButtonSelectedForegroundBrushKey = null;
        private static readonly ComponentResourceKey _ButtonSelectedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonSelectedBackgroundBrushKey = null;
        // pressed
        private static readonly ComponentResourceKey _ButtonPressedForegroundBrushKey = null;
        private static readonly ComponentResourceKey _ButtonPressedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonPressedBackgroundBrushKey = null;
        // defaulted
        private static readonly ComponentResourceKey _ButtonDefaultedForegroundBrushKey = null;
        private static readonly ComponentResourceKey _ButtonDefaultedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonDefaultedBackgroundBrushKey = null;
        // disabled
        private static readonly ComponentResourceKey _ButtonTransparentBrushKey = null;
        private static readonly ComponentResourceKey _ButtonDisabledForegroundBrushKey = null;
        private static readonly ComponentResourceKey _ButtonDisabledBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonDisabledBackgroundBrushKey = null;

        // Week column
        private static readonly ComponentResourceKey _WeekColumnForegroundBrushKey = null;
        private static readonly ComponentResourceKey _WeekColumnBackgroundBrushKey = null;
        private static readonly ComponentResourceKey _WeekColumnBorderBrushKey = null;

        // Footer
        private static readonly ComponentResourceKey _FooterForegroundBrushKey = null;
        private static readonly ComponentResourceKey _FooterBorderBrushKey = null;
        private static readonly ComponentResourceKey _FooterBackgroundBrushKey = null;

        // Day column
        private static readonly ComponentResourceKey _DayNamesForegroundBrushKey = null;
        private static readonly ComponentResourceKey _DayNamesBorderBrushKey = null;
        private static readonly ComponentResourceKey _DayNamesBackgroundBrushKey = null;

        // Control
        private static readonly ComponentResourceKey _ControlBorderBrushKey = null;
        private static readonly ComponentResourceKey _ControlBackgroundBrushKey = null;
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
        public static ComponentResourceKey ControlBackgroundBrushKey
        {
            get { return GetRegisteredKey(_ControlBackgroundBrushKey, "ControlBackgroundBrush"); }
        }
        #endregion

        #region Header
        /// <summary>
        /// Header normal foreground
        /// </summary>
        public static ComponentResourceKey HeaderNormalForegroundBrushKey
        {
            get { return GetRegisteredKey(_HeaderNormalForegroundBrushKey, "HeaderNormalForegroundBrush"); }
        }

        /// <summary>
        /// Header focused foreground
        /// </summary>
        public static ComponentResourceKey HeaderFocusedForegroundBrushKey
        {
            get { return GetRegisteredKey(_HeaderFocusedForegroundBrushKey, "HeaderFocusedForegroundBrush"); }
        }

        /// <summary>
        /// Header pressed foreground
        /// </summary>
        public static ComponentResourceKey HeaderPressedForegroundBrushKey
        {
            get { return GetRegisteredKey(_HeaderPressedForegroundBrushKey, "HeaderPressedForegroundBrush"); }
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
        public static ComponentResourceKey HeaderNormalBackgroundBrushKey
        {
            get { return GetRegisteredKey(_HeaderNormalBackgroundBrushKey, "HeaderNormalBackgroundBrush"); }
        }

        /// <summary>
        /// Header focused background brush
        /// </summary>
        public static ComponentResourceKey HeaderFocusedBackgroundBrushKey
        {
            get { return GetRegisteredKey(_HeaderFocusedBackgroundBrushKey, "HeaderFocusedBackgroundBrush"); }
        }

        /// <summary>
        /// Header pressed background brush
        /// </summary>
        public static ComponentResourceKey HeaderPressedBackgroundBrushKey
        {
            get { return GetRegisteredKey(_HeaderPressedBackgroundBrushKey, "HeaderPressedBackgroundBrush"); }
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
        public static ComponentResourceKey ArrowNormalFillBrushKey
        {
            get { return GetRegisteredKey(_ArrowNormalFillBrushKey, "ArrowNormalFillBrush"); }
        }

        /// <summary>
        /// Direction arrow focused fill
        /// </summary>
        public static ComponentResourceKey ArrowFocusedFillBrushKey
        {
            get { return GetRegisteredKey(_ArrowFocusedFillBrushKey, "ArrowFocusedFillBrush"); }
        }

        /// <summary>
        /// Direction arrow pressed fill
        /// </summary>
        public static ComponentResourceKey ArrowPressedFillBrushKey
        {
            get { return GetRegisteredKey(_ArrowPressedFillBrushKey, "ArrowPressedFillBrush"); }
        }
        #endregion

        #region Day Column
        /// <summary>
        /// Day names foreground
        /// </summary>
        public static ComponentResourceKey DayNamesForegroundBrushKey
        {
            get { return GetRegisteredKey(_DayNamesForegroundBrushKey, "DayNamesForegroundBrush"); }
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
        public static ComponentResourceKey DayNamesBackgroundBrushKey
        {
            get { return GetRegisteredKey(_DayNamesBackgroundBrushKey, "DayNamesBackgroundBrush"); }
        }
        #endregion

        #region Week Column
        /// <summary>
        /// Week column foreground
        /// </summary>
        public static ComponentResourceKey WeekColumnForegroundBrushKey
        {
            get { return GetRegisteredKey(_WeekColumnForegroundBrushKey, "WeekColumnForegroundBrush"); }
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
        public static ComponentResourceKey WeekColumnBackgroundBrushKey
        {
            get { return GetRegisteredKey(_WeekColumnBackgroundBrushKey, "WeekColumnBackgroundBrush"); }
        }
        #endregion

        #region Button
        #region Normal
        /// <summary>
        /// Button normal foreground
        /// </summary>
        public static ComponentResourceKey ButtonNormalForegroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonNormalForegroundBrushKey, "ButtonNormalForegroundBrush"); }
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
        public static ComponentResourceKey ButtonNormalBackgroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonNormalBackgroundBrushKey, "ButtonNormalBackgroundBrush"); }
        }
        #endregion

        #region Focused
        /// <summary>
        /// Button focused foreground
        /// </summary>
        public static ComponentResourceKey ButtonFocusedForegroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonFocusedForegroundBrushKey, "ButtonFocusedForegroundBrush"); }
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
        public static ComponentResourceKey ButtonFocusedBackgroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonFocusedBackgroundBrushKey, "ButtonFocusedBackgroundBrush"); }
        }
        #endregion

        #region Selected
        /// <summary>
        /// Button selected foreground
        /// </summary>
        public static ComponentResourceKey ButtonSelectedForegroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonSelectedForegroundBrushKey, "ButtonSelectedForegroundBrush"); }
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
        public static ComponentResourceKey ButtonSelectedBackgroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonSelectedBackgroundBrushKey, "ButtonSelectedBackgroundBrush"); }
        }
        #endregion

        #region Defaulted
        /// <summary>
        /// Button defaulted foreground
        /// </summary>
        public static ComponentResourceKey ButtonDefaultedForegroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonDefaultedForegroundBrushKey, "ButtonDefaultedForegroundBrush"); }
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
        public static ComponentResourceKey ButtonDefaultedBackgroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonDefaultedBackgroundBrushKey, "ButtonDefaultedBackgroundBrush"); }
        }
        #endregion

        #region Pressed
        /// <summary>
        /// Button pressed foreground
        /// </summary>
        public static ComponentResourceKey ButtonPressedForegroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonPressedForegroundBrushKey, "ButtonPressedForegroundBrush"); }
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
        public static ComponentResourceKey ButtonPressedBackgroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonPressedBackgroundBrushKey, "ButtonPressedBackgroundBrush"); }
        }
        #endregion

        #region Disabled
        /// <summary>
        /// Transparent brush
        /// </summary>
        public static ComponentResourceKey ButtonTransparentBrushKey
        {
            get { return GetRegisteredKey(_ButtonTransparentBrushKey, "ButtonTransparentBrush"); }
        }

        /// <summary>
        /// Button disabled foreground
        /// </summary>
        public static ComponentResourceKey ButtonDisabledForegroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonDisabledForegroundBrushKey, "ButtonDisabledForegroundBrush"); }
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
        public static ComponentResourceKey ButtonDisabledBackgroundBrushKey
        {
            get { return GetRegisteredKey(_ButtonDisabledBackgroundBrushKey, "ButtonDisabledBackgroundBrush"); }
        }
        #endregion
        #endregion

        #region Footer
        /// <summary>
        /// Footer foreground
        /// </summary>
        public static ComponentResourceKey FooterForegroundBrushKey
        {
            get { return GetRegisteredKey(_FooterForegroundBrushKey, "FooterForegroundBrush"); }
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
        public static ComponentResourceKey FooterBackgroundBrushKey
        {
            get { return GetRegisteredKey(_FooterBackgroundBrushKey, "FooterBackgroundBrush"); }
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
