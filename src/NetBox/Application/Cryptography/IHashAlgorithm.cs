using System;
using System.IO;

namespace NetBox.Application.Cryptography
{
   interface IHashAlgorithm : IDisposable
   {
      byte[] ComputeHash(byte[] buffer, byte[] salt);

      byte[] ComputeHash(Stream stream, byte[] salt);
   }
}
