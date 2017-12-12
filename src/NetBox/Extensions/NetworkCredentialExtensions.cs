using System.Net;

namespace NetBox.Extensions
{
   /// <summary>
   /// Provides extension methods for <see cref="System.Net.NetworkCredential"/>
   /// </summary>
   public static class NetworkCredentialExtensions
   {
      /// <summary>
      /// Converts to a friendly format which is username:password@domain
      /// </summary>
      /// <param name="credential">Credential to convert</param>
      /// <returns>Friendly string. If some parts are missing they are not added, for example if there is no
      /// domain information you'll get username:password.</returns>
      public static string ToFriendlyString(this NetworkCredential credential)
      {
         if (credential == null) return null;

         string usernameAndPassword;
         
         if(string.IsNullOrEmpty(credential.UserName) && string.IsNullOrEmpty(credential.Password))
         {
            usernameAndPassword = string.Empty;
         }
         else if(string.IsNullOrEmpty(credential.UserName))
         {
            usernameAndPassword = $":{credential.Password}";
         }
         else if(string.IsNullOrEmpty(credential.Password))
         {
            usernameAndPassword = credential.UserName;
         }
         else
         {
            usernameAndPassword = $"{credential.UserName}:{credential.Password}";
         }
         
         return string.IsNullOrEmpty(credential.Domain)
            ? usernameAndPassword
            : $"{usernameAndPassword}@{credential.Domain}";
      }
   }
}
