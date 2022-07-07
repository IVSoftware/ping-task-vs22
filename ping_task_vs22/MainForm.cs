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
            textBoxUri.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Return) e.SuppressKeyPress = true;
            };
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!(DesignMode || _isHandleInitialized))
            {
                _isHandleInitialized = true;
                execPing();
                execUpdateUI();
            }
        }

        bool _isHandleInitialized = false;

        // https://docs.microsoft.com/en-us/dotnet/api/system.net.networkinformation.ping?view=net-6.0
        void execPing()
        {
            Task.Run(async() =>
            {   //   ^^^^^
                // The thing is, this loop is already running in a background task so
                // we're not really holding anything up. There may be little-to-no value
                // in using await and making async calls inside the loop. But if you really
                // must do that for some reason, adding the 'async' lets you do that.
                // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

                while (!DisposePing.IsCancellationRequested)
                {
                    // But what if there were a concurrent task? To avoid
                    // deadlock we may want to await the signalled state async.
                    await _semaphore.WaitAsync();

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
                            PingReply reply = await pingSender.SendPingAsync(
                                uri.Host,
                                timeout,
                                buffer,
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
                    finally
                    {
                        _semaphore.Release();
                    }
                    // Since the timeout is so large, it wouldn't make sense for it to be on 
                    // a 1-second timer. What we DO want to do is wait for the Ping to complete
                    // synchronously and then wait a second before starting the next one.
                    await Task.Delay(1000);
                }
            });
        }
        SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

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

        /// <summary>
        /// Just for fun
        /// </summary>
        private void execUpdateUI()
        {
            Task.Run(async () =>
            {   //   ^^^^^
                // The thing is, this loop is already running in a background task so
                // we're not really holding anything up. There may be little-to-no value
                // in using await and making async calls inside the loop. But if you really
                // must do that for some reason, adding the 'async' lets you do that.
                // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

                while (!DisposePing.IsCancellationRequested)
                {
                    // BUT what if there were a concurrent task? To avoid
                    // deadlock we may want to await the signalled state async.
                    await _semaphore.WaitAsync();
                    try 
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            Invoke(() => 
                            {
                               labelCountNothingForNoReason.Text = $"Count: {i}";
                            });
                            await Task.Delay(1);
                        }
                    }
                    catch (Exception)
                    {
                        Debug.Assert(false, "This method is not expected to ever fail!");
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                    // Since the timeout is so large, it wouldn't make sense for it to be on 
                    // a 1-second timer. What we DO want to do is wait for the Ping to complete
                    // synchronously and then wait a second before starting the next one.
                    await Task.Delay(5000);
                }
            });
        }
    }
}