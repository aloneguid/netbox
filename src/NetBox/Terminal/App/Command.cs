using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetBox.Terminal.App.Help;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Terminal.App
{
   /// <summary>
   /// Describes application command
   /// </summary>
   public class Command
   {
      private Func<Task> _onExecuteAsyncMethod;
      private Action _onExecuteMethod;
      private ConsoleArguments _args;
      private readonly List<LinePrimitive> _arguments = new List<LinePrimitive>();
      private readonly List<LinePrimitive> _options = new List<LinePrimitive>();
      private readonly Application _app;

      internal Command(Application app, string name)
      {
         _app = app;
         Name = name;
      }

      public string Name { get; set; }

      public string Description { get; set; }

      public IReadOnlyCollection<LinePrimitive> Arguments => _arguments;

      public IReadOnlyCollection<LinePrimitive> Options => _options;

      public Command OnExecute(Func<Task> onExecuteMethod)
      {
         _onExecuteAsyncMethod = onExecuteMethod;

         return this;
      }

      public Command OnExecute(Action onExecuteMethod)
      {
         _onExecuteMethod = onExecuteMethod;

         return this;
      }

      /// <summary>
      /// Adds a positional argument
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="name"></param>
      /// <param name="description"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      public LinePrimitive<T> Argument<T>(string name, string description, T defaultValue = default(T))
      {
         var arg = new LinePrimitive<T>(false, name, description, defaultValue);
         arg.Command = this;
         _arguments.Add(arg);
         return arg;
      }

      /// <summary>
      /// Adds an option
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="spec"></param>
      /// <param name="description"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      public LinePrimitive<T> Option<T>(string spec, string description, T defaultValue = default(T))
      {
         var op = new LinePrimitive<T>(true, spec, description, defaultValue);
         op.Command = this;
         _options.Add(op);
         return op;
      }

      internal void Add(LinePrimitive lp)
      {
         _options.Add(lp);
      }

      internal int Execute(ConsoleArguments arguments, Action<Command> onBeforeExecuteCommand)
      {
         //make sure options have a command attached
         foreach(LinePrimitive opt in _options)
         {
            opt.Command = this;
         }

         if(arguments.HasOnlyHelpSwitch())
         {
            IHelpGenerator help = new ConsoleHelpGenerator();
            help.GenerateHelp(this);
            return 0;
         }

         _args = arguments;

         onBeforeExecuteCommand?.Invoke(this);

         try
         {
            if(_onExecuteMethod != null)
            {
               _onExecuteMethod();
            }
            else if(_onExecuteAsyncMethod != null)
            {
               _onExecuteAsyncMethod().Wait();
            }
            else
            {
               WriteLine(Strings.Error_CommandHasNoExecute, T.ErrorTextColor);
               return 1;
            }

            return 0;
         }
         catch(ArgValidationException ex)
         {
            Write(ex.ParamName, T.ErrorTextColor);
            Write(": ");
            WriteLine(ex.OriginalMessage);

            return 1;
         }
         catch(Exception ex)
         {
            bool continueErrorHandling = _app.RaiseError(this, ex);

            if (continueErrorHandling)
            {
               WriteLine(ex.Message, T.ErrorTextColor);
            }

            return 1;
         }
      }

      internal string GetArgument(LinePrimitive argument)
      {
         int index = _arguments.IndexOf(argument);

         return _args.GetArgument(index);
      }

      internal string GetOption(LinePrimitive option, bool isSwitch)
      {
         return _args.GetOption(isSwitch, option.Name);
      }
   }
}
