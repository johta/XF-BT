using BTTest.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTTest.Models
{
    public interface IBluetoothManager
    {
        event EventHandler<DataEventArgs> DataReceived;
        void getAllPairedDevices();

        List<BTDevice> GetBondedDevices();
        void Start();

    }
}
;