using System.Threading;
using NetBox.Terminal.Widgets;

namespace Concord.Runner
{
   class Samples
   {
      public Samples()
      {
      }

      public void ProgressBar()
      {
         using (var bar = new ProgressBar(false))
         {
            for (int i = 0; i < 100; i += 3)
            {
               bar.Value = i;

               Thread.Sleep(100);
            }
         }

      }
   }
}
