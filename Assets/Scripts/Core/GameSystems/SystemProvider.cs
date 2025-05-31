using System;
using System.Collections.Generic;

namespace Core.GameSystems
{
    public static class SystemProvider
    {
        private static readonly Dictionary<Type,object> systems = new Dictionary<Type, object>();

        public static void Register<T>(T system) where T : class
        {
            var type = typeof(T);
            if(systems.ContainsKey(type)) throw new Exception($"System of type {type} already registered");
            systems.Add(type, system);
        }

        public static T GetSystem<T>() where T : class
        {
            var type = typeof(T);
            if(systems.ContainsKey(type)) return (T)systems[type];
            else throw new Exception($"System of type {type} not registered");
        }
        
        public static void Clear()
        {
            systems.Clear();
        }
    }
}