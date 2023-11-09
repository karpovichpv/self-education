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
                string wmiQuery = "SELECT * FROM __InstanceCreationEvent WITHIN 10 TargetInstance.Caption = 'notepad'";
                ManagementEventWatcher watcher = new ManagementEventWatcher(wmiQuery);
                Console.WriteLine("Waiting for an event...");

                watcher.EventArrived += new EventArrivedEventHandler(WmiEventHandler);
                ManagementBaseObject eventObj = watcher.WaitForNextEvent();

                Console.WriteLine("{0} event occurred.", eventObj["__CLASS"]);

                // Cancel the event subscription
                watcher.Stop();
                return;
            }
            catch (ManagementException err)
            {
                Console.WriteLine("An error occurred while trying to receive an event: " + err.Message);
            }
        }

        private static void WmiEventHandler(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("Active :          " + e.NewEvent.Properties["Name"].Value.ToString());
        }
    }
}
