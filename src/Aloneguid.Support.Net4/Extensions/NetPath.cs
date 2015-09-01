using System.Reflection;

// ReSharper disable once CheckNamespace
namespace System.IO
{
   public static class NetPath
   {
      private static Assembly _currentAssemblyCache;
      private static DirectoryInfo _execDir;

      private static Assembly CurrentAssembly
      {
         get
         {
            return _currentAssemblyCache ?? (_currentAssemblyCache = Assembly.GetExecutingAssembly());
         }
      }

      public static DirectoryInfo ExecDir
      {
         get
         {
            return _execDir ?? (_execDir = new DirectoryInfo((CurrentAssembly != (Assembly)null
               ? Path.GetDirectoryName(CurrentAssembly.Location)
               : (string)null) ?? Environment.CurrentDirectory));
         }
      }
   }
}

