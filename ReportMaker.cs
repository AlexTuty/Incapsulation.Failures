using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    internal enum FailureType
    {
        UnexpectedShutDown = 0,
        ShortNonResponding = 1,
        HardwareFailures = 2,
        ConnectionProblems = 3
    }

    internal class Device
    {
        public readonly int DeviceId;
        public readonly string Name;

        public Device(int deviceId, string name)
        {
            DeviceId = deviceId;
            Name = name;
        }
    }

    public class Common
    {
        internal static bool IsFailureSerious(FailureType failureType)
        {
            return failureType == FailureType.UnexpectedShutDown
                || failureType == FailureType.HardwareFailures;
        }

        internal static bool Earlier(DateTime failureDate, DateTime beforeDate)
        {
            return failureDate < beforeDate;
        }
    }

    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var beforeDate = new DateTime(year, month, day);
            var failures = Enumerable.Range(0, failureTypes.Length)
                .Select(i => Tuple.Create(
                    (FailureType)failureTypes[i],
                    new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]),
                    new Device((int)devices[i]["DeviceId"], (string)devices[i]["Name"])))
                .ToArray();
            return FindDevicesFailedBeforeDate(beforeDate, failures);
        }

        private static List<string> FindDevicesFailedBeforeDate(
            DateTime beforeDate,
            Tuple<FailureType, DateTime, Device>[] failures)
        {
            return failures
                .Where(f => Common.IsFailureSerious(f.Item1) && Common.Earlier(f.Item2, beforeDate))
                .Select(f => f.Item3.Name)
                .ToList();
        }
    }
}
