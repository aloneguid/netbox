using System;
using System.Collections.Generic;
using System.Text;

namespace Housework.Core.Commands.Authoring
{
   interface IFileAuthoring
   {
      string TypeName { get; }

      void Update(string path, ISettings settings);
   }
}
