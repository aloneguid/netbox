using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NetBox.Terminal.App.Help;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Terminal.App
{
   /// <summary>
   /// Console application description
   /// </summary>
   public class Application
   {
      private readonly Dictionary<string, Command> _commandNameToCommand = new Dictionary<string, Command>();
      private readonly string _name;
      private Func<Command, Exception, bool> _onErrorMethod;
      private Func<Command, Exception, Task<bool>> _onErrorMethodAsync;
      private Action<Command> _onBeforeExecuteCommand;

      public Application(string name) : this()
      {
         _name = name;
      }

      private Application()
      {
         AssemblyInformationalVersionAttribute infoAttr = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();

         if (infoAttr == null)
         {
            Version = "unknown";
         }
         else
         {
            Version = infoAttr.InformationalVersion;
         }
      }

      public Command Command(string commandName, Action<Command> init)
      {
         if (commandName == null) throw new ArgumentNullException(nameof(commandName));
         if (init == null) throw new ArgumentNullException(nameof(init));

         var command = new Command(this, commandName);

         foreach(LinePrimitive so in SharedOptions)
         {
            //these are detached still
            command.Add(so);
         }

         _commandNameToCommand[commandName] = command;
         init(command);
         return command;
      }

      public string Name => _name;

      public string Version { get; }

      internal ICollection<LinePrimitive> SharedOptions { get; } = new List<LinePrimitive>();

      public IReadOnlyCollection<Command> Commands => new List<Command>(_commandNameToCommand.Values);

      public void OnError(Func<Command, Exception, bool> onErrorMethod)
      {
         _onErrorMethod = onErrorMethod;
      }

      public void OnBeforeExecuteCommand(Action<Command> onBeforeExecuteCommand)
      {
         _onBeforeExecuteCommand = onBeforeExecuteCommand;
      }

      public void OnError(Func<Command, Exception, Task<bool>> onErrorMethod)
      {
         _onErrorMethodAsync = onErrorMethod;
      }

      public LinePrimitive<T> SharedOption<T>(string spec, string description, T defaultValue = default(T))
      {
         var op = new LinePrimitive<T>(true, spec, description, defaultValue);
         SharedOptions.Add(op);
         return op;
      }

      public int Execute()
      {
         if (_commandNameToCommand.Count == 0)
         {
            WriteLine(Strings.Error_NoCommands, T.ErrorTextColor);
            return 1;
         }

         var args = new ConsoleArguments();

         if (args.HasOnlyHelpSwitch() || args.CommandName == null)
         {
            IHelpGenerator help = new ConsoleHelpGenerator();
            help.GenerateHelp(this);
            return 0;
         }

         if (!_commandNameToCommand.TryGetValue(args.CommandName, out Command command))
         {
            WriteLine(string.Format(Strings.Error_UnknownCommand, args.CommandName), T.ErrorTextColor);
            return 1;
         }

         return command.Execute(args.WithoutCommand(), _onBeforeExecuteCommand);
      }

      internal bool RaiseError(Command cmd, Exception ex)
      {
         if(_onErrorMethod != null)
         {
            return _onErrorMethod(cmd, ex);
         }
         else if(_onErrorMethodAsync != null)
         {
            return _onErrorMethodAsync(cmd, ex).Result;
         }

         return true;
      }
   }
}