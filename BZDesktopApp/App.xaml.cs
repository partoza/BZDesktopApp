namespace BZDesktopApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new MainPage()) { Title = "BZDesktopApp" };

#if WINDOWS
            bool applied = false;

            void TryMaximize()
            {
                if (applied) return;
                try
                {
                    var nativeWindow = (window.Handler?.PlatformView as Microsoft.UI.Xaml.Window);
                    if (nativeWindow is not null)
                    {
                        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                        if (appWindow is not null)
                        {
                            // Use Overlapped presenter and maximize to keep system caption buttons (min/max/close)
                            if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter overlapped)
                            {
                                overlapped.IsResizable = true;
                                overlapped.IsMaximizable = true;
                                overlapped.SetBorderAndTitleBar(true, true);
                                overlapped.Maximize();
                            }
                            else
                            {
                                appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);
                                if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter overlapped2)
                                {
                                    overlapped2.IsResizable = true;
                                    overlapped2.IsMaximizable = true;
                                    overlapped2.SetBorderAndTitleBar(true, true);
                                    overlapped2.Maximize();
                                }
                            }
                            applied = true;
                        }
                    }
                }
                catch
                {
                }
            }

            window.HandlerChanged += (s, e) => TryMaximize();
            window.Activated += (s, e) => TryMaximize();
#endif

            return window;
        }
    }
}
