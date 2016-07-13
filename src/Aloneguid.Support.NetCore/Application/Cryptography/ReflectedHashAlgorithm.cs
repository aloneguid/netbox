#if PORTABLE
using System;
using System.Collections.Generic;
using System.Reflection;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Application.Cryptography
{
   class ReflectedHashAlgorithm : IHashAlgorithm
   {
      private readonly dynamic _instance;

      private static readonly Dictionary<HashType, string> HashTypeToTypeName = new Dictionary<HashType, string>
      {
         {HashType.Md5, "System.Security.Cryptography.MD5"},
         {HashType.Sha1, "System.Security.Cryptography.SHA1"},
         {HashType.Sha256, "System.Security.Cryptography.SHA256"},
         {HashType.Sha384, "System.Security.Cryptography.SHA384"},
         {HashType.Sha512, "System.Security.Cryptography.SHA512"},
         {HashType.RipeMd160, "System.Security.Cryptography.RIPEMD160"}
      };

      public ReflectedHashAlgorithm(HashType hashType)
      {
         _instance = CreateAlgorithm(hashType);

         if(_instance == null)
            throw new NotSupportedException($"cannot create {hashType} on this platform");
      }

      private static object CreateAlgorithm(HashType hashType)
      {
         string className;
         if(!HashTypeToTypeName.TryGetValue(hashType, out className)) return null;

         Type classType = Type.GetType(className);
         if(classType == null) return null;

         MethodInfo info = classType.GetRuntimeMethod("Create", new Type[0]);
         object instance = info.Invoke(null, null);
         return instance;
      }

      public void Dispose()
      {
         _instance.Dispose();
      }

      public byte[] ComputeHash(byte[] buffer)
      {
         return _instance.ComputeHash(buffer);
      }

      public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
      {
         return _instance.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
      }

      public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
      {
         return _instance.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
      }

      public byte[] Hash => _instance.Hash;
   }
}
#endif