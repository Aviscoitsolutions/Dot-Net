using System;
using System.Collections.Generic;

namespace ExampleService.Merged
{
    internal class VaryingItemStorage
    {
        private Dictionary<ushort, MergedData.VaryingItem> items;
        public const ushort MaxId = MergedData.VaryingItem.IdPartMask;

        public VaryingItemStorage()
        {
            this.items = new Dictionary<ushort, MergedData.VaryingItem>();
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public void Add(ushort id, long value)
        {
            if (id > MaxId)
                throw new ArgumentException("Id cannot be bigger than " + MaxId, "id");
            this.items[id] = new MergedData.VaryingItem { Id = id, Value = value };
        }

        public MergedData.VaryingItem[] GetCalculated()
        {
            if (this.items.Count == 0)
                return null;
            MergedData.VaryingItem[] array = new MergedData.VaryingItem[this.items.Count];
            this.items.Values.CopyTo(array, 0);
            Array.Sort(array, (a, b) => a.Id.CompareTo(b.Id));
            return array;
        }
    }
}