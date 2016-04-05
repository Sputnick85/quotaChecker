using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Storage;
using System.Collections;
using System.Globalization;

namespace quotaChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            String path = null;
            decimal critical = 0;
            decimal warning = 0;
            decimal limit = 0;
            decimal used = 0;
            if (args.Length != 3) {
                Program.Usage();
                Environment.Exit(3);
            }
            try {
                path = args[0];
                critical = decimal.Parse(args[2]);
                warning = decimal.Parse(args[1]);
            } catch (Exception e) {
                Console.WriteLine();
                throw new ApplicationException("Error converting arguments!", e);
            }
            try {
                IFsrmQuotaManager quotaManager = new FsrmQuotaManager();
                IFsrmQuota quota = quotaManager.GetQuota(path);
                limit = quota.QuotaLimit / 1024 / 1024;
                used = quota.QuotaUsed / 1024 / 1024;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error getting Quota '" + path + "'", e);
            }
            CultureInfo enUS = CultureInfo.CreateSpecificCulture("en-US");
            String usageToLimit = (used / 1024).ToString("N") + "GB/" + (limit / 1024).ToString("N") + "GB";
            String perfdata = "'" + path + "'=" +
                used.ToString("F0", enUS) + "MB;" +
                (limit * (warning / 100)).ToString("F0", enUS) + ";" +
                (limit * (critical / 100)).ToString("F0", enUS);
            if (used >= (limit * (critical / 100))) {
                PrintOutput(2, path, usageToLimit, perfdata);args
                Console.WriteLine();
                Environment.Exit(2);
            }
            if (used >= (limit * (warning / 100))) {
                PrintOutput(1, path, usageToLimit, perfdata);
                Environment.Exit(1);
            }
            PrintOutput(0, path, usageToLimit, perfdata);
            Environment.Exit(0);
        }

        static private void PrintOutput(int exitCode, String path, String usageToLimit, String perfdata) {
            String stateString = "UNKNOWN";
            if (exitCode == 0)
                stateString = "OK";
            if (exitCode == 1)
                stateString = "WARNING";
            if (exitCode == 2)
                stateString = "CRITICAL";
            Console.WriteLine(stateString + " - " + path.Replace("\\","\\\\") + " -> " + usageToLimit + " | " + perfdata);
        }

        static private void Usage() {
            Console.WriteLine();
            Console.WriteLine("Usage of quotaChecker:");
            Console.WriteLine();
            Console.WriteLine("quotaChecker.exe $PATH $WARNING $CRITICAL");
            Console.WriteLine();
            Console.WriteLine("$PATH -> C:\\Folder");
            Console.WriteLine("$WARNING -> 90 (90% space left)");
            Console.WriteLine("$CRITICAL -> 95 (95% space left)");
            Console.WriteLine();
            Console.WriteLine("quotaChecker.exe C:\\Folder 90 95");
            Console.WriteLine();
        }


    }
}
