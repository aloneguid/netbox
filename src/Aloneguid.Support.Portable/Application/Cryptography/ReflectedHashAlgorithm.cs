using System;
using System.Collections.Generic;
using System.Reflection;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Application.Cryptography
{
   class ReflectedHashAlgorithm : IHashAlgorithm
   {
      private readonly HashType _hashType;
      private object _platformHasher;

      private static readonly Dictionary<HashType, string> HashTypeToTypeName = new Dictionary<HashType, string>
      {
         {HashType.Md5, "System.Security.Cryptography.MD5"}
      };

      public ReflectedHashAlgorithm(HashType hashType)
      {
         _hashType = hashType;

         _platformHasher = CreateAlgorithm(hashType);
      }

      private static object CreateAlgorithm(HashType hashType)
      {
         string className;
         if (!HashTypeToTypeName.TryGetValue(hashType, out className)) return null;

         Type classType = Type.GetType(className);
         if (classType == null) return null;

         MethodInfo info = classType.GetRuntimeMethod("Create", null);

         return null;
      }

      public void Dispose()
      {
         throw new NotImplementedException();
      }

      public byte[] ComputeHash(byte[] buffer)
      {
         throw new NotImplementedException();
      }

      public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
      {
         throw new NotImplementedException();
      }

      public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
      {
         throw new NotImplementedException();
      }

      public byte[] Hash { get; }
   }
}
