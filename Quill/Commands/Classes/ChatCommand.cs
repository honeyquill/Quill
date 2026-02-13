using System;

namespace Quill
{
    public class ChatCommand
    {
        public string Name { get; }
        public string Description { get; }
        public Action<string[]> Execute { get; }

        public int MinArgs { get; }

        protected ChatCommand(string name, string description, Action<string[]> execute, int minArgs)
        {
            Name = name;
            Description = description;
            Execute = execute;
            MinArgs = minArgs;
        }
    }
}