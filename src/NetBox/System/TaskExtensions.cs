/*
 * b.             8 8 8888888888 8888888 8888888888 8 888888888o       ,o888888o.  `8.`8888.      ,8' 
 * 888o.          8 8 8888             8 8888       8 8888    `88.  . 8888     `88. `8.`8888.    ,8'  
 * Y88888o.       8 8 8888             8 8888       8 8888     `88 ,8 8888       `8b `8.`8888.  ,8'   
 * .`Y888888o.    8 8 8888             8 8888       8 8888     ,88 88 8888        `8b `8.`8888.,8'    
 * 8o. `Y888888o. 8 8 888888888888     8 8888       8 8888.   ,88' 88 8888         88  `8.`88888'     
 * 8`Y8o. `Y88888o8 8 8888             8 8888       8 8888888888   88 8888         88  .88.`8888.     
 * 8   `Y8o. `Y8888 8 8888             8 8888       8 8888    `88. 88 8888        ,8P .8'`8.`8888.    
 * 8      `Y8o. `Y8 8 8888             8 8888       8 8888      88 `8 8888       ,8P .8'  `8.`8888.   
 * 8         `Y8o.` 8 8888             8 8888       8 8888    ,88'  ` 8888     ,88' .8'    `8.`8888.  
 * 8            `Yo 8 888888888888     8 8888       8 888888888P       `8888888P'  .8'      `8.`8888. 
 * 
 * by aloneguid. GitHub: https://github.com/aloneguid/netbox
 */


using System.Threading.Tasks;

namespace System
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Task utility methods
   /// </summary>
   static class TaskExtensions
   {
      /// <summary>
      /// Fire-and-forget without compiler warnings
      /// </summary>
      /// <param name="task"></param>
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "task")]
      public static void Forget(this Task task)
      {
      }
   }
}
