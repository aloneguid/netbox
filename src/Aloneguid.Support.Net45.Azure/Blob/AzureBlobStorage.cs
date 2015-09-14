﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Aloneguid.Support.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Aloneguid.Support.Net45.Azure.Blob
{
   public class AzureBlobStorage : AzureStorage, IBlobStorage
   {
      private readonly CloudBlobContainer _blobContainer;

      public AzureBlobStorage(string accountName, string key, string containerName)
         : base(accountName, key)
      {
         ValidateContainerName(containerName);

         CloudBlobClient blobClient = Account.CreateCloudBlobClient();

         _blobContainer = blobClient.GetContainerReference(containerName);
         _blobContainer.CreateIfNotExists();
      }

      private void ValidateContainerName(string containerName)
      {
         if (containerName == null) throw new ArgumentNullException(nameof(containerName));

         /* from MSDN: 
          * 
          A container name must be a valid DNS name, conforming to the following naming rules:
          1. Container names must start with a letter or number, and can contain only letters, numbers, and the dash (-) character.
          2. Every dash (-) character must be immediately preceded and followed by a letter or number; consecutive dashes are not permitted in container names.
          3. All letters in a container name must be lowercase.
          4. Container names must be from 3 through 63 characters long.
         */

         //1. todo

         //2. todo

         //3. check that all characters are lowercase
         for (int i = 0; i < containerName.Length; i++)
         {
            if (char.IsLetter(containerName[i]) && !char.IsLower(containerName, i))
            {
               throw new ArgumentOutOfRangeException(nameof(containerName),
                  $"container [{containerName}] has uppercase character at position {i}");
            }
         }

         //4. check for length
         if (containerName.Length < 3 || containerName.Length > 63)
         {
            throw new ArgumentOutOfRangeException(nameof(containerName),
               $"container [{containerName}] length must be between 3 and 63 but it's {containerName.Length}");
         }
      }

      public IEnumerable<string> List(string prefix)
      {
         return _blobContainer.ListBlobs()
            .OfType<CloudBlockBlob>()
            .Where(b => string.IsNullOrEmpty(prefix) || b.Name.StartsWith(prefix))
            .Select(b => ToUserId(b.Name));
      }

      public void Delete(string id)
      {
         if (id == null) throw new ArgumentNullException(nameof(id));
         id = ToInternalId(id);

         CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(id);
         blob.DeleteIfExists();
      }

      public void UploadFromStream(string id, Stream sourceStream)
      {
         if (id == null) throw new ArgumentNullException(nameof(id));
         if (sourceStream == null) throw new ArgumentNullException(nameof(sourceStream));
         id = ToInternalId(id);

         CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(id);
         blob.UploadFromStream(sourceStream);
      }

      public void DownloadToStream(string id, Stream targetStream)
      {
         if (id == null) throw new ArgumentNullException(nameof(id));
         if (targetStream == null) throw new ArgumentNullException(nameof(targetStream));
         id = ToInternalId(id);

         CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(id);
         blob.DownloadToStream(targetStream);
      }

      public Stream OpenStreamToRead(string id)
      {
         if (id == null) throw new ArgumentNullException(nameof(id));
         id = ToInternalId(id);

         CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(id);
         return blob.OpenRead();
      }

      public bool Exists(string id)
      {
         CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(ToInternalId(id));
         try
         {
            //this is the only reliable way to check the existence :(
            blob.FetchAttributes();
            return true;
         }
         catch (StorageException ex)
         {
            WebException wex = ex.InnerException as WebException;
            var hwr = wex?.Response as HttpWebResponse;
            if (hwr != null && hwr.StatusCode == HttpStatusCode.NotFound)
            {
               return false;
            }

            throw;
         }

      }

      private static string ToInternalId(string userId)
      {
         return HttpUtility.UrlEncode(userId);
      }

      private static string ToUserId(string internalId)
      {
         return HttpUtility.UrlDecode(internalId);
      }
   }
}
