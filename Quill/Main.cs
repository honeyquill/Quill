using System.Net;
using Il2Cpp;
using static Quill.BeetleUtils;
using MelonLoader;
using Quill.Commands;
using UnityEngine;
using Main = Quill.Main;


[assembly: MelonInfo(typeof(Main), "Quill", "1.0", "Bee")]


namespace Quill
{
    public class Main : MelonMod
    {
        public class PlayerData
        {
            public ulong NetworkID;
            public BeetleType BeetleID;
            public TeamType team;
            public Vector3 Position;
            public Quaternion Rotation;
        }
        public class DungData
        {
            public int Size;
            public Vector3 Position;
        }
        private static CommandManager CommandManager { get; } = new CommandManager();
        public static BeetleRegistry BeetleRegistry { get; } = new BeetleRegistry();

        public static bool _quillEnabled = false;
        public static bool inCustom = false;


        public override void OnInitializeMelon()
        {
            CommandManager.RegisterCommand("dung", new SpawnDung());
            CommandManager.RegisterCommand("bunny", new BunnyModify());
            CommandManager.RegisterCommand("cooldowns", new FastCooldowns());
            CommandManager.RegisterCommand("clear", new ClearDung());
            CommandManager.RegisterCommand("beetle", new ChangeBeetle());
            CommandManager.RegisterCommand("goal", new DisableScoring());
            CommandManager.RegisterCommand("ss", new SaveStates());

            BeetleRegistry.RegisterNameToIdCache();
        }

        public override void OnUpdate()
        {
            if (!IsHost())
            {
                _quillEnabled = false;
                return;
            }

            switch (GameState.CurrentState)
            {
                case GameState.State.Lobby_Custom:
                    inCustom = true;
                    break;
                case GameState.State.MainMenu:
                    inCustom = false;
                    break;
                case GameState.State.Lobby_PreMatch:
                    inCustom = true;
                    break;
            }


            if (!inCustom) return;
            if (!_quillEnabled) SendChatMessage($"This server is running {Info.Name} {Info.Version} by {Info.Author}");
            _quillEnabled = true;

            CommandManager.CommandHandler();
        }



    }
} 