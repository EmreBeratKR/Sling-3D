using System;
using UnityEngine;

namespace Helpers
{
    public static class JsonPrefs
    {
        public static void Save<T>(string key, T data)
        {
            var dataAsJson = JsonUtility.ToJson(data);
            
            PlayerPrefs.SetString(key, dataAsJson);
        }

        public static T Load<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                throw Exceptions.KeyDoesNotExist(key);
            }
            
            var dataAsJson = PlayerPrefs.GetString(key);

            try
            {
                return JsonUtility.FromJson<T>(dataAsJson);
            }
            catch
            {
                throw Exceptions.InvalidJsonValueWithKey(key);
            }
        }

        public static void SaveArray<T>(string key, T[] array)
        {
            var arrayAsJson = JsonHelper.ToJson(array);
            
            PlayerPrefs.SetString(key, arrayAsJson);
        }

        public static T[] LoadArray<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                throw Exceptions.KeyDoesNotExist(key);
            }
            
            var dataAsJson = PlayerPrefs.GetString(key);

            try
            {
                return JsonHelper.FromJson<T>(dataAsJson);
            }
            catch
            {
                throw Exceptions.InvalidJsonValueWithKey(key);
            }
        }
        
        
        
        private static class Exceptions
        {
            public static Exception KeyDoesNotExist(string key)
            {
                return new Exception($"Key [{key}] Does Not Exist]");
            }
            
            public static Exception InvalidJsonValueWithKey(string key)
            {
                return new Exception($"Invalid Json Value with key [{key}]");
            }
        }
    }
}
