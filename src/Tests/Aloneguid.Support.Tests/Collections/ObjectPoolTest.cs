using System;
using System.Threading;
using Aloneguid.Support.Collections;
using Xunit;

namespace Aloneguid.Support.Tests.Collections
{
   
   public class ObjectPoolTest
   {
      [Fact]
      public void AddInstance_UpToTheLimit_CountersCorrect()
      {
         int i = 0;

         var pool = new ObjectPool<PoolObject>(
            () => new PoolObject((i++).ToString()),
            null,
            2,
            TimeSpan.MaxValue);
         Assert.Equal(0, pool.InstanceCount);
         Assert.False(pool.HasFreeInstance);

         //first instance
         PoolObject obj1 = pool.GetInstance();
         Assert.Equal(1, pool.InstanceCount);
         Assert.False(pool.HasFreeInstance);

         //second instance
         PoolObject obj2 = pool.GetInstance();
         Assert.Equal(2, pool.InstanceCount);
         Assert.False(pool.HasFreeInstance);

         //third instance
         new Thread(() => { pool.ReleaseInstance(obj1);}).Start();

         PoolObject obj3 = pool.GetInstance();
         Assert.Equal(2, pool.InstanceCount);
         Assert.False(pool.HasFreeInstance);
         Assert.True(ReferenceEquals(obj3, obj1));

         pool.Dispose();

         Assert.Equal(0, pool.InstanceCount);
         Assert.False(pool.HasFreeInstance);
      }

      private class PoolObject
      {
         public string Name { get; set; }

         public PoolObject(string name)
         {
            Name = name;
         }
      }
   }
}
