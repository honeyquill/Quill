using System;

namespace Quill
{
    public class ChatCommand
    {
        public string Name;
        public string Description;
        private readonly Action<string[], string> _execute;
        private readonly Action? _Update;

        public ChatCommand(string name, string description, Action<string[], string> execute, int args, Action? Update = null)
        {
            Name = name;
            Description = description;
            _execute = execute;
            _Update = Update;
        }

        public void Execute(string[] args, string player)
        {
            _execute(args, player);
        }
        public void Update()
        {
            _Update?.Invoke();
        }
    }
}