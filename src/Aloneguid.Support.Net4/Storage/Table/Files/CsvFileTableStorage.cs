using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aloneguid.Support.Application.FileFormats;

namespace Aloneguid.Support.Storage.Table.Files
{
   /// <summary>
   /// Creates an abstaction of <see cref="ITableStorage"/> in a CSV file structure.
   /// Works relative to the root directory specified in the constructor.
   /// Each table will be a separate subfolder, where files are partitions.
   /// </summary>
   public class CsvFileTableStorage : ITableStorage
   {
      private const string TablePartitionFormat = "{0}.partition.csv";
      private const string TableNsFormat = "{0}.table";
      private const string TableNamesSuffix = ".table";
      private const string TableNamesSearchPattern = "*.table";
      private static readonly char[] Kvs = {':'};
      private readonly DirectoryInfo _rootDir;
      private readonly string _rootDirPath;

      /// <summary>
      /// Creates a new instance of CSV file storage
      /// </summary>
      /// <param name="rootDir"></param>
      /// <exception cref="ArgumentNullException"></exception>
      public CsvFileTableStorage(DirectoryInfo rootDir)
      {
         if(rootDir == null) throw new ArgumentNullException(nameof(rootDir));

         _rootDir = rootDir;
         _rootDirPath = rootDir.FullName;
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
      public bool HasOptimisticConcurrency
      {
         get { return false; }
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
      public IEnumerable<string> ListTableNames()
      {
         return _rootDir
            .GetDirectories(TableNamesSearchPattern, SearchOption.TopDirectoryOnly)
            .Select(d => d.Name.Substring(0, d.Name.Length - TableNamesSuffix.Length));
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
      public void Delete(string tableName)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));

         DirectoryInfo table = OpenTable(tableName, false);
         table?.Delete(true);
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
      public IEnumerable<TableRow> Get(string tableName, string partitionKey)
      {
         if(tableName == null) throw new ArgumentNullException(nameof(tableName));
         if(partitionKey == null) throw new ArgumentNullException(nameof(partitionKey));

         return ReadPartition(tableName, partitionKey).Values;
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
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

      /// <summary>
      /// See interface documentation
      /// </summary>
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

      /// <summary>
      /// See interface documentation
      /// </summary>
      public void Insert(string tableName, TableRow row)
      {
         Insert(tableName, new[] {row});
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
      public void Update(string tableName, IEnumerable<TableRow> rows)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
      public void Update(string tableName, TableRow row)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
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

      /// <summary>
      /// See interface documentation
      /// </summary>
      public void Merge(string tableName, TableRow row)
      {
         Merge(tableName, new[] {row});
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
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

      /// <summary>
      /// See interface documentation
      /// </summary>
      public void Delete(string tableName, TableRowId rowId)
      {
         Delete(tableName, new[] {rowId});
      }

      /// <summary>
      /// See interface documentation
      /// </summary>
      public IEnumerable<TableRow> Get(string tableName, string partitionKey, string rowKey, int maxRecords)
      {
         throw new NotImplementedException();
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
         if(createIfNotExists)
         {
            if(!_rootDir.Exists) _rootDir.Create();

            var result = new DirectoryInfo(Path.Combine(_rootDirPath, string.Format(TableNsFormat, name).SanitizePath()));
            if(!result.Exists) result.Create();

            return result;
         }

         if(!_rootDir.Exists) return null;
         var dir = new DirectoryInfo(Path.Combine(_rootDirPath, string.Format(TableNsFormat, name).SanitizePath()));
         if(!dir.Exists) return null;
         return dir;
      }

      private Stream OpenTablePartition(string tableName, string partitionName, bool createIfNotExists)
      {
         DirectoryInfo fs = OpenTable(tableName, createIfNotExists);
         if(fs == null) return null;

         string partitionPath = Path.Combine(fs.FullName,
            string.Format(TablePartitionFormat, partitionName).SanitizePath());

         if(!File.Exists(partitionPath))
         {
            if(!createIfNotExists) return null;
            return File.OpenWrite(partitionPath);
         }

         return File.Open(partitionPath, FileMode.Open, FileAccess.ReadWrite);
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
               var values = new List<string>(row.Values.Count + 1) {row.RowKey};
               values.AddRange(row.Select(r => r.Value.RawValue));
               writer.Write(values);
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
