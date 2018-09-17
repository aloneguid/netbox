using System;
using System.Threading;
using NetBox.Terminal;

namespace NetBox.Runner
{
   class Program
   {
      static void Main(string[] args)
      {
         Console.WriteLine("Hello World!");

         using (var bar = new ConsoleProgressBar(false, 0, 100))
            using(var bar2 = new ConsoleProgressBar(true, 0, 200))
         {
            for (int i = 0; i <= 100; i += 10)
            {
               bar.Value = i;
               bar2.Value = i;
               bar2.Subtitle = $"processed {i}";

               Thread.Sleep(100);

               Console.WriteLine("haha " + i);
            }
         }

         Console.ReadLine();
      }
   }
}
