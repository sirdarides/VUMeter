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

namespace VUMeter
{
    /// <summary>
    /// Interaction logic for VU_Flat.xaml
    /// </summary>
    public partial class VU_Flat : UserControl
    {
        double VolumeMaxValue = 0;

        public VU_Flat()
        {
            InitializeComponent();
        }

        public void MoveVolumeNeddle(double Volume)
        {
            double RotationAngle = -49 + ((Volume * 74) / 100);
            //RotationAngle = -49;

            RotateTransform rotateTransform1 = new RotateTransform(RotationAngle);
            VolumeNiddle.RenderTransform = rotateTransform1;

            if (Volume >= VolumeMaxValue)
            {
                VolumeMaxValue = Volume;
            }
            if (Volume == 0)
            {
                VolumeMaxValue = 0;
                /*
                for (double tmpVol = Volume; tmpVol < 1; tmpVol = tmpVol - 5)
                {
                    VolumeMaxValue = tmpVol;
                    MoveMaxNeddle(VolumeMaxValue);
                }
                */
            }
            MoveMaxNeddle(VolumeMaxValue);        
        }

        public void MoveMaxNeddle(double Volume)
        {
            double RotationAngle = -49 + ((Volume * 74) / 100);
            //RotationAngle = -49;

            RotateTransform rotateTransform1 = new RotateTransform(RotationAngle);
            MaxNiddle.RenderTransform = rotateTransform1;
        }

        public void SetChannelLabel(string ChannelName)
        {
            ChannelLabel.Text = ChannelName;
        }

        public void SetDeviceLabel(string DevName)
        {
            DeviceName.Text = DevName;
        }
    }
}
