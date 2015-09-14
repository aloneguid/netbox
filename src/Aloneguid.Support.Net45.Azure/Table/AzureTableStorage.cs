﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Generics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Aloneguid.Support.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using IAzTableEntity = Microsoft.WindowsAzure.Storage.Table.ITableEntity;
using AzSE = Microsoft.WindowsAzure.Storage.StorageException;
using MeSE = Aloneguid.Support.Storage.StorageException;

namespace Aloneguid.Support.Net45.Azure.Table
{
   public class AzureTableStorage : ITableStorage
   {
      private const int MaxInsertLimit = 100;
      private const string PartitionKeyName = "PartitionKey";
      private const string RowKeyName = "RowKey";
      private readonly CloudTableClient _client;
      private static readonly ConcurrentDictionary<string, TableTag> TableNameToTableTag = new ConcurrentDictionary<string, TableTag>();
      private static readonly object RefLock = new object();
      private static readonly Regex TableNameRgx = new Regex("^[A-Za-z][A-Za-z0-9]{2,62}$");

      private class TableTag
      {
         public CloudTable Table { get; set; }

         public bool Exists { get; set; }
      }

      public AzureTableStorage(string accountName, string storageKey)
      {
         if (accountName == null) throw new ArgumentNullException(nameof(accountName));
         if (storageKey == null) throw new ArgumentNullException(nameof(storageKey));

         var account = new CloudStorageAccount(new StorageCredentials(accountName, storageKey), true);
         _client = account.CreateCloudTableClient();
      }

      public bool HasOptimisticConcurrency
      {
         get { return true; }
      }

      public IEnumerable<string> ListTableNames()
      {
         IEnumerable<CloudTable> tables = _client.ListTables();

         return tables == null ? null : tables.Select(t => t.Name);
      }

      public void Delete(string tableName)
      {
         CloudTable table = GetTable(tableName, false);
         if (table != null)
         {
            lock (RefLock)
            {
               table.Delete();
               TableTag tag;
               TableNameToTableTag.TryRemove(tableName, out tag);
            }
         }
      }

      public IEnumerable<TableRow> Get(string tableName, string partitionKey)
      {
         return Get(tableName, partitionKey, null, -1);
      }

      public TableRow Get(string tableName, string partitionKey, string rowKey)
      {
         return Get(tableName, partitionKey, rowKey, -1).FirstOrDefault();
      }

      public IEnumerable<TableRow> Get(string tableName, string partitionKey, string rowKey, int maxRecords)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");
         CloudTable table = GetTable(tableName, false);
         if (table == null) return null;

         var query = new TableQuery();

         var filters = new List<string>();

         if (partitionKey != null)
         {
            filters.Add(TableQuery.GenerateFilterCondition(
               PartitionKeyName,
               QueryComparisons.Equal,
               EncodeKey(partitionKey)));
         }

         if (rowKey != null)
         {
            filters.Add(TableQuery.GenerateFilterCondition(
               RowKeyName,
               QueryComparisons.Equal,
               EncodeKey(rowKey)));
         }

         if (filters.Count > 0)
         {
            string finalFilter = filters.First();

            for (int i = 1; i < filters.Count; i++)
            {
               finalFilter = TableQuery.CombineFilters(finalFilter, TableOperators.And, filters[i]);
            }

            query = query.Where(finalFilter);
         }

         if (maxRecords > 0)
         {
            query = query.Take(maxRecords);
         }

         IEnumerable<DynamicTableEntity> entities = table.ExecuteQuery(query);
         return entities.Select(ToTableRow);
      }

      public void Insert(string tableName, IEnumerable<TableRow> rows)
      {
         BatchedOperation(tableName, true,
            (b, te) => b.InsertOrReplace(te),
            rows);
      }

      public void Insert(string tableName, TableRow row)
      {
         Insert(tableName, new[] { row });
      }

      public void Update(string tableName, IEnumerable<TableRow> rows)
      {
         BatchedOperation(tableName, false,
            (b, te) => b.Replace(te),
            rows);
      }

      public void Update(string tableName, TableRow row)
      {
         Update(tableName, new[] { row });
      }

      public void Merge(string tableName, IEnumerable<TableRow> rows)
      {
         BatchedOperation(tableName, true,
            (b, te) => b.InsertOrMerge(te),
            rows);
      }

      public void Merge(string tableName, TableRow row)
      {
         Merge(tableName, new[] { row });
      }

      public void Delete(string tableName, IEnumerable<TableRowId> rowIds)
      {
         BatchedOperation(tableName, true,
            (b, te) => b.Delete(te),
            rowIds);
      }

      public void Delete(string tableName, TableRowId rowId)
      {
         Delete(tableName, new[] { rowId });
      }

