using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace ZapanControls.SingleInstanceManager
{
    public class AppInstanceManager
    {
        private readonly string _pipeName;
        private readonly int _bufferSize = 2048;

        private bool _appIsRunning = true;
        private NamedPipeServerStream _pipeServer;

        public bool IsFirstInstance { get; private set; }

        public event EventHandler<AppInstanceEventArgs> OnReceivedArgs; 

        public AppInstanceManager(string username = "", bool closeIfNotFirst = true, int exitCode = 0)
        {
            _pipeName = $"{Environment.MachineName}" +
                $"{(!string.IsNullOrEmpty(username) ? $"-{username}" : null)}" +
                $"-{Process.GetCurrentProcess().ProcessName}" +
                $"-{Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductVersion}";

            // Try to send args to named pipe server
            TrySendArgs();

            if (IsFirstInstance)
            {
                // Start named pipe server
                Task.Run(StartServer);

                // Close server on process exit
                Process process = Process.GetCurrentProcess();
                process.Exited += delegate
                {
                    _appIsRunning = false;
                    _pipeServer?.Disconnect();
                    _pipeServer?.Dispose();
                };
            }
            else if (closeIfNotFirst)
            {
                Environment.Exit(exitCode);
            }
        }

        /// <summary>
        /// Verify if it's the first instance.
        /// </summary>
        private void TrySendArgs()
        {
            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", _pipeName, PipeDirection.Out))
                {
                    pipeClient.Connect(500);
                    if (pipeClient.IsConnected)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(string.Join(" ", Environment.GetCommandLineArgs()));

                        pipeClient.Write(buffer, 0, buffer.Length);
                        pipeClient.WaitForPipeDrain();
                        IsFirstInstance = false;
                    }
                    else
                    {
                        IsFirstInstance = true;
                    }
                }
            }
            catch
            {
                IsFirstInstance = true;
            }
        }

        /// <summary>
        /// Registers the named pipe server.
        /// </summary>
        private void StartServer()
        {
            while (_appIsRunning)
            {
                using (_pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.In, 1, PipeTransmissionMode.Message))
                {
                    _pipeServer.WaitForConnection();

                    StringBuilder builder = new StringBuilder();
                    byte[] buffer = new byte[_bufferSize];

                    do
                    {
                        _pipeServer.Read(buffer, 0, buffer.Length);
                        builder.Append(Encoding.UTF8.GetString(buffer));
                        buffer = new byte[buffer.Length];
                    }
                    while (!_pipeServer.IsMessageComplete);

                    string[] args = builder.ToString().TrimEnd('\0').Split(' ');
                    OnReceivedArgs?.Invoke(this, new AppInstanceEventArgs { CommandLineArgs = args });
                }
            }
        }
    }

}
