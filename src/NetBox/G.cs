using System.Globalization;
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
   }
}