      private void BatchedOperation(string tableName, bool createTable,
         Action<TableBatchOperation, IAzTableEntity> azAction,
         IEnumerable<TableRow> rows)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");
         if (rows == null) return;

         CloudTable table = GetTable(tableName, createTable);
         if (table == null) return;

         foreach (var group in rows.GroupBy(e => e.PartitionKey))
         {
            foreach (IEnumerable<TableRow> chunk in group.Chunk(MaxInsertLimit))
            {
               if (chunk == null) break;

               var chunkLst = new List<TableRow>(chunk);
               var batch = new TableBatchOperation();
               foreach (TableRow row in chunkLst)
               {
                  azAction(batch, new EntityAdapter(row));
               }

               List<TableResult> result = ExecOrThrow(table, batch);
               for (int i = 0; i < result.Count && i < chunkLst.Count; i++)
               {
                  TableResult tr = result[i];
                  TableRow row = chunkLst[i];

                  row.Id.ConcurrencyKey = tr.Etag;
               }
            }
         }
      }

      private void BatchedOperation(string tableName, bool createTable,
         Action<TableBatchOperation, IAzTableEntity> azAction,
         IEnumerable<TableRowId> rowIds)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");
         if (rowIds == null) return;

         CloudTable table = GetTable(tableName, createTable);
         if (table == null) return;

         foreach (var group in rowIds.GroupBy(e => e.PartitionKey))
         {
            foreach (IEnumerable<TableRowId> chunk in group.Chunk(MaxInsertLimit))
            {
               if (chunk == null) break;

               var batch = new TableBatchOperation();
               foreach (TableRowId row in chunk)
               {
                  azAction(batch, new EntityAdapter(row));
               }

               ExecOrThrow(table, batch);
            }
         }
      }

      private CloudTable GetTable(string name, bool createIfNotExists)
      {
         if (name == null) throw new ArgumentNullException("name");
         if (!TableNameRgx.IsMatch(name)) throw new ArgumentException(@"invalid table name: " + name, "name");

         lock (RefLock)
         {
            TableTag tag;
            bool cached = TableNameToTableTag.TryGetValue(name, out tag);

            if (!cached)
            {
               tag = new TableTag
               {
                  Table = _client.GetTableReference(name),
               };
               tag.Exists = tag.Table.Exists();
               TableNameToTableTag[name] = tag;
            }

            if (!tag.Exists && createIfNotExists)
            {
               tag.Table.Create();
               tag.Exists = true;
            }

            if (!tag.Exists) return null;
            return tag.Table;
         }
      }

      private List<TableResult> ExecOrThrow(CloudTable table, TableBatchOperation op)
      {
         try
         {
            return table.ExecuteBatch(op).ToList();
         }
         catch (AzSE ex)
         {
            throw new MeSE(ex.Message, ex);
         }
      }

      private class EntityAdapter : IAzTableEntity
      {
         private readonly TableRow _row;

         public EntityAdapter(TableRow row) : this(row == null ? null : row.Id)
         {
            _row = row;
         }

         public EntityAdapter(TableRowId rowId)
         {
            if (rowId == null) throw new ArgumentNullException("rowId");

            PartitionKey = ToInternalId(rowId.PartitionKey);
            RowKey = ToInternalId(rowId.RowKey);
            ETag = rowId.ConcurrencyKey ?? "*";
            Timestamp = rowId.LastModified;
         }

         public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
         {
            throw new NotSupportedException();
         }

         public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
         {
            //Azure Lib calls this when it wants to transform this entity to a writeable one

            var dic = new Dictionary<string, EntityProperty>();
            foreach (KeyValuePair<string, TableCell> cell in _row)
            {
               dic[cell.Key] = EntityProperty.GeneratePropertyForString(cell.Value);
            }
            return dic;
         }

         public string PartitionKey { get; set; }
         public string RowKey { get; set; }
         public DateTimeOffset Timestamp { get; set; }
         public string ETag { get; set; }

         private static string ToInternalId(string userId)
         {
            return HttpUtility.UrlEncode(userId);
         }
      }

      private static TableRow ToTableRow(DynamicTableEntity az)
      {
         var result = new TableRow(az.PartitionKey, az.RowKey);
         result.Id.ConcurrencyKey = az.ETag;
         result.Id.LastModified = az.Timestamp;
         foreach (KeyValuePair<string, EntityProperty> pair in az.Properties)
         {
            result[pair.Key] = pair.Value.StringValue;
         }
         return result;
      }

      private static string EncodeKey(string key)
      {
         //todo: read more: https://msdn.microsoft.com/library/azure/dd179338.aspx
         return HttpUtility.UrlEncode(key);
      }
   }
}
