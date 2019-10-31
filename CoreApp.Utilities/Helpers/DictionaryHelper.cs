using System.Collections.Generic;

namespace CoreApp.Utilities.Helpers
{
    public static class DictionaryHelper
    {
        public static Dictionary<string, int> AddOrUpdate(Dictionary<string, int> dictKey, string key)
        {
            if (dictKey.ContainsKey(key))
            {
                dictKey[key] += 1;
            }
            else
            {
                dictKey.Add(key, 1);
            }
            return dictKey;
        }
    }
}