using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NetBox.Collections
{
   /// <summary>
   /// Implement an object pool which allows object to be reusable and cached for a specific amount of time.
   /// This class is thread safe.
   /// </summary>
   /// <typeparam name="T">Type of object to be pooled</typeparam>
   public class ObjectPool<T> : IDisposable where T : class
   {
      // ReSharper disable once StaticMemberInGenericType
      private static readonly TimeSpan SleepInterval = TimeSpan.FromSeconds(1);
      private readonly Func<T> _createFunction;
      private readonly Action<T> _destroyFunction;
      private readonly int _capacity;
      private readonly TimeSpan _maxAge;
      private readonly object _dataLock = new object();
      private readonly List<InstanceTag> _tags;
      private int _instanceCount;
      private int _freeInstances;
      private readonly ManualResetEvent _instanceMayBeAvailable = new ManualResetEvent(false);

      private class InstanceTag
      {
         public T Instance;

         public DateTime LastUsed;

         public bool IsAvailable;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="createFunction">
      /// A function responsible for creating new instances of an object. It must  return a new instance every
      /// time and is required. You should not use any form of instance caching.</param>
      /// <param name="destroyFunction">
      /// When specified is called on the object instance. You are responsible for a proper cleanup as object
      /// pool only removes it from internal instance collection.</param>
      /// <param name="capacity">
      /// Maximum capacity of this pool specifying how many instances the pool
      /// can hold as a maximum.
      /// </param>
      /// <param name="maxAge">
      /// Specifies how long the object can live. When object is used for more than this time it's destroyed
      /// by removing from internal cache and calling the destroy function when specified.
      /// </param>
      public ObjectPool(
         Func<T> createFunction,
         Action<T> destroyFunction,
         int capacity,
         TimeSpan maxAge)
      {
         if(createFunction == null) throw new ArgumentNullException(nameof(createFunction));
         _createFunction = createFunction;
         _destroyFunction = destroyFunction;
         _capacity = capacity;
         _maxAge = maxAge;
         _tags = new List<InstanceTag>(_capacity);
      }

      /// <summary>
      /// Gets an available instance from the object pool by creating a new one, reusing existing one,
      /// or waiting for an available one.
      /// </summary>
      /// <returns></returns>
      public T GetInstance()
      {
         T instance;

         while((instance = TryGetInstance()) == null)
         {
            _instanceMayBeAvailable.WaitOne(SleepInterval);
            _instanceMayBeAvailable.Reset();
         }

         return instance;
      }

      /// <summary>
      /// Returns true if object pool has instances in the cache which have not been used yet.
      /// </summary>
      public bool HasFreeInstance
      {
         get
         {
            lock(_dataLock)
            {
               return _freeInstances > 0;
            }
         }
      }

      /// <summary>
      /// Returns the number of instances in use
      /// </summary>
      public int InstanceCount
      {
         get
         {
            lock(_dataLock)
            {
               return _instanceCount;
            }
         }
      }

      private T TryGetInstance()
      {
         lock(_dataLock)
         {
            CleanupInstances();

            InstanceTag tag;

            if(_freeInstances > 0)
            {
               //find free instance
               tag = _tags.First(t => t.IsAvailable);
               tag.IsAvailable = false;
               tag.LastUsed = DateTime.UtcNow;
               _freeInstances -= 1;
               return tag.Instance;
            }

            if(_instanceCount < _capacity)
            {
               //create new instance as capacity allows it
               tag = new InstanceTag();
               _tags.Add(tag);
               tag.Instance = _createFunction();
               tag.IsAvailable = false;
               tag.LastUsed = DateTime.UtcNow;
               _instanceCount += 1;
               return tag.Instance;
            }

            return null;
         }
      }

      private void CleanupInstances()
      {
         if(_maxAge == TimeSpan.MaxValue) return;
         DateTime now = DateTime.UtcNow;
         foreach(InstanceTag tag in _tags.Where(t => (now - t.LastUsed) > _maxAge).ToList())
         {
            _tags.Remove(tag);
            _destroyFunction?.Invoke(tag.Instance);
            _instanceCount -= 1;
         }
      }

      /// <summary>
      /// Returns instance to the object pool so it can be reused
      /// </summary>
      /// <param name="instance"></param>
      public void ReleaseInstance(T instance)
      {
         lock(_dataLock)
         {
            CleanupInstances();

            InstanceTag tag = _tags.FirstOrDefault(t => ReferenceEquals(t.Instance, instance));
            if(tag != null)   //tag can be cleaned up
            {
               tag.IsAvailable = true;
               _freeInstances += 1;
            }
         }

         _instanceMayBeAvailable.Set();
      }

      /// <summary>
      /// Releases all object instances and calls the destroy function on each of them if you have specified one.
      /// </summary>
      public void Dispose()
      {
         lock(_dataLock)
         {
            //release all instances
            if(_destroyFunction != null)
            {
               foreach(InstanceTag tag in _tags)
               {
                  _destroyFunction(tag.Instance);
               }
            }

            //reset the counters
            _freeInstances = 0;
            _instanceCount = 0;
            _tags.Clear();
         }
      }
   }
}
