using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Web;

namespace ServiceMonitor
{
    public class MachineInfoCollector
    {
        static PerformanceCounter _cpuCounter;
        static float TotalPhysicalMemory = 0;
        public static void Collect(ref MachineInfo info)
        {
            if (info == null)
            {
                info = new MachineInfo();
            }
            //1、cpu占用率
            if (_cpuCounter == null)
            {
                _cpuCounter = new PerformanceCounter();
                _cpuCounter.CategoryName = "Processor";
                _cpuCounter.CounterName = "% Processor Time";
                _cpuCounter.InstanceName = "_Total";
            }

            /*
             * 寄宿到iis下时，会报权限不足错误，参考 http://www.java123.net/998683.html
             */
            info.CpuUsage = _cpuCounter.NextValue();

            //2、内存占用率
            //①物理内存
            if (TotalPhysicalMemory == 0)
            {
                //获得物理内存 
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if (mo["TotalPhysicalMemory"] != null)
                    {
                        TotalPhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                    }
                }
            }
            //②可用内存
            float availablebytes = 0;
            ManagementClass mos = new ManagementClass("Win32_OperatingSystem");
            foreach (ManagementObject mo in mos.GetInstances())
            {
                if (mo["FreePhysicalMemory"] != null)
                {
                    availablebytes = 1024 * float.Parse(mo["FreePhysicalMemory"].ToString());
                }
            }
            info.MemoryUsage = (1 - availablebytes / TotalPhysicalMemory) * 100;

            info.Time = DateTime.Now;
        }
    }
}