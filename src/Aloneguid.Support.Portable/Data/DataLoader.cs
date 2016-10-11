using System.Reflection;

namespace Aloneguid.Support.Data
{
   class DataLoader
   {
      public static string[] LoadFirstNames()
      {
#if NETFULL
         return Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("first-names.txt");
#else
         return Assembly.GetEntryAssembly().GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("first-names.txt");
#endif
      }

      public static string[] LoadLastNames()
      {
#if NETFULL
         return Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("last-names.txt");
#else
         return Assembly.GetEntryAssembly().GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("last-names.txt");
#endif
      }
   }
}
