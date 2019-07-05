using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBox.Model;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class EnumExtensionsTest
   {
      /*
      [Fact]
      public void GetTag_AttributedMember_PropertiesCorrect()
      {
         EnumTagAttribute tag = AttributedEnum1.Attributed.GetEnumTag();

         Assert.NotNull(tag);
         Assert.Equal("F", tag.Id);
         Assert.Equal("Female", tag.DisplayName);
      }

      [Fact]
      public void GetTag_NonAttributedMember_Handled()
      {
         EnumTagAttribute tag = AttributedEnum1.NotAttributed.GetEnumTag();

         Assert.Null(tag);
      }

      [Fact]
      public void GetTag_IntersectedEnums_DifferentValues()
      {
         EnumTagAttribute tag1 = AttributedEnum1.Attributed.GetEnumTag();
         EnumTagAttribute tag2 = AttributedEnum2.Attributed.GetEnumTag();

         Assert.Equal("F", tag1.Id);
         Assert.Equal("M", tag2.Id);
      }*/

      [Fact]
      public void GetById_ByF_Finds()
      {
         AttributedEnum2 byId = EnumTagAttribute.GetById<AttributedEnum2>("F").Value;
         Assert.Equal(AttributedEnum2.AnotherAttributed, byId);
      }

      [Fact]
      public void GetById_ByNonExistent_DoesntFind()
      {
         AttributedEnum2? byId = EnumTagAttribute.GetById<AttributedEnum2>("1");
         Assert.Null(byId);
      }
   }

   enum AttributedEnum1
   {
      [EnumTag("F", "Female")]
      Attributed,

      NotAttributed
   }

   enum AttributedEnum2
   {
      [EnumTag("M", "Male")]
      Attributed,

      NotAttributed,

      [EnumTag("F", "Female")]
      AnotherAttributed
   }


}