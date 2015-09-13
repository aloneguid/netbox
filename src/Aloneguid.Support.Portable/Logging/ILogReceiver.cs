using System;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Logging
{
   public interface ILogReceiver
   {
      void Send(LogSeverity severity,
         string sourceName,
         string threadName,
         DateTime eventTime,
         string message,
         Exception error);
   }
}
