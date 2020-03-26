using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;
using CoreAudioApi;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace VUMeter
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private readonly List<Icon> DefaultTrayIcons = new List<Icon>();
        private List<Icon> ActiveTrayIcons = new List<Icon>();
        string[] DevicesIDs = new string[100];

        public Settings()
        {
            InitializeComponent();

            RefreshDevices(AudioSwitch.CoreAudioApi.EDataFlow.eRender);
            //SetTrayIcons();
        }

        private void RefreshDevices(AudioSwitch.CoreAudioApi.EDataFlow renderType)
        {
            DevicesListBox.Items.Clear();
            //DeviceIcons.Clear();

            //listDevices.BeginUpdate();

            AudioSwitch.CoreAudioApi.MMDeviceEnumerator DevEnum = new AudioSwitch.CoreAudioApi.MMDeviceEnumerator();
            AudioSwitch.CoreAudioApi.MMDeviceCollection DevCol = DevEnum.EnumerateAudioEndPoints(renderType, AudioSwitch.CoreAudioApi.EDeviceState.Active);

            if (DevCol.Count > 0)
            {
                var defaultDev = DevEnum.GetDefaultAudioEndpoint(renderType, AudioSwitch.CoreAudioApi.ERole.eMultimedia).ID;
                var devCount = DevCol.Count;

                for (var i = 0; i < devCount; i++)
                {
                    var device = DevCol[i];
                    var devID = device.ID;
                    
                    DevicesIDs[i] = devID;
                    DevicesListBox.Items.Add(device.FriendlyName);
                }
            }
        }


        private void ChangeOutput(string op)
        {
            try
            {
                AudioSwitch.CoreAudioApi.MMDeviceEnumerator DevEnum = new AudioSwitch.CoreAudioApi.MMDeviceEnumerator();
                AudioSwitch.CoreAudioApi.MMDeviceCollection DevCol = DevEnum.EnumerateAudioEndPoints(AudioSwitch.CoreAudioApi.EDataFlow.eRender, AudioSwitch.CoreAudioApi.EDeviceState.Active);
                 
                for(int i = 0; i < DevCol.Count; i++)
                {
                    //DevCol[i].AudioEndpointVolume.
                    if (DevCol[i].FriendlyName == op)
                    { 
                        MessageBox.Show("Found!");
                    }
                        //.SetAsDefault();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DevicesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // set audio device default
            string tmpDev = DevicesListBox.SelectedItem.ToString();
            ChangeOutput(tmpDev);
        }
    }
}
