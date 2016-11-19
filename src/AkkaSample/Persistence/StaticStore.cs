using AkkaSample.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaSample.Persistence
{
    public static class StaticStore
    {
        private static readonly ConcurrentDictionary<int, int> Store = new ConcurrentDictionary<int, int>();

        public static void Set(int id, int count)
        {
            Store.AddOrUpdate(id, count, (currValue, oldValue) => count);
        }

        public static int Get(int id)
        {
            if (Store.TryGetValue(id, out int value))
                return value;
            return 0;
        }

        public static async Task PrintStoreStatusAsync()
        {
            foreach (var entry in Store)
            {
                await ConsoleHelper.WriteAsync("Actor {0} counted {1} messages.", entry.Key, entry.Value);
            }
        }
    }
}
