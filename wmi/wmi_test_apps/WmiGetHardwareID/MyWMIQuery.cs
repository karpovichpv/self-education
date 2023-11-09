using System.Management;

namespace WmiGetHardwareID
{
    public class WmiQuery
    {
        public static void Show()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_NetworkAdapter");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Win32_NetworkAdapter instance");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("AdapterTypeId: {0}", queryObj["AdapterTypeId"]);
                    Console.WriteLine("DeviceID: {0}", queryObj["DeviceID"]);
                    Console.WriteLine("GUID: {0}", queryObj["GUID"]);
                    Console.WriteLine("MACAddress: {0}", queryObj["MACAddress"]);
                    Console.WriteLine("Manufacturer: {0}", queryObj["Manufacturer"]);
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
        }
    }
}
