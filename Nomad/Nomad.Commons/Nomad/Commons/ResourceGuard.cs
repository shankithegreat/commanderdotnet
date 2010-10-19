namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class ResourceGuard
    {
        private static Dictionary<Type, Type> ResourceGuardMap;

        public static IResourceGuard Create(object resource)
        {
            if (resource == null)
            {
                return null;
            }
            IResourceGuard guard = null;
            if (resource is Mutex)
            {
                guard = new MutexResourceGuard(resource);
            }
            if (guard == null)
            {
                Type type;
                if (ResourceGuardMap == null)
                {
                    throw new InvalidOperationException("No guards registered");
                }
                if (!ResourceGuardMap.TryGetValue(resource.GetType(), out type))
                {
                    throw new ArgumentException();
                }
                guard = Activator.CreateInstance(type, new object[] { resource }) as IResourceGuard;
                if (guard == null)
                {
                    throw new InvalidOperationException("Unknown guard type");
                }
            }
            guard.Enter();
            return guard;
        }

        public static void RegisterResourceType(Type resourceType, Type guardType)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException("resourceType");
            }
            if (guardType == null)
            {
                throw new ArgumentNullException("guardType");
            }
            if (ResourceGuardMap == null)
            {
                ResourceGuardMap = new Dictionary<Type, Type>();
            }
            ResourceGuardMap.Add(resourceType, guardType);
        }

        public static void UnregisterResourceType(Type resourceType)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException();
            }
            if (ResourceGuardMap != null)
            {
                ResourceGuardMap.Remove(resourceType);
            }
        }
    }
}

