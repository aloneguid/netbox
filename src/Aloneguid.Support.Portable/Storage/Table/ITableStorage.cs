using System.Collections.Generic;

namespace Aloneguid.Support.Storage.Table
{
   public interface ITableStorage : ISimpleTableStorage
   {
      IEnumerable<TableRow> Get(string tableName, string partitionKey, string rowKey, int maxRecords);
   }
}
