using System.Management;

namespace WmiPermanentEvent
{
    internal class PermanentEventRegister
    {
        public static void PersistWMI()
        {
            ManagementObject? myEventFilter = null;
            ManagementObject? myEventConsumer = null;
            ManagementObject? myBinder = null;

            try
            {
                ManagementScope scope = new(@"\\.\root\subscription");

                ManagementClass wmiEventFilter = new(scope, new ManagementPath("__EventFilter"), null);
                string strQuery = @"SELECT * FROM __InstanceCreationEvent WITHIN 5 "
                    + "WHERE TargetInstance ISA \"Win32_Process\" "
                    + "AND TargetInstance.Name = \"notepad.exe\"";

                WqlEventQuery myEventQuery = new WqlEventQuery(strQuery);
                myEventFilter = wmiEventFilter.CreateInstance();
                myEventFilter["Name"] = "demoEventFilter";
                myEventFilter["Query"] = myEventQuery.QueryString;
                myEventFilter["QueryLanguage"] = myEventQuery.QueryLanguage;
                myEventFilter["EventNameSpace"] = @"\root\cimv2";
                myEventFilter.Put();
                Console.WriteLine("[*] Event filter created.");

                myEventConsumer = new ManagementClass(scope, new ManagementPath("LogFileEventConsumer"), null)
                    .CreateInstance();
                myEventConsumer["Name"] = "LogEventConsumer";
                myEventConsumer["FileName"] = "C:\\Logs\\ProcLog2.log";
                myEventConsumer["Text"] = "New process has been created: %TargetInstance.Name%;" +
                    " CreationDate: %TargetInstance.CreationDate%";
                myEventConsumer.Put();

                Console.WriteLine("[*] Event consumer created.");

                myBinder = new ManagementClass(scope, new ManagementPath("__FilterToConsumerBinding"), null)
                    .CreateInstance();
                myBinder["Filter"] = myEventFilter.Path.RelativePath;
                myBinder["Consumer"] = myEventConsumer.Path.RelativePath;
                myBinder.Put();

                Console.WriteLine("[*] Subscription created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
