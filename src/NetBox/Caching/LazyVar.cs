using System;
using System.Threading.Tasks;

namespace NetBox.Caching
{
   /// <summary>
   /// Implements a lazy value i.e. that can expire in future
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class LazyVar<T>
   {
      private readonly Func<Task<T>> _renewFuncAsync;
      private readonly Func<T> _renewFunc;
      private DateTime _lastRenewed = DateTime.MinValue;
      private readonly TimeSpan _timeToLive;
      private T _value;
      private bool _readOnce;

      /// <summary>
      /// Creates an instance of a lazy variable with time-to-live value
      /// </summary>
      /// <param name="timeToLive">Time to live. Setting to <see cref="TimeSpan.Zero"/> disables caching completely</param>
      /// <param name="renewFunc"></param>
      public LazyVar(TimeSpan timeToLive, Func<Task<T>> renewFunc)
      {
         _timeToLive = timeToLive;
         _renewFuncAsync = renewFunc ?? throw new ArgumentNullException(nameof(renewFunc));
         _renewFunc = null;
      }

      /// <summary>
      /// Creates an instance of a lazy variable with time-to-live value
      /// </summary>
      /// <param name="timeToLive">Time to live. Setting to <see cref="TimeSpan.Zero"/> disables caching completely</param>
      /// <param name="renewFunc"></param>
      public LazyVar(TimeSpan timeToLive, Func<T> renewFunc)
      {
         _timeToLive = timeToLive;
         _renewFuncAsync = null;
         _renewFunc = renewFunc ?? throw new ArgumentNullException(nameof(renewFunc));
      }

      /// <summary>
      /// Gets the values, renewing it if necessary
      /// </summary>
      /// <returns>Value</returns>
      public async Task<T> GetValueAsync()
      {
         if(_renewFuncAsync == null)
         {
            throw new InvalidOperationException("cannot renew value, async delegate is not specified");
         }

         if(_timeToLive == TimeSpan.Zero)
         {
            if(!_readOnce)
            {
               _value = await _renewFuncAsync();
               _readOnce = true;
            }

            return _value;
         }

         bool expired = (DateTime.UtcNow - _lastRenewed) > _timeToLive;

         if(expired)
         {
            _value = await _renewFuncAsync();
            _lastRenewed = DateTime.UtcNow;
         }

         return _value;
      }

      /// <summary>
      /// Gets the values, renewing it if necessary
      /// </summary>
      /// <returns>Value</returns>
      public T GetValue()
      {
         if (_renewFunc == null)
         {
            throw new InvalidOperationException("cannot renew value, synchronous delegate is not specified");
         }

         if (_timeToLive == TimeSpan.Zero)
         {
            if (!_readOnce)
            {
               _value = _renewFunc();
               _readOnce = true;
            }

            return _value;
         }

         bool expired = (DateTime.UtcNow - _lastRenewed) > _timeToLive;

         if (expired)
         {
            _value = _renewFunc();
            _lastRenewed = DateTime.UtcNow;
         }

         return _value;
      }
   }
}
