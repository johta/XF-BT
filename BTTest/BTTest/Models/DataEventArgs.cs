using System;
using System.Collections.Generic;
using System.Text;

namespace BTTest.Models
{
    public class DataEventArgs : EventArgs
    {
        public string Data { get; }

        public DataEventArgs(string data)
        {
            Data = data;
        }
    }
}
