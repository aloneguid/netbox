using Config.Net;

namespace Aloneguid.Support.Tests.Integration
{
   static class TestSettings
   {
      public static readonly Setting<string> AzureStorageName = new Setting<string>("Azure.Storage.Name", null);

      public static readonly Setting<string> AzureStorageKey = new Setting<string>("Azure.Storage.Key", null);
   }
}
