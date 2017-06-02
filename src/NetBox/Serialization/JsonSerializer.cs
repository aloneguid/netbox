using System.IO;
using NetBox.Serialization.Core;
using System.Text;

namespace NetBox.Serialization
{
   /// <summary>
   /// JSON serializer implemented according to specs at http://www.json.org/. The class is based on
   /// <see cref="WalkingSerializer"/>. Not ready for production use yet!
   /// </summary>
   public class JsonSerializer : WalkingSerializer
   {
      private const string ContainerBegin = "{";
      private const string ContainerEnd = "}";

      /// <summary>
      /// Serializes into stream as JSON
      /// </summary>
      public void Serialize(object instance, Stream s)
      {
         using (var writer = new StreamWriter(s, Encoding.UTF8, 1024, true))
         {
            base.Serialize(instance, writer);
         }
      }

      /// <summary>
      /// Puts the value to the stream according to JSON specs
      /// </summary>
      protected override void SerializeValue(Node node, object value, object state)
      {
         base.SerializeValue(node, value, state);
      }

      /// <summary>
      /// Writes the beginning of container ({)
      /// </summary>
      protected override bool BeforeContainerSerialize(Node node, object state, object instance, out object newState)
      {
         var writer = state as StreamWriter;

         writer.Write(ContainerBegin);

         newState = state;
         return true;
      }

      /// <summary>
      /// Writed the end of container (})
      /// </summary>
      protected override object AfterContainerSerialize(Node node, object containerState, object previousState)
      {
         var writer = containerState as StreamWriter;

         writer.Write(ContainerEnd);

         return previousState;
      }

   }
}
