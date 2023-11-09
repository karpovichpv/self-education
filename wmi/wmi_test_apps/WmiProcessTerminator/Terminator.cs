using System.Management;

namespace WmiInstalledApps
{
    public class Terminator
    {
        public static void Kill()
        {
            try
            {
                List<ManagementObject> notepadObjects = GetNotepadProcess();

                foreach (ManagementObject notepadProcess in notepadObjects)
                {
                    ManagementBaseObject inParams = notepadProcess.GetMethodParameters("Terminate");
                    ManagementBaseObject outParams = notepadProcess.InvokeMethod("Terminate", inParams, options: null);
                    Console.WriteLine("Out parameters:");
                    Console.WriteLine("ReturnValue: " + outParams["ReturnValue"]);
                }

            }
            catch (ManagementException err)
            {
                Console.WriteLine("An error occurred while trying to execute the WMI method: " + err.Message);
            }
        }

        private static List<ManagementObject> GetNotepadProcess()
        {
            try
            {
                ManagementObjectSearcher searcher = new(
                    "root\\CIMV2",
                    "SELECT * FROM Win32_Process WHERE Name = \"notepad.exe\"");

                List<ManagementObject> objects = new();

                foreach (ManagementObject queryObj in searcher.Get())
                    objects.Add(queryObj);

                return objects;
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
            throw new Exception("notepad isn't running");
        }
    }
}
