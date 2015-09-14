using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aloneguid.Support.Application.FileFormats;

namespace Aloneguid.Support.Storage.Table.Files
{
   public class CsvFileTableStorage : ISimpleTableStorage
   {
      private const string TablePartitionFormat = "{0}.partition.csv";
      private const string TableNsFormat = "{0}.table";
      private const string TableNamesSuffix = ".table";
      private const string TableNamesSearchPattern = "*.table";
      private static readonly char[] Kvs = {':'};
      private readonly DirectoryInfo _rootDir;

      public CsvFileTableStorage(DirectoryInfo rootDir)
      {
         if(rootDir == null) throw new ArgumentNullException(nameof(rootDir));

      }

      public bool HasOptimisticConcurrency
      {
         get { return false; }
      }

      public IEnumerable<string> ListTableNames()
      {
         return _rootDir
            .GetDirectories(TableNamesSearchPattern, SearchOption.TopDirectoryOnly)
            .Select(d => d.Name);
      }

      public void Delete(string tableName)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));

         string ns = string.Format(TableNsFormat, tableName);

         string fullPath = Path.Combine(_rootDir.FullName, tableName);
         Directory.Delete(fullPath);
      }

      public IEnumerable<TableRow> Get(string tableName, string partitionKey)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));
         if(partitionKey == null) throw new ArgumentNullException(nameof(partitionKey));

         return ReadPartition(tableName, partitionKey).Values;
      }

      public TableRow Get(string tableName, string partitionKey, string rowKey)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));
         if(partitionKey == null) throw new ArgumentNullException(nameof(partitionKey));
         if(rowKey == null) throw new ArgumentNullException(nameof(rowKey));

         Dictionary<string, TableRow> rows = ReadPartition(tableName, partitionKey, rowKey);
         TableRow result;
         rows.TryGetValue(rowKey, out result);
         return result;
      }

      public void Insert(string tableName, IEnumerable<TableRow> rows)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));
         if(rows == null) return;

         foreach(var group in rows.GroupBy(r => r.PartitionKey))
         {
            string partitionKey = group.Key;

            Dictionary<string, TableRow> partition = ReadPartition(tableName, partitionKey);
            if(partition == null) partition = new Dictionary<string, TableRow>();
            Insert(partition, group);
            WritePartition(tableName, partitionKey, partition.Values);
         }
      }

      public void Insert(string tableName, TableRow row)
      {
         Insert(tableName, new[] {row});
      }

      public void Update(string tableName, IEnumerable<TableRow> rows)
      {
         throw new NotImplementedException();
      }

      public void Update(string tableName, TableRow row)
      {
         throw new NotImplementedException();
      }

      public void Merge(string tableName, IEnumerable<TableRow> rows)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));
         if(rows == null) return;

         foreach(var group in rows.GroupBy(r => r.PartitionKey))
         {
            string partitionKey = group.Key;

            Dictionary<string, TableRow> partition = ReadPartition(tableName, partitionKey);
            if(partition == null) partition = new Dictionary<string, TableRow>();
            Merge(partition, group);
            WritePartition(tableName, partitionKey, partition.Values);
         }
      }

      public void Merge(string tableName, TableRow row)
      {
         Merge(tableName, new[] {row});
      }

      public void Delete(string tableName, IEnumerable<TableRowId> rowIds)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));
         if(rowIds == null) return;

         foreach(var group in rowIds.GroupBy(r => r.PartitionKey))
         {
            string partitionKey = group.Key;

            Dictionary<string, TableRow> partition = ReadPartition(tableName, partitionKey);
            if(partition == null) partition = new Dictionary<string, TableRow>();
            Delete(partition, group);
            WritePartition(tableName, partitionKey, partition.Values);
         }
      }

      public void Delete(string tableName, TableRowId rowId)
      {
         Delete(tableName, new[] {rowId});
      }

      private string TableNameFromDirName(string dirName)
      {
         if(dirName == null) return null;

         int idx = dirName.IndexOf(TableNamesSuffix, StringComparison.Ordinal);
         if(idx == -1) return null;

         return dirName.Substring(0, idx);
      }

      #region [ Big Data Processing ]

      private void Insert(Dictionary<string, TableRow> data, IEnumerable<TableRow> rows)
      {
         foreach(TableRow row in rows)
         {
            data[row.RowKey] = row;
         }
      }

      private void Merge(Dictionary<string, TableRow> data, IEnumerable<TableRow> rows)
      {
         foreach(TableRow row in rows)
         {
            TableRow prow;
            if(!data.TryGetValue(row.RowKey, out prow))
            {
               data[row.RowKey] = row;
            }
            else
            {
               foreach(KeyValuePair<string, TableCell> line in row)
               {
                  prow[line.Key] = line.Value;
               }
            }
         }
      }

      private void Delete(Dictionary<string, TableRow> data, IEnumerable<TableRowId> rows)
      {
         foreach(TableRowId row in rows)
         {
            data.Remove(row.RowKey);
         }
      }

      #endregion

      #region [ Disk Partitioning ]

      private DirectoryInfo OpenTable(string name, bool createIfNotExists)
      {
         string ns = string.Format(TableNsFormat, name);
         var dir = new DirectoryInfo(Path.Combine(_rootDir.FullName, ns));
         return dir;
      }

      private Stream OpenTablePartition(string tableName, string partitionName, bool createIfNotExists)
      {
         DirectoryInfo fs = OpenTable(tableName, createIfNotExists);
         if(fs == null) return null;

         string pfn = string.Format(TablePartitionFormat, partitionName);
         if(!fs.Exists(pfn))
         {
            if(!createIfNotExists) return null;
            return fs.CreateWriteableStream(pfn);
         }

         return fs.OpenFile(pfn, true);
      }

      #endregion

      #region [ CSV Read & Write ]

      private void WritePartition(string tableName, string partitionName, IEnumerable<TableRow> rows)
      {
         using(Stream s = OpenTablePartition(tableName, partitionName, true))
         {
            s.Seek(0, SeekOrigin.Begin);
            s.SetLength(0);
            var writer = new CsvWriter(s, Encoding.UTF8);

            foreach(TableRow row in rows)
            {
               var cols = new List<string>(row.Count + 1);
               cols.Add(row.RowKey);
               cols.AddRange(row.Select(cv => string.Format("{0}:{1}", cv.Key, cv.Value)));
               writer.Write(cols);
            }
         }
      }

      private Dictionary<string, TableRow> ReadPartition(string tableName, string partitionName, string stopOnRowKey = null)
      {
         using(Stream s = OpenTablePartition(tableName, partitionName, false))
         {
            if(s == null) return null;

            var result = new Dictionary<string, TableRow>();
            IEnumerable<string> rr;
            var reader = new CsvReader(s, Encoding.UTF8);
            while((rr = reader.ReadNextRow()) != null)
            {
               TableRow row = null;
               foreach(string r in rr)
               {
                  if(row == null)
                  {
                     row = new TableRow(partitionName, r);
                  }
                  else
                  {
                     string[] keyValue = r.Split(Kvs, 2);
                     if(keyValue.Length == 2)
                     {
                        row[keyValue[0]] = keyValue[1];
                     }
                  }
               }

               if(row != null)
               {
                  result[row.RowKey] = row;
                  if(stopOnRowKey != null && row.RowKey == stopOnRowKey) break;
               }
            }

            return result;
         }
      }

      #endregion

   }
}
