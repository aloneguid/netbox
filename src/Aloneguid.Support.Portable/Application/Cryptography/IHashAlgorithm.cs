using System;

namespace Aloneguid.Support.Application.Cryptography
{
   interface IHashAlgorithm : IDisposable
   {
      byte[] ComputeHash(byte[] buffer);

      int TransformBlock(
         byte[] inputBuffer,
         int inputOffset,
         int inputCount,
         byte[] outputBuffer,
         int outputOffset
         );

      byte[] TransformFinalBlock(
         byte[] inputBuffer,
         int inputOffset,
         int inputCount
         );

      byte[] Hash { get; }
   }
}
