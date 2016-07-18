using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aloneguid.Support.Model;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class EnumExtensionsTest
   {
      [Test]
      public void GetTag_AttributedMember_PropertiesCorrect()
      {
         EnumTagAttribute tag = AttributedEnum1.Attributed.GetEnumTag();

         Assert.IsNotNull(tag);
         Assert.AreEqual("F", tag.Id);
         Assert.AreEqual("Female", tag.DisplayName);
      }

      [Test]
      public void GetTag_NonAttributedMember_Handled()
      {
         EnumTagAttribute tag = AttributedEnum1.NotAttributed.GetEnumTag();

         Assert.IsNull(tag);
      }

      [Test]
      public void GetTag_IntersectedEnums_DifferentValues()
      {
         EnumTagAttribute tag1 = AttributedEnum1.Attributed.GetEnumTag();
         EnumTagAttribute tag2 = AttributedEnum2.Attributed.GetEnumTag();

         Assert.AreEqual("F", tag1.Id);
         Assert.AreEqual("M", tag2.Id);
      }

      [Test]
      public void GetById_ByF_Finds()
      {
         AttributedEnum2 byId = EnumTagAttribute.GetById<AttributedEnum2>("F").Value;
         Assert.AreEqual(AttributedEnum2.AnotherAttributed, byId);
      }

      [Test]
      public void GetById_ByNonExistent_DoesntFind()
      {
         AttributedEnum2? byId = EnumTagAttribute.GetById<AttributedEnum2>("1");
         Assert.IsNull(byId);
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
