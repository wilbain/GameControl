using GameControlFramework;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RiftControl
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (var inputSender = new InputSender("RIFT"))
            {
                inputSender.BringWindowToFocus();
                inputSender.SendKeyPress(Key.D1, Key.LeftAlt);
            }

            //var desc1 = new AlertDescriptor("Poison Malice", AlertDescriptorType.Cooldown, new Point(100, 200), Colors.Red);
            //var desc2 = new AlertDescriptor("Thread of Death", AlertDescriptorType.Cooldown, new Point(500, 600), Colors.Purple);
            //AlertDescriptor.SerializeToXml(new List<AlertDescriptor> { desc1, desc2 });

            //List<AlertDescriptor> descriptors = AlertDescriptor.DeserializeFromXml(@"c:\descriptors.xml");
        }
    }
}