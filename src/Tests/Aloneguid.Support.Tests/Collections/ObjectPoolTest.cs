using System;
using System.Threading;
using Aloneguid.Support.Collections;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Collections
{
   [TestFixture]
   public class ObjectPoolTest
   {
      [Test]
      public void AddInstance_UpToTheLimit_CountersCorrect()
      {
         int i = 0;

         var pool = new ObjectPool<PoolObject>(
            () => new PoolObject((i++).ToString()),
            null,
            2,
            TimeSpan.MaxValue);
         Assert.AreEqual(0, pool.InstanceCount);
         Assert.IsFalse(pool.HasFreeInstance);

         //first instance
         PoolObject obj1 = pool.GetInstance();
         Assert.AreEqual(1, pool.InstanceCount);
         Assert.IsFalse(pool.HasFreeInstance);

         //second instance
         PoolObject obj2 = pool.GetInstance();
         Assert.AreEqual(2, pool.InstanceCount);
         Assert.IsFalse(pool.HasFreeInstance);

         //third instance
         new Thread(() => { pool.ReleaseInstance(obj1);}).Start();

         PoolObject obj3 = pool.GetInstance();
         Assert.AreEqual(2, pool.InstanceCount);
         Assert.IsFalse(pool.HasFreeInstance);
         Assert.IsTrue(ReferenceEquals(obj3, obj1));

         pool.Dispose();

         Assert.AreEqual(0, pool.InstanceCount);
         Assert.IsFalse(pool.HasFreeInstance);
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
