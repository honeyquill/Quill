using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;
using static Quill.BeetleUtils;

namespace Quill
{
    public class CommandManager
    {
        private string _newestMsg;
        private const char CommandPrefix = '!';
        
        private readonly Dictionary<string, ChatCommand> _commands =
            new Dictionary<string, ChatCommand>();
        
        public void RegisterCommand(string command, ChatCommand chatCommand)
        {
            _commands.Add(command.ToLower(), chatCommand);
            MelonLogger.Msg("Command " + chatCommand.Name + " registered!");
        }
        
        public void CommandHandler()
        {
            var result = GetNewestCommand();
            if (result == null) return;
            var (commandName, args) = result.Value;

            switch (commandName)
            {
                case null:
                    return;
                case "help":
                {
                    foreach (var command in _commands.Select(kvp => kvp.Value))
                    {
                        SendChatMessage($"{command.Name} - {command.Description}");
                    }

                    return;
                }
            }

            if (_commands.TryGetValue(commandName, out var commandAction))
            {
                commandAction.Execute(args);
            }
            else
            {
                SendChatMessage("Unknown command: " + commandName);
            }
        }
        
        private (string command, string[] args)? GetNewestCommand()
        {
            var history = GetChatHistory();
            if (history == null || history.Count == 0)
                return null;

            var newestIndex = history.Count - 1;
            var lastMessage = _newestMsg;
            _newestMsg = history[newestIndex].message?.ToLower();
            Debug.Assert(_newestMsg != null, nameof(_newestMsg) + " != null");

            if (_newestMsg == lastMessage) return null;
            if (_newestMsg.ToCharArray()[0] != CommandPrefix) return null;

            var withoutPrefix = _newestMsg.Substring(1);

            var parts = withoutPrefix
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var commandName = parts[0];
            var args = parts.Length > 1
                ? parts.Skip(1).ToArray()
                : Array.Empty<string>();

            MelonLogger.Msg("Command identified:" + _newestMsg);

            return (commandName, args);
        }
    }
}