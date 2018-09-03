namespace ExampleService.Merged
{
    internal static class MergedDataHelper
    {
        public static int BinarySearch(MergedData.VaryingItem[] array, ushort id)
        {
            int minIdx = 0;
            int maxIdx = array.Length - 1;
            while (minIdx <= maxIdx)
            {
                int idx = (minIdx + maxIdx) / 2;
                if (array[idx].Id == id)
                    return idx;
                else if (array[idx].Id < id)
                    minIdx = idx + 1;
                else
                    maxIdx = idx - 1;
            }
            return -1;
        }
    }
}