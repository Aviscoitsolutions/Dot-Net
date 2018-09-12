using System;

namespace ExampleService.Merged
{
    internal static class Helper
    {
        private static DateTime startDate = new DateTime(2008, 01, 01);

        public static uint ToMinimizedTicks(DateTime time)
        {
            TimeSpan nn = time.Subtract(startDate);
            if (nn.Ticks < 0)
                return 0;
            long tickmin = nn.Ticks / 10000000;
            if (tickmin > uint.MaxValue)
                return uint.MaxValue;
            return (uint)tickmin;
        }

        public static DateTime FromMinimizedTicks(uint ticks)
        {
            TimeSpan dif = new TimeSpan((long)ticks * 10000000);
            return startDate.Add(dif);
        }
    }
}