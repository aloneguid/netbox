using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aloneguid.Support.NetCore.Tests
{
   public class Program
   {
      public static void Main(string[] args)
      {
         Console.WriteLine(Generator.GetRandomInt(0, 10));
         Console.ReadLine();
      }
   }
}
