using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Logging
{
   /// <summary>
   /// Utility class to server log clients
   /// </summary>
   class LogClient : ILog
   {
      private readonly IEnumerable<ILogReceiver> _receivers;
      private readonly object _eventLock;
      private readonly string _name;

      public LogClient(Type type, IEnumerable<ILogReceiver> receivers, object eventLock)
      {
         if(type == null) throw new ArgumentNullException(nameof(type));
         if(receivers == null) throw new ArgumentNullException(nameof(receivers));
         if(eventLock == null) throw new ArgumentNullException(nameof(eventLock));

         _name = type.Name;
         _receivers = receivers;
         _eventLock = eventLock;
      }

      private void Serve(LogSeverity severity, string format, params object[] parameters)
      {
         string threadName = Task.CurrentId.ToString();
         DateTime eventTime = DateTime.UtcNow;
         Exception error;

         if(parameters != null && parameters.Length > 0)
         {
            error = parameters[parameters.Length - 1] as Exception;
            if(error != null)
            {
               Array.Resize(ref parameters, parameters.Length - 1);
            }
         }
         else
         {
            error = null;
         }

         var message = string.Format(format, parameters);

         //send the message
         lock(_eventLock)
         {
            foreach(ILogReceiver receiver in _receivers)
            {
               //Not sure if logging in parallel will make sense. I'm not doing this for now because
               //most of my apps have just one receiver
               receiver.Send(severity, _name, threadName, eventTime, message, error);
            }
         }
      }

      public void D(string format, params object[] parameters)
      {
         Serve(LogSeverity.Debug, format, parameters);
      }

      public void E(string format, params object[] parameters)
      {
         Serve(LogSeverity.Error, format, parameters);
      }

      public void I(string format, params object[] parameters)
      {
         Serve(LogSeverity.Info, format, parameters);
      }

      public void W(string format, params object[] parameters)
      {
         Serve(LogSeverity.Warning, format, parameters);
      }
   }
}
