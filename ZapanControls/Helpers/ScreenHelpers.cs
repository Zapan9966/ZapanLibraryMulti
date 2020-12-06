using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace ZapanControls.Helpers
{
    public static class ScreenHelpers
    {
        public static Screen GetCurrentScreen(Window window)
        {
            var parentArea = new Rectangle((int)window?.Left, (int)window?.Top, (int)window?.Width, (int)window?.Height);
            return Screen.FromRectangle(parentArea);
        }

        public static Screen GetScreen(int requestedScreen)
        {
            var screens = Screen.AllScreens;
            var mainScreen = 0;
            if (screens.Length > 1 && mainScreen < screens.Length)
            {
                return screens[requestedScreen];
            }
            return screens[0];
        }
    }
}
