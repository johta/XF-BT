using BTTest.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTTest.Models
{
    public interface IBluetoothManager
    {
        event EventHandler<DataEventArgs> DataReceived;
        List<BTDevice> GetBondedDevices();
        void Start();

    }
}
;