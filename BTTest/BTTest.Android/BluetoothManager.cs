using Android.Bluetooth;
using BTTest.Items;
using BTTest.Models;
using Java.IO;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(BTTest.Droid.BluetoothManager))]

namespace BTTest.Droid
{
    public class BluetoothManager : IBluetoothManager
    {
        public event EventHandler<DataEventArgs> DataReceived;

        private const string Uuid = "00001101-0000-1000-8000-00805f9b34fb";
        
        private UUID getUUIDFromString()
        {
            return UUID.FromString(Uuid);
        }

        private void close(IDisposable aConnectedObject)
        {
            try
            {
                if (aConnectedObject == null)
                {
                    return;
                }
            }catch(Exception e)
            {
                throw;
            }
            aConnectedObject = null;
        }

        public void Start()
        {
            var btAdapter = BluetoothAdapter.DefaultAdapter;
            var devices = btAdapter.BondedDevices;
            if (devices != null && devices.Count > 0)
            {
                foreach (var device in devices)
                {
                    try
                    {
                        var socket = device.CreateRfcommSocketToServiceRecord(getUUIDFromString());
                        Task.Run(() =>
                        {
                            try
                            {
                                socket.Connect();
                                System.Console.WriteLine("#debug connect to {0}", device.Name);
                                var br = new BufferedReader(new InputStreamReader(socket.InputStream));
                                string data;
                                while ((data = br.ReadLine()) != null)
                                {
                                    DataReceived?.Invoke(this, new DataEventArgs(data));
                                }
                            }
                            catch (IOException e)
                            {
                            }
                            finally
                            {
                                socket.Close();
                                System.Console.WriteLine("#debug close socket  {0}", device.Name);
                            }
                        });
                    }
                    catch (IOException e)
                    {
                    }
                }
            }
        }

        private List<BTDevice> result = new List<BTDevice>();
        public List<BTDevice> GetBondedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter; //アダプター作成
            var listBondedDevices = adapter.BondedDevices;              //ペアリング済みデバイスの取得

            if (listBondedDevices != null && listBondedDevices.Count > 0)
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