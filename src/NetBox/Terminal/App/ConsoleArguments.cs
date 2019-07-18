using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBox.Terminal.App
{
   class ConsoleArguments
   {
      private readonly List<string> _arguments = new List<string>();

      public ConsoleArguments()
      {
         //first argument is process name so skip it
         _arguments.AddRange(Environment.GetCommandLineArgs().Skip(1));
      }

      public ConsoleArguments(IEnumerable<string> args)
      {
         _arguments.AddRange(args);
      }

      public string CommandName => _arguments.Count > 0 ? _arguments[0] : null;

      public string GetArgument(int position)
      {
         int i = -1;
         foreach(string arg in _arguments)
         {
            if(!arg.StartsWith("-"))
            {
               i += 1;
               if (position == i)
                  return arg;
            }
         }

         return null;
      }

      public string GetOption(bool isSwitch, string specs)
      {
         string[] specsArray = specs.Split('|');
         for(int i = 0; i < _arguments.Count; i++)
         {
            string p = _arguments[i];
            foreach(string spec in specsArray)
            {
               if(spec == p)
               {
                  if(isSwitch)
                  {
                     return p;
                  }
                  else
                  {
                     if (i + 1 == _arguments.Count)
                        return null;

                     if (_arguments[i + 1].StartsWith("-"))
                        return null;


                     return _arguments[i + 1];
                  }
               }
            }
         }

         return null;
      }

      public string GetArgument(string spec)
      {
         string[] specar = spec.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

         for(int i = 0; i < _arguments.Count; i++)
         {
            string name = _arguments[i];
            
            foreach(string specItem in specar)
            {
               if(name == specItem)
               {
                  if(i + 1 < _arguments.Count)
                  {
                     return _arguments[i + 1];
                  }
               }
            }
         }

         return null;
      }

      public int Count => _arguments.Count;

      public bool HasOnlyHelpSwitch()
      {
         if (_arguments.Count != 1)
            return false;

         string arg = _arguments[0];

         return arg == "-h" || arg == "--help";
      }

      public ConsoleArguments WithoutCommand()
      {
         if (_arguments.Count == 0)
            return this;

         return new ConsoleArguments(_arguments.Skip(1));
      }
   }
}
