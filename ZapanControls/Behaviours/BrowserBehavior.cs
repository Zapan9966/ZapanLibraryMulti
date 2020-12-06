using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ZapanControls.Behaviours
{
    public sealed class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof(string), typeof(BrowserBehavior), new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d?.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d?.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is WebBrowser webBrowser)
            {
                Stream stream = new MemoryStream(Encoding.Default.GetBytes(e.NewValue as string ?? "&nbsp;"));
                webBrowser.NavigateToStream(stream);
            }
        }
    }
}
