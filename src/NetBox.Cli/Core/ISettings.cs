using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Cli.Core
{
   public interface ISettings
   {
      string Get(string key);
   }
}
