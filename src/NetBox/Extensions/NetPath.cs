#if NETFULL
using System.Reflection;

namespace System.IO
{
   /// <summary>
   /// <see cref="Path"/> extensions. Due to the fact <see cref="Path"/> is a static class and cannot be extended
   /// with extension methods this is implemented a new static class.
   /// </summary>
   public static class NetPath
   {
      private static Assembly _currentAssemblyCache;
      private static bool _execDirTried;
      private static string _execDir;
      private static bool _execDirInfoTried;
      private static DirectoryInfo _execDirInfo;

      private static Assembly CurrentAssembly
      {
         get
         {
            return _currentAssemblyCache ?? (_currentAssemblyCache = Assembly.GetExecutingAssembly());
         }
      }

      /// <summary>
      /// Gets current assembly execution directory in a more reliable way
      /// </summary>
      public static string ExecDir
      {
         get
         {
            if (!_execDirTried)
            {
               _execDir = (CurrentAssembly != null
                  ? Path.GetDirectoryName(CurrentAssembly.Location)
                  : null) ?? Environment.CurrentDirectory;

               _execDirTried = true;
            }

            return _execDir;
         }
      }
      /// <summary>
      /// Gets current assembly execution directory information in a more reliable way
      /// </summary>
      public static DirectoryInfo ExecDirInfo
      {
         get
         {
            if (!_execDirInfoTried)
            {
               string execDir = ExecDir;
               _execDirInfo = execDir == null ? null : new DirectoryInfo(execDir);

               _execDirInfoTried = true;
            }

            return _execDirInfo;
         }
      }
   }
}
#endif