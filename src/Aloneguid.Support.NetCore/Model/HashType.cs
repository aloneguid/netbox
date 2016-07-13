namespace Aloneguid.Support.Model
{
   /// <summary>
   /// Supported hashing algorithms
   /// </summary>
   public enum HashType
   {
      /// <summary>
      /// 500Mb hashes in 1462 ms, size: 16b
      /// </summary>
      Md5,

      /// <summary>
      /// 500Mb hashes in 1644 ms, size: 20b
      /// </summary>
      Sha1,

      /// <summary>
      /// 500Mb hashes in 5618 ms, size: 32b
      /// </summary>
      Sha256,

      /// <summary>
      /// 500Mb hashes in 3839 ms, size: 48b
      /// </summary>
      Sha384,

      /// <summary>
      /// 500Mb hashes in 3820 ms, size: 64b
      /// </summary>
      Sha512,

#if !NETCORE
      /// <summary>
      /// 500Mb hashes in 7066 ms, size: 20b
      /// </summary>
      RipeMd160
#endif
   }

}
