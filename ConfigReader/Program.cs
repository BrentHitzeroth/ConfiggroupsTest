using System;
using System.Threading;
using System.Windows.Forms;

namespace ConfigReader
{
    static class Program
    {
        private static SplashForm splash;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the splash on its own STA thread so it animates while the main thread is busy.
            var splashReady = new ManualResetEventSlim(false);
            var splashThread = new Thread(() =>
            {
                splash = new SplashForm();
                splash.Shown += (s, e) => splashReady.Set();
                // Guarantee the main thread is never left waiting if the splash fails to show.
                splash.Disposed += (s, e) => splashReady.Set();
                Application.Run(splash);
            });
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.IsBackground = true;
            splashThread.Start();
            splashReady.Wait();

            MainForm mainForm = null;
            try
            {
                mainForm = new MainForm(); // heavy initialization goes here
                mainForm.Shown += (s, e) =>
                {
                    CloseSplash();
                    mainForm.Activate();
                };
            }
            catch
            {
                CloseSplash();
                throw;
            }

            try
            {
                Application.Run(mainForm);
            }
            finally
            {
                CloseSplash();
            }
        }

        private static void CloseSplash()
        {
            var s = splash;
            if (s == null || s.IsDisposed) return;
            try
            {
                if (s.IsHandleCreated) s.Invoke(new Action(s.Close));
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }
    }
}
