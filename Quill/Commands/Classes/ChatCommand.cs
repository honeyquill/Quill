using System;

namespace Quill
{
    public class ChatCommand
    {
        public string Name;
        public string Description;
        private readonly Action<string[], string> _execute;

        public ChatCommand(string name, string description, Action<string[], string> execute, int args)
        {
            Name = name;
            Description = description;
            _execute = execute;
        }

        public void Execute(string[] args, string player)
        {
            _execute(args, player);
        }
    }
}