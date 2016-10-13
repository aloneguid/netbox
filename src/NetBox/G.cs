using System.Globalization;
using System.Reflection;
using System.Text;

namespace NetBox
{
   /// <summary>
   /// Global defaults
   /// </summary>
   static class G
   {
      /// <summary>
      /// Global default encoding
      /// </summary>
      public static readonly Encoding Enc = Encoding.UTF8;

      /// <summary>
      /// Global default culture
      /// </summary>
      public static readonly CultureInfo C = CultureInfo.InvariantCulture;

      private static Assembly _thisAsm;

      internal static Assembly ThisAssembly
      {
         get
         {
            if(_thisAsm == null)
            {
#if NETFULL
               _thisAsm = Assembly.GetExecutingAssembly();
#endif

#if NETSTANDARD
               _thisAsm = Assembly.Load(new AssemblyName("NetBox"));
#endif
            }

            return _thisAsm;
         }
      }

   }
}
