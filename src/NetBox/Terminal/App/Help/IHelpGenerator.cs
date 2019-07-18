namespace NetBox.Terminal.App.Help
{
   public interface IHelpGenerator
   {
      void GenerateHelp(Application app);

      void GenerateHelp(Command cmd);
   }
}