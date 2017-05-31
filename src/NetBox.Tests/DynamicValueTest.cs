using System;
using Xunit;

namespace NetBox.Tests
{
   public class DynamicValueTest
   {
      [Fact]
      public void String_to_long()
      {
         var dv = new DynamicValue("123");

         long l = dv;

         Assert.Equal(123, l);
      }

      [Fact]
      public void String_to_datetime()
      {
         var d = DateTime.UtcNow;

         var dv = new DynamicValue(d.ToString());

         DateTime d2 = dv;

         Assert.Equal(d.RoundToSecond(), d2);
      }

      [Fact]
      public void Datetime_to_string()
      {
         var d = DateTime.UtcNow;
         var dv = new DynamicValue(d);
         Assert.Equal(d.ToString(), (string)dv);
      }

      [Fact]
      public void Bytearray_to_string()
      {
         var b = Generator.GetRandomBytes(10, 20);
         var dv = new DynamicValue(b);
         Assert.Equal(b.ToHexString(), (string)dv);
      }

      [Fact]
      public void String_to_bytearray()
      {
         string s = Generator.GetRandomBytes(10, 20).ToHexString();
         var dv = new DynamicValue(s);
         Assert.Equal(s.FromHexToBytes(), (byte[])dv);
      }

      [Fact]
      public void Datetimeoffset_to_string()
      {
         DateTimeOffset o = DateTime.UtcNow.RoundToSecond();
         var dv = new DynamicValue(o);
         string s = dv;
         Assert.Equal(o.ToString(), (string)dv);
      }

      [Fact]
      public void String_to_datetimeoffset()
      {
         DateTimeOffset o = DateTime.UtcNow.RoundToSecond();
         var dv = new DynamicValue(o.ToString());
         Assert.Equal(o, (DateTimeOffset)dv);
      }

      [Fact]
      public void Invalid_number_to_int_returns_zero()
      {
         var dv = new DynamicValue("abc");
         int i = dv;
         Assert.Equal(0, i);
      }

      [Fact]
      public void Invalid_datetimeoffset_to_string_returns_null()
      {
         var dv = new DynamicValue("dsfadf");
         Assert.Equal(DateTimeOffset.MinValue, (DateTimeOffset)dv);
      }
   }
}