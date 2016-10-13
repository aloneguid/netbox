using System.Reflection;

namespace NetBox.Data
{
   class DataLoader
   {
      public static string[] LoadFirstNames()
      {
         return G.ThisAssembly.GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("first-names.txt");
      }

      public static string[] LoadLastNames()
      {
         return G.ThisAssembly.GetSameFolderEmbeddedResourceFileAsLines<DataLoader>("last-names.txt");
      }
   }
}
