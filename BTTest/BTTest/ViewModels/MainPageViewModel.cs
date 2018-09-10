using BTTest.Items;
using BTTest.Models;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;


namespace BTTest.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class MainPageViewModel
    {
        public ObservableCollection<BTDevice> ListDevices { get; set; }
        public BTDevice Device { get; set; }
        public string lbl1 { get; set; }

        public MainPageViewModel()
        {
            lbl1 = "Bluetoothデバイス一覧";
            Initialize();
            IBluetoothManager btMan = DependencyService.Get<IBluetoothManager>();
            btMan.DataReceived += (sender, e) =>
            {
                Console.WriteLine("#debug {0}", e.Data);
            };

            btMan.Start();
        }

        public void Initialize()
        {
            ListDevices = new ObservableCollection<BTDevice>();

            IBluetoothManager btMan = DependencyService.Get<IBluetoothManager>();
            var listBTDevices = btMan.GetBondedDevices();

            if(listBTDevices != null &&  listBTDevices.Count > 0 )
            {
                foreach (var device in listBTDevices)
                {
                    Device = new BTDevice
                    {
                        Name = device.Name,
                        UUID = device.UUID,
                        Address = device.Address
                    };
                    ListDevices.Add(Device);
                }
            }
            else
            {
                lbl1 = "Bluetoothデバイスがありません。";
            }

            //btMan.DataReceived += (sender, e) =>
            // {
            //     Console.WriteLine("#debug {0}", e.Data);
            // };

            //btMan.Start();

        }
    }
}
