using System;
using System.Net;
using Xunit;

namespace NetBox.Tests.Extensions
{
   public class NetworkCredentialTest
   {
      [Theory]
      [InlineData(null)]
      [InlineData("ivan@isolineltd.com")]
      [InlineData("ivan:pass123@isolineltd.com")]
      [InlineData("ivan:pass123")]
      [InlineData(":pass123")]
      public void FriendlyString_Variable_Variable(string sample)
      {
         NetworkCredential cred = sample.ToNetworkCredential();

         string back = cred.ToFriendlyString();

         Assert.Equal(sample, back);
      }
   }
}
