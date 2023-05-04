using System;
using System.Windows.Forms;

namespace SokobanApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
            wplayer.URL = "baraban.mp3";
            wplayer.controls.play();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SokobanForm());
        }
    }
}
