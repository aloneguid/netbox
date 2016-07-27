#if NETFULL
using System.Reflection;

namespace Aloneguid.Support.Data
{
   class DataLoader
   {
      public static string[] LoadFirstNames()
      {
         return Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("first-names.txt");
      }

      public static string[] LoadLastNames()
      {
         return Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("last-names.txt");
      }
   }
}
#endif