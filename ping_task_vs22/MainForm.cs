using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace ping_task_vs22
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!(DesignMode || _isHandleInitialized))
            {
                _isHandleInitialized = true;
                execPing();

            }
        }
        bool _isHandleInitialized = false;

        // https://docs.microsoft.com/en-us/dotnet/api/system.net.networkinformation.ping?view=net-6.0
        void execPing()
        {
            Task.Run(() =>
            {
                while (!DisposePing.IsCancellationRequested)
                {
                    var pingSender = new Ping();
                    var pingOptions = new PingOptions
                    {
                        DontFragment = true,
                    };
                // https://docs.microsoft.com/en-us/dotnet/api/system.net.networkinformation.ping?view=net-6.0#examples
                // Create a buffer of 32 bytes of data to be transmitted.
                    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    int timeout = 120;
                    try
                    {
                    // https://stackoverflow.com/a/25654227/5438626
                        if (Uri.TryCreate(textBoxUri.Text, UriKind.Absolute, out Uri? uri)
                            && (uri.Scheme == Uri.UriSchemeHttp ||
                            uri.Scheme == Uri.UriSchemeHttps))
                        {
                            PingReply reply = pingSender.Send(
                                uri.Host,
                                timeout, buffer,
                                pingOptions);
                            switch (reply.Status)
                            {
                                case IPStatus.Success:
                                    Invoke(() => onPingSuccess());
                                    break;
                                default:
                                    Invoke(() => onPingFailed(reply.Status));
                                    break;
                            }
                        }
                        else
                        {
                            Invoke(() => labelStatus.Text =
                                $"{DateTime.Now}: Invalid URI: try 'http://");
                        }
                    }
                    catch (Exception ex)
                    {
                    // https://stackoverflow.com/a/60827505/5438626
                        if (ex.InnerException == null)
                        {
                            Invoke(() => labelStatus.Text = ex.Message);
                        }
                        else
                        {
                            Invoke(() => labelStatus.Text = ex.InnerException.Message);
                        }
                    }
                // Since the timeout is so large, it wouldn't make sense for it to be on 
                // a 1-second timer. What we DO want to do is wait for the Ping to complete
                // synchronously and then wait a second brfore starting the next one.
                    Task.Delay(1000).Wait();
                }
            });
        }

    private void onPingSuccess()
    {
        labelStatus.Text = $"{DateTime.Now}: {IPStatus.Success}";
        // Up to you what you do here
    }

    private void onPingFailed(IPStatus status)
    {
        labelStatus.Text = $"{DateTime.Now}: {status}";
        // Up to you what you do here
    }

        public CancellationTokenSource DisposePing { get; } = new CancellationTokenSource();
    }
}