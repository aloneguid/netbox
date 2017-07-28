/*using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NetBox.Application
{
   class EasyHttp
   {
      public static async Task<string> HttpGetAsync(string url, string contentType)
      {
         using (var client = new HttpClient())
         {
            client.DefaultRequestHeaders.Accept.Clear();
            if (contentType != null) client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            using (HttpResponseMessage response = await client.GetAsync(url))
            {
               using (HttpContent content = response.Content)
               {
                  string result = await content.ReadAsStringAsync();

                  return result;
               }
            }
         }
      }
   }
}
*/