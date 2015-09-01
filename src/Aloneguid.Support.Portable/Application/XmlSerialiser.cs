using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Aloneguid.Support.Application
{
   class XmlSerialiser
   {
      private static readonly char[] Boms = { (char)65279 };

      public string Serialise(object obj, bool noNamespaces, bool asFragment, Encoding encoding)
      {
         if (obj == null) return null;

         using (var ms = new MemoryStream())
         {
            Serialise(ms, obj, noNamespaces, asFragment);

            byte[] data = ms.ToArray();

            return RemoveBom(encoding.GetString(data, 0, data.Length));
         }
      }

      private void Serialise(Stream s, object obj, bool noNamespaces, bool asFragment)
      {
         var xmlser = new XmlSerializer(obj.GetType());

         var settings = new XmlWriterSettings { OmitXmlDeclaration = asFragment, Indent = true };

         using (XmlWriter xw = XmlWriter.Create(s, settings))
         {
            if (noNamespaces)
            {
               var ns = new XmlSerializerNamespaces();
               ns.Add("", "");
               xmlser.Serialize(xw, obj, ns);
            }
            else
            {
               xmlser.Serialize(xw, obj);
            }
         }
      }

      private string RemoveBom(string s)
      {
         if (s == null) return null;
         return s.Trim(Boms);
      }

      public T Deserialise<T>(string s, Encoding encoding) where T : class, new()
      {
         if (s == null) return null;

         using (var ms = new MemoryStream(encoding.GetBytes(s)))
         {
            return Deserialise(typeof(T), ms) as T;
         }
      }

      public object Deserialise(Type t, string s, Encoding encoding)
      {
         if (t == null || s == null) return null;

         using (var ms = new MemoryStream(encoding.GetBytes(s)))
         {
            return Deserialise(t, ms);
         }
      }

      private object Deserialise(Type t, Stream s)
      {
         var xs = new XmlSerializer(t);

         return xs.Deserialize(s);
      }
   }
}
