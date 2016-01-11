using System.Globalization;
using System.Text;

namespace Aloneguid.Support
{
   /// <summary>
   /// Global defaults
   /// </summary>
   public static class G
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
