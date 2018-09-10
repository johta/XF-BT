using Android.Bluetooth;
using BTTest.Items;
using BTTest.Models;
using Java.IO;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

[assembly: Dependency(typeof(BTTest.Droid.BluetoothManager))]

namespace BTTest.Droid
{
    public class BluetoothManager : IBluetoothManager
    {
        public event EventHandler<DataEventArgs> DataReceived;
        private const string Uuid = "00001124-0000-1000-8000-00805f9b34fb";
        //private BluetoothServerSocket mServerSocket;
        private BluetoothSocket mSocket;
        private BluetoothAdapter mAdapter;
        private BufferedReader reader;
        private System.IO.Stream mStream;
        private InputStreamReader mReader;

        public string getDataFromDevice()
        {
            return reader.ReadLine();
        }

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

        private void openDeviceConnection(BluetoothDevice btDevice)
        {
            try
            {
                mSocket = btDevice.CreateRfcommSocketToServiceRecord(getUUIDFromString());
                System.Console.WriteLine("#debug get socket [{0}]", btDevice.Name);
                //ブロック処理を書く
                mSocket.Connect();
                System.Console.WriteLine("#debug connect[{0}]", btDevice.Name);
                //input stream
                mStream = mSocket.InputStream;
                System.Console.WriteLine("#debug open InputStream [{0}]", btDevice.Name);
                //output stream
                //msocket.OutputStream;
                mReader = new InputStreamReader(mStream);
                System.Console.WriteLine("#debug get StreamReader [{0}]", btDevice.Name);
                reader = new BufferedReader(mReader);
                System.Console.WriteLine("#debug get get BufferedReader [{0}]", btDevice.Name);
            }
            catch(IOException e)
            {
                close(mSocket);
                close(mStream);
                close(mReader);
                e.PrintStackTrace();
                throw e;
                
            }
        }
        public void getAllPairedDevices()
        {
            System.Console.WriteLine("#debug getAllPairedDevice()");
            BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
            var devices = btAdapter.BondedDevices;
            if (devices != null && devices.Count > 0)
            {
                
                foreach (var mDevice in devices)
                {
                    openDeviceConnection(mDevice);
                }
            }
        }

        public void Start()
        {
            System.Console.WriteLine("#debug location:Start()");
            getAllPairedDevices();

            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    var data = getDataFromDevice();
                    DataReceived?.Invoke(this, new DataEventArgs(data));
                }
            });
            thread.IsBackground = true;
            thread.Start();
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