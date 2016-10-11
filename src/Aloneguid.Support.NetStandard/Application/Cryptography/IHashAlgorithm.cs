using System;
using System.IO;

namespace Aloneguid.Support.Application.Cryptography
{
   interface IHashAlgorithm : IDisposable
   {
      byte[] ComputeHash(byte[] buffer);

      byte[] ComputeHash(Stream stream);
   }
}
