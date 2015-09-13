using System;
using System.Collections.Generic;

namespace Aloneguid.Support.Logging
{
   public static class L
   {
      private static readonly List<ILogReceiver> LogReceivers = new List<ILogReceiver>();
      private static readonly object EventLock = new object();

      public static void AddReceiver(ILogReceiver receiver)
      {
         if(receiver == null) throw new ArgumentNullException("receiver");

         lock(LogReceivers)
         {
            LogReceivers.Add(receiver);
         }
      }

      public static ILog G<T>()
      {
         return new LogClient(typeof(T), LogReceivers, EventLock);
      }

      public static ILog G(Type t)
      {
         return new LogClient(t, LogReceivers, EventLock);
      }
   }
}
