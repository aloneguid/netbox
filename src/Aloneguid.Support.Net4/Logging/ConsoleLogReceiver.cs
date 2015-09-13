using System;
using System.Threading;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Logging
{
   public class ConsoleLogReceiver : ILogReceiver
   {
      public void Send(LogSeverity severity, string sourceName, string threadName, DateTime eventTime, string message, Exception error)
      {
         string threadId = Thread.CurrentThread.Name;
         if (string.IsNullOrEmpty(threadId)) threadId = Thread.CurrentThread.ManagedThreadId.ToString();

         Console.WriteLine(@"{0}|{1}|{2}|{3}{4}",
            eventTime.ToString("H:mm:ss,fff"),
            sourceName,
            threadId,
            message,
            error == null ? string.Empty : (Environment.NewLine + error));
      }
   }
}
