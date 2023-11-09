using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WmiIntrinsicEvents
{
    internal class WmiW
    {
        public static void Subscribe()
        {
            try
            {
                string wmiQuery = @"SELECT * FROM __InstanceCreationEvent WITHIN 5 "
                    + "WHERE TargetInstance ISA \"Win32_Process\" "
                    + "AND TargetInstance.Name = \"notepad.exe\"";
                ManagementEventWatcher watcher = new(wmiQuery);
                Console.WriteLine("Waiting for an event...");

                ManagementBaseObject @event = watcher.WaitForNextEvent();

                Console.WriteLine("Process {0} has been created, path is: {1}",
                            ((ManagementBaseObject)@event
                            ["TargetInstance"])["Name"],
                            ((ManagementBaseObject)@event
                            ["TargetInstance"])["ExecutablePath"]);

                watcher.Stop();
                return;
            }
            catch (ManagementException err)
            {
                Console.WriteLine("An error occurred while trying to receive an event: " + err.Message);
            }
        }
    }
}
