using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CoreAudioApi;

namespace VUMeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int WM_NCHITTEST = 0x84;
        const int HT_CAPTION = 0x2;

        private List<float> left = new List<float>();
        private List<float> right = new List<float>();

        private MMDevice device;

        double LeftChannelValue = 0;
        double RightChannelValue = 0;

        double lblMax = 0;
        public MainWindow()
        {
            InitializeComponent();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            double value = device.AudioMeterInformation.PeakValues[0] * 100;
            if (left.Count == 10)
                left.RemoveAt(0);
            left.Add((float)value);
            double leftaverage = left.Average();

            value = device.AudioMeterInformation.PeakValues[1] * 100;
            if (right.Count == 10)
                right.RemoveAt(0);
            right.Add((float)value);
            double rightaverage = right.Average();

            if (leftaverage > rightaverage)
            {
                if (leftaverage > lblMax)
                {
                    float interval;
                    float max;
                    interval = (float)Math.Ceiling(leftaverage / 4);
                    max = interval * 4;
                    lblMax = max;
                    LeftChannelVU.MoveMaxNeddle((lblMax));
                }
            }
            else
            {
                if (rightaverage > lblMax)
                {
                    float interval;
                    float max;
                    interval = (float)Math.Ceiling(rightaverage / 4);
                    max = interval * 4;
                    lblMax = max;
                    RightChannelVU.MoveMaxNeddle((lblMax));
                }
            }

            LeftChannelValue = (float)(leftaverage / (lblMax / 100));
            RightChannelValue = (float)(rightaverage / (lblMax / 100));

            LeftChannelVU.MoveVolumeNeddle(LeftChannelValue);
            RightChannelVU.MoveVolumeNeddle(RightChannelValue);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LeftChannelValue = 0;
            RightChannelValue = 0;

            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            this.Title = device.FriendlyName.ToString();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(15);
            timer.Tick += timer_Tick;
            timer.Start();

            LeftChannelVU.SetChannelLabel("Left Channel");
            RightChannelVU.SetChannelLabel("Right Channel");

            LeftChannelVU.SetDeviceLabel(device.FriendlyName.ToString());
            RightChannelVU.SetDeviceLabel(device.FriendlyName.ToString());
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                RefreshDefaultAudioEndPoint();
            }
        }

        private void ExitImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RefreshDefaultAudioEndPoint()
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            this.Title = device.FriendlyName.ToString();
            LeftChannelVU.SetDeviceLabel(device.FriendlyName.ToString());
            RightChannelVU.SetDeviceLabel(device.FriendlyName.ToString());
        }
        /*
        this.Location = Properties.Settings.Default.location;
        this.TopMost = Properties.Settings.Default.TopMost;
        if (Properties.Settings.Default.variablevalue == true)
        {
            allowVariableMaxValueToolStripMenuItem.Checked = true;
            int zero = 1;
            lblMax.Text = zero.ToString();
            resetMaxValueToolStripMenuItem.Enabled = true;
        }
        else
        {
            allowVariableMaxValueToolStripMenuItem.Checked = false;
            int zero = 100;
            lblMax.Text = zero.ToString();
            resetMaxValueToolStripMenuItem.Enabled = false;
        }
        */
    }
}
