using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal.Widgets
{
   public class Table
   {
      private readonly string[] _columnNames;
      private List<object[]> _rows = new List<object[]>();

      public Table(params string[] columnNames)
      {
         _columnNames = columnNames;
      }

      public void AddRow(params object[] values)
      {
         _rows.Add(values);
      }

      public void Render(bool printColumnNames, int leftPad = 0, params ConsoleColor[] columnColors)
      {
         //get max length for each column
         int[] widths = new int[_columnNames.Length];

         for(int i = 0; i < _columnNames.Length; i++)
         {
            int max = printColumnNames ? _columnNames[i].Length : -1;

            foreach(object[] row in _rows)
            {
               int l = row[i].ToString().Length;
               if (l > max) max = l;
            }

            widths[i] = max;
         }

         //draw header
         if(printColumnNames)
         {
            //todo
         }

         foreach(object[] row in _rows)
         {
            for(int i = 0; i < _columnNames.Length; i++)
            {
               if(leftPad > 0 && i == 0)
               {
                  Console.Write(new string(' ', leftPad));
               }

               string v = row[i]?.ToString() ?? string.Empty;
               v = v.PadRight(widths[i]);

               ConsoleColor color = (columnColors != null && i < columnColors.Length)
                  ? columnColors[i]
                  : PoshConsole.T.NormalTextColor;

               PoshConsole.Write(v, color);
               Console.Write(" ");
            }

            Console.WriteLine();
         }
      }
   }
}
