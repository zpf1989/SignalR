using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceMonitor
{
    public class MachineInfo
    {
        public float CpuUsage { get; set; }
        public float MemoryUsage { get; set; }

        public DateTime Time { get; set; }
    }
}