using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetBox.Serialization.Core;

namespace NetBox.Serialization
{
   /// <summary>
   /// JSON serializer implemented according to specs at http://www.json.org/
   /// </summary>
   public class JsonSerializer : WalkingSerializer
   {
      /// <summary>
      /// Serializes into stream as JSON
      /// </summary>
      public void Serialize(object instance, Stream s)
      {
         base.Serialize(instance, s);
      }

      /// <summary>
      /// Puts value into stream
      /// </summary>
      protected override void SerializeValue(Node node, object value, object state)
      {
         base.SerializeValue(node, value, state);
      }


   }
}
