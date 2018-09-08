using BTTest.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTTest.Models
{
    public interface IBluetoothManager
    {
        List<BTDevice> GetBondedDevices();
    }
}
;