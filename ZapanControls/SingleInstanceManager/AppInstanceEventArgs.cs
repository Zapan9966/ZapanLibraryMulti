using System;

namespace ZapanControls.SingleInstanceManager
{
    public class AppInstanceEventArgs : EventArgs
    {
        public string[] CommandLineArgs { get; set; }
    }
}
