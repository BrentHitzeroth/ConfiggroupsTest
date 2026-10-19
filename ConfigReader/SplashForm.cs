using System.Drawing;
using System.Windows.Forms;

namespace ConfigReader
{
    public class SplashForm : Form
    {
        public SplashForm(Image waitImage = null)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            ShowInTaskbar = false;
            ClientSize = new Size(320, 160);
            BackColor = Color.White;

            if (waitImage != null)
            {
                Controls.Add(new PictureBox
                {
                    Dock = DockStyle.Fill,
                    Image = waitImage,
                    SizeMode = PictureBoxSizeMode.CenterImage
                });
            }
            else
            {
                Controls.Add(new ProgressBar
                {
                    Style = ProgressBarStyle.Marquee,
                    Dock = DockStyle.Bottom,
                    MarqueeAnimationSpeed = 30
                });
                Controls.Add(new Label
                {
                    Text = "Loading...",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 14f)
                });
            }
        }
    }
}
