namespace Cook.Cli.Commands
{
    using System.CommandLine;

    internal class EntryCommand : RootCommand
    {
        public EntryCommand() : base("Developer tool for cooking various breakfast, lunch and supper meals.")
        {
            Name = "Cook";
        }
    }
}
