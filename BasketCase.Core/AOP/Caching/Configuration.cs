using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Caching
{
    public class Configuration
    {
        public string Endpoint { get; set; }
        public int Port { get; set; }
        public string Key { get; set; }
        public bool AbortOnConnectFail { get; set; } = true;
        public int ConnectTimeout { get; set; } = 100000;
        public int SyncTimeout { get; set; } = 100000;
    }
}
