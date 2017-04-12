using System;
using System.Web;
using System.Web.Caching;

namespace todoclient.Infrastructure
{
    public static class Scheduler
    {
        private static CacheItemRemovedCallback OnCacheRemove = null;

        public static void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        private static void CacheItemRemoved(string key, object value, CacheItemRemovedReason r)
        {
            Synchronizer synchronizer = new Synchronizer();
            synchronizer.UpdateBufferStorage();
            AddTask(key, Convert.ToInt32(value));
        }
    }
}