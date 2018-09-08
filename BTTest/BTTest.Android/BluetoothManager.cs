using Android.Bluetooth;
using BTTest.Items;
using BTTest.Models;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

[assembly: Dependency(typeof(BTTest.Droid.BluetoothManager))]

namespace BTTest.Droid
{
    public class BluetoothManager:IBluetoothManager
    {
        private List<BTDevice> result = new List<BTDevice>();
        public List<BTDevice> GetBondedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter; //アダプター作成
            var listBondedDevices = adapter.BondedDevices;              //ペアリング済みデバイスの取得

            if(listBondedDevices != null && listBondedDevices.Count>0)
            {
                foreach (var device in listBondedDevices)
                {
                    var btDevice = new BTDevice();
                    btDevice.Name = device.Name;
                    btDevice.UUID = device.GetUuids().FirstOrDefault().ToString();
                    btDevice.Address = device.Address;
                    result.Add(btDevice);
                }
            }
            return result;
        }

    }
}