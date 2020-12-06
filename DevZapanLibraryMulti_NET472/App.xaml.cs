using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZapanControls.SingleInstanceManager;

namespace DevZapanLibraryMulti_NET472
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var manager = new AppInstanceManager(Environment.UserName);
            manager.OnReceivedArgs += SingleInstanceCallback;

            base.OnStartup(e);
        }

        /// <summary>
        /// Single instance callback handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SingleInstanceApplication.InstanceCallbackEventArgs"/> instance containing the event data.</param>
        private void SingleInstanceCallback(object sender, AppInstanceEventArgs args)
        {
            if (args == null || Dispatcher == null) return;
            Action<bool> d = (bool x) =>
            {

                //win.Activate(x);
                MessageBox.Show(string.Join("\r\n", args.CommandLineArgs), "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            };
            Dispatcher.Invoke(d, true);
        }
    }
}
