using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace NetBox.Extensions
{
   public static class TaskExtensions
   {
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "task")]
      public static void Forget(this Task task)
      {
      }

   }
}
