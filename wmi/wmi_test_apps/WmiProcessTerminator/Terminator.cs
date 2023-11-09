using System.Management;

namespace WmiInstalledApps
{
    public class Terminator
    {
        public static void Kill()
        {
            try
            {
                ManagementObject notepadObject = GetNotepadProcess();

                // Obtain in-parameters for the method
                ManagementBaseObject inParams =
                    notepadObject.GetMethodParameters("Terminate");

                // Add the input parameters.

                // Execute the method and obtain the return values.
                ManagementBaseObject outParams =
                    notepadObject.InvokeMethod("Terminate", inParams, null);

                // List outParams
                Console.WriteLine("Out parameters:");
                Console.WriteLine("ReturnValue: " + outParams["ReturnValue"]);
            }
            catch (ManagementException err)
            {
                Console.WriteLine("An error occurred while trying to execute the WMI method: " + err.Message);
            }
        }

        private static ManagementObject GetNotepadProcess()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Win32_Process instance");
                    Console.WriteLine("-----------------------------------");

                    bool isNotepad = queryObj["Caption"].ToString().ToLower().Contains("notepad");
                    if (isNotepad)
                        return queryObj;
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
            throw new Exception("notepad isn't running");
        }
    }
}
