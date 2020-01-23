using System;
using System.Collections.Generic;
using System.Text;

namespace Housework.Core
{
   public interface ISettings
   {
      string Get(string key);
   }
}
