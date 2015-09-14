using System;
using System.IO;
using Aloneguid.Support.Net45.Azure.Blob;
using Aloneguid.Support.Storage.Blob;
using Aloneguid.Support.Storage.Blob.Files;
using Config.Net;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Integration.Storage
{
   [TestFixture("azure")]
   [TestFixture("disk-directory")]
   public class BlobStorageTest : AbstractTestFixture
   {
      private readonly string _type;
      private IBlobStorage _storage;

      public BlobStorageTest(string type)
      {
         _type = type;
      }

      [SetUp]
      public void SetUp()
      {
         switch (_type)
         {
            case "azure":
               _storage = new AzureBlobStorage(
                  Cfg.Read(TestSettings.AzureStorageName),
                  Cfg.Read(TestSettings.AzureStorageKey),
                  "blobstoragetest");
               break;
            case "disk-directory":
               _storage = new DirectoryFilesBlobStorage(TestDir);
               break;
         }
      }

      private string GetRandomStreamId()
      {
         string id = Guid.NewGuid().ToString();

         using (Stream s = "kjhlkhlkhlkhlkh".ToMemoryStream())
         {
            _storage.UploadFromStream(id, s);
         }

         return id;
      }

      [Test]
      public void Upload_New_CanDownload()
      {
         string id = GetRandomStreamId();

         using (Stream s = new MemoryStream())
         {
            _storage.DownloadToStream(id, s);
         }
      }

      [Test]
      public void OpenStream_New_CanDownload()
      {
         string id = GetRandomStreamId();

         using (Stream s = _storage.OpenStreamToRead(id))
         {
            var ms = new MemoryStream();
            s.CopyTo(ms);

            Assert.Greater(ms.Length, 0);
         }
      }

   }
}
